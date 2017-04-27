﻿using System;
using System.Data;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.Base.Web.UI;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.FormEngine.Web.UI;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Membership;
using CMS.Modules;
using CMS.SiteProvider;
using CMS.UIControls;


public partial class CMSModules_EventManager_Controls_EventList : CMSAdminControl
{
    #region "Private variables"

    private UserInfo currentUserInfo;
    private SiteInfo currentSiteInfo;
    private string mOrderBy = "EventDate DESC";
    private string mItemsPerPage = String.Empty;
    private string mEventScope = "all";
    private string mSiteName = String.Empty;
    private string mGMTTooltip;


    public CMSModules_EventManager_Controls_EventList()
    {
        UsePostBack = false;
        SelectedEventID = 0;
    }

    #endregion


    #region "Properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }


    /// <summary>
    /// Indicates if control is used on live site.
    /// </summary>
    public override bool IsLiveSite
    {
        get
        {
            return base.IsLiveSite;
        }
        set
        {
            plcMess.IsLiveSite = value;
            base.IsLiveSite = value;
        }
    }


    /// <summary>
    /// Indicates whether change location or postback if edit.
    /// </summary>
    public bool UsePostBack
    {
        get;
        set;
    }


    /// <summary>
    /// Site name filter.
    /// </summary>
    public string SiteName
    {
        get
        {
            return mSiteName;
        }
        set
        {
            mSiteName = value;
        }
    }


    /// <summary>
    /// ID of selected event.
    /// </summary>
    public int SelectedEventID
    {
        get;
        set;
    }


    /// <summary>
    /// Gets or sets the order by condition.
    /// </summary>
    public string OrderBy
    {
        get
        {
            return mOrderBy;
        }
        set
        {
            mOrderBy = value;
        }
    }


    /// <summary>
    /// Gets or sets the value of items per page.
    /// </summary>
    public string ItemsPerPage
    {
        get
        {
            return mItemsPerPage;
        }
        set
        {
            mItemsPerPage = value;
        }
    }


    /// <summary>
    /// Gets or sets event date filter.
    /// </summary>
    public string EventScope
    {
        get
        {
            return mEventScope;
        }
        set
        {
            mEventScope = value;
        }
    }


