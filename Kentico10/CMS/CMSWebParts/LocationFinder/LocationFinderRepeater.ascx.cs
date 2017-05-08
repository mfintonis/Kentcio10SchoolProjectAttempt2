using CMS.PortalEngine.Web.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using LocationFinder;
using CMS.EventLog;
using CMS.Helpers;

public partial class CMSWebParts_LocationFinder_LocationFinderRepeater : CMSAbstractWebPart
{
    public List<LocationInfo> locations;
    public double[] arrLongitudes;
    public double[] arrLatitudes;
    public string[] arrTitles;

    public int ZoomLevel
    {
        get
        {
            return ValidationHelper.GetInteger(GetValue("ZoomLevel"), 6);
        }
        set
        {
            SetValue("ZoomLevel", value);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        locations = LocationInfoProvider.GetLocations().WhereEquals("LocationActive", true).OrderBy("LocationOrder").ToList();

        if (locations != null && locations.Count > 0)
        {
            arrLongitudes = new double[locations.Count];
            arrLatitudes = new double[locations.Count];
            arrTitles = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++)
            {
                arrLongitudes[i] = Convert.ToDouble(locations[i].LocationLongitude);
                arrLatitudes[i] = Convert.ToDouble(locations[i].LocationLatitude);
                arrTitles[i] = locations[i].LocationName;
            }

            rptLocatioons.DataSource = locations;
            rptLocatioons.DataBind();
        }
    }

    public static string Serialize(object o)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(o);
    }

    public double GetAvgLatitude()
    {
        if(locations.Count > 0)
        {
            double sum = 0.0;
            foreach(var location in locations)
            {
                double latitude;
                if(Double.TryParse(location.LocationLatitude, out latitude))
                {
                    sum += latitude;
                }
                else
                {
                    EventLogProvider.LogEvent(EventType.ERROR, "Error Convertin to Double", "LOCATION_FINDER", eventDescription: "Error converting latitude to double on location object with ID: " + location.LocationID);
                }
            }
            return sum / locations.Count;
        }
        return 0.0;
    }

    public double GetAvgLongitude()
    {
        if (locations.Count > 0)
        {
            double sum = 0.0;
            foreach (var location in locations)
            {
                double longitude;
                if (Double.TryParse(location.LocationLongitude, out longitude))
                {
                    sum += longitude;
                }
                else
                {
                    EventLogProvider.LogEvent(EventType.ERROR, "Error Convertin to Double", "LOCATION_FINDER", eventDescription: "Error converting longitude to double on location object with ID: " + location.LocationID);
                }
            }
            return sum / locations.Count;
        }
        return 0.0;
    }
}