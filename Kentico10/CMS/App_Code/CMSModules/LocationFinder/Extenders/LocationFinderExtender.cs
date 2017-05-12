using CMS;
using CMS.Base.Web.UI;
using CMS.UIControls;
using LocationFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: RegisterCustomClass("LocationFinderExtender", typeof(LocationFinderExtender))]
public class LocationFinderExtender : ControlExtender<UniGrid>
{
    public override void OnInit()
    {
        Control.OnAction += Control_OnAction;
    }

    private void Control_OnAction(string actionName, object actionArgument)
    {
        if(actionName == "toggleLocationEnabled")
        {
            int id = Convert.ToInt32(actionArgument.ToString());
            var location = LocationInfoProvider.GetLocationInfo(id);
            if(location != null)
            {
                if(location.LocationActive == false)
                {
                    location.LocationActive = true;
                }
                else
                {
                    location.LocationActive = false;
                }
                LocationInfoProvider.SetLocationInfo(location);
            }
        }
    }
}