    /// <summary>
    /// Stop processing.
    /// </summary>
    public override bool StopProcessing
    {
        get
        {
            return base.StopProcessing;
        }
        set
        {
            base.StopProcessing = value;
            gridElem.StopProcessing = value;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!URLHelper.IsPostback())
        {
            drpEventScope.Items.Add(new ListItem(GetString("general.selectall"), "all"));
            drpEventScope.Items.Add(new ListItem(GetString("eventmanager.eventscopeupcoming"), "upcoming"));

            drpEventScope.SelectedValue = QueryHelper.GetString("scope", "all");
        }

        EventScope = drpEventScope.SelectedValue;

        btnFilter.Text = GetString("general.filter");

        gridElem.HideControlForZeroRows = false;
        gridElem.ZeroRowsText = GetString("Events_List.NoBookingEvent");
        gridElem.OnAction += gridElem_OnAction;

        if ((!RequestHelper.IsPostBack()) && (!string.IsNullOrEmpty(ItemsPerPage)))
        {
            gridElem.Pager.DefaultPageSize = ValidationHelper.GetInteger(ItemsPerPage, -1);
        }

        gridElem.OrderBy = OrderBy;
        gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;

        gridElem.GridName = UsePostBack ? "~/CMSModules/EventManager/Controls/Events_List_Control.xml" : "~/CMSModules/EventManager/Controls/Events_List.xml";

        SetWhereCondition();
    }


    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        ScriptHelper.RegisterModule(this, "CMS.EventManager/EventsGrid", new
        {
            GridSelector = "#" + gridElem.ClientID,
            PagesApplicationHash = ApplicationUrlHelper.GetApplicationHash("cms.content", "content"),
            EventDetailURL = UIContextHelper.GetElementUrl("CMS.EventManager", "DetailsEvent")
        });
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        SetWhereCondition();
        gridElem.ReloadData();
        base.ReloadData();
    }


    private void SetWhereCondition()
    {
        // Check existence of CMS.BookingEvent dataclass
        if (DataClassInfoProvider.GetDataClassInfo("CMS.BookingEvent") != null)
        {
            // Filter site name            
            string siteName = SiteName;
            if (siteName == String.Empty)
            {
                siteName = SiteContext.CurrentSiteName;
            }

            // If not show all
            if (siteName != TreeProvider.ALL_SITES)
            {
                gridElem.WhereCondition = "(NodeLinkedNodeID IS NULL AND NodeSiteID = " + SiteInfoProvider.GetSiteID(siteName) + ")";
            }
            else
            {
                gridElem.WhereCondition = "NodeLinkedNodeID IS NULL";
            }

            // Filter time interval
            if (EventScope == "all")
            {
                return;
            }

            string conditionOperator = (EventScope == "upcoming") ? ">=" : "<=";
            gridElem.WhereCondition = SqlHelper.AddWhereCondition(gridElem.WhereCondition, String.Format("EventDate {0} @Date", conditionOperator));

            QueryDataParameters parameters = new QueryDataParameters();
            parameters.Add("@Date", DateTime.Now);

            gridElem.QueryParameters = parameters;
        }
        else
        {
            // Document type with code name 'CMS.BookingEvent' does not exist
            ShowError(GetString("Events_List.NoBookingEventClass"));
        }
    }

    #endregion


    #region "Grid events"

    /// <summary>
    /// Manage if user edit event.
    /// </summary>
    /// <param name="actionName">Edit</param>
    /// <param name="actionArgument">Id of event</param>
    protected void gridElem_OnAction(string actionName, object actionArgument)
    {
        switch (actionName.ToLowerCSafe())
        {
            case "view":
                SelectedEventID = Convert.ToInt32(actionArgument);
                break;
        }
    }


    /// <summary>
    /// Handles data bound event.
    /// </summary>
    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        string result = string.Empty;
        DataRowView data;

        switch (sourceName.ToLowerCSafe())
        {
            case "view":
                {
                    CMSGridActionButton editButton = ((CMSGridActionButton)sender);
                    var dataItemRow = ((DataRowView)(((GridViewRow)(parameter)).DataItem)).Row;
                    var nodeID = dataItemRow.Field<Int32>("NodeID");

                    editButton.Attributes.Add("data-node-id", nodeID.ToString());
                    break;
                }

            // Create link to event document 
            case "documentname":
                {
                    data = (DataRowView)parameter;
                    string documentName = ValidationHelper.GetString(data["DocumentName"], String.Empty);
                    string culture = ValidationHelper.GetString(data["DocumentCulture"], String.Empty);
                    int nodeID = ValidationHelper.GetInteger(data["NodeID"], 0);

                    SiteInfo si = SiteInfoProvider.GetSiteInfo(data["NodeSiteID"].ToInteger(0));
                    if (si != null)
                    {
                        return "<a class=\"js-unigrid-action js-edit\" " +
                               "href=\"javascript:void(0)\" " +
                               "data-node-id=\"" + nodeID + "\" " +
                               "data-document-culture=\"" + culture + "\" >" + HTMLHelper.HTMLEncode(documentName) + "</a>";
                    }
                }
                return HTMLHelper.HTMLEncode(parameter.ToString());

            case "eventtooltip":
                data = (DataRowView)parameter;
                return UniGridFunctions.DocumentNameTooltip(data);

            case "eventdate":
            case "eventopenfrom":
            case "eventopento":
            case "eventdatetooltip":
            case "eventopenfromtooltip":
            case "eventopentotooltip":
                if (!String.IsNullOrEmpty(parameter.ToString()))
                {
                    if (currentUserInfo == null)
                    {
                        currentUserInfo = MembershipContext.AuthenticatedUser;
                    }
                    if (currentSiteInfo == null)
                    {
                        currentSiteInfo = SiteContext.CurrentSite;
                    }

                    if (sourceName.EndsWithCSafe("tooltip"))
                    {
                        return mGMTTooltip ?? (mGMTTooltip = TimeZoneHelper.GetUTCLongStringOffset(currentUserInfo, currentSiteInfo));
                    }

                    DateTime time = ValidationHelper.GetDateTime(parameter, DateTimeHelper.ZERO_TIME);
                    return TimeZoneHelper.ConvertToUserTimeZone(time, true, currentUserInfo, currentSiteInfo);
                }
                return result;
            case "eventenddate":
            case "eventenddatetooltip":
                data = (DataRowView)parameter;
                try
                {
                    parameter = data["eventenddate"];
                }
                catch
                {
                    parameter = null;
                }

                if ((parameter != null) && !String.IsNullOrEmpty(parameter.ToString()))
                {
                    if (currentUserInfo == null)
                    {
                        currentUserInfo = MembershipContext.AuthenticatedUser;
                    }
                    if (currentSiteInfo == null)
                    {
                        currentSiteInfo = SiteContext.CurrentSite;
                    }

                    if (sourceName.EndsWithCSafe("tooltip"))
                    {
                        return mGMTTooltip ?? (mGMTTooltip = TimeZoneHelper.GetUTCLongStringOffset(currentUserInfo, currentSiteInfo));
                    }

                    DateTime time = ValidationHelper.GetDateTime(parameter, DateTimeHelper.ZERO_TIME);
                    return TimeZoneHelper.ConvertToUserTimeZone(time, true, currentUserInfo, currentSiteInfo);
                }
                return result;
        }

        return parameter;
    }

    #endregion
}