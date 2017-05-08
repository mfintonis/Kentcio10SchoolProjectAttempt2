using System;
using System.Data;

using CMS.Base;
using CMS.DataEngine;
using CMS.Helpers;
using System.Web;
using System.Xml;

namespace LocationFinder
{    
    /// <summary>
    /// Class providing LocationInfo management.
    /// </summary>
    public partial class LocationInfoProvider : AbstractInfoProvider<LocationInfo, LocationInfoProvider>
    {
        #region "Constructors"

        /// <summary>
        /// Constructor
        /// </summary>
        public LocationInfoProvider()
            : base(LocationInfo.TYPEINFO)
        {
        }

        #endregion


        #region "Public methods - Basic"

        /// <summary>
        /// Returns a query for all the LocationInfo objects.
        /// </summary>
        public static ObjectQuery<LocationInfo> GetLocations()
        {
            return ProviderObject.GetLocationsInternal();
        }


        /// <summary>
        /// Returns LocationInfo with specified ID.
        /// </summary>
        /// <param name="id">LocationInfo ID</param>
        public static LocationInfo GetLocationInfo(int id)
        {
            return ProviderObject.GetLocationInfoInternal(id);
        }


        /// <summary>
        /// Returns LocationInfo with specified name.
        /// </summary>
        /// <param name="name">LocationInfo name</param>
        public static LocationInfo GetLocationInfo(string name)
        {
            return ProviderObject.GetLocationInfoInternal(name);
        }


        /// <summary>
        /// Returns LocationInfo with specified GUID.
        /// </summary>
        /// <param name="guid">LocationInfo GUID</param>                
        public static LocationInfo GetLocationInfo(Guid guid)
        {
            return ProviderObject.GetLocationInfoInternal(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified LocationInfo.
        /// </summary>
        /// <param name="infoObj">LocationInfo to be set</param>
        public static void SetLocationInfo(LocationInfo infoObj)
        {
            ProviderObject.SetLocationInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes specified LocationInfo.
        /// </summary>
        /// <param name="infoObj">LocationInfo to be deleted</param>
        public static void DeleteLocationInfo(LocationInfo infoObj)
        {
            ProviderObject.DeleteLocationInfoInternal(infoObj);
        }


        /// <summary>
        /// Deletes LocationInfo with specified ID.
        /// </summary>
        /// <param name="id">LocationInfo ID</param>
        public static void DeleteLocationInfo(int id)
        {
            LocationInfo infoObj = GetLocationInfo(id);
            DeleteLocationInfo(infoObj);
        }

        #endregion


        #region "Internal methods - Basic"
	
        /// <summary>
        /// Returns a query for all the LocationInfo objects.
        /// </summary>
        protected virtual ObjectQuery<LocationInfo> GetLocationsInternal()
        {
            return GetObjectQuery();
        }    


        /// <summary>
        /// Returns LocationInfo with specified ID.
        /// </summary>
        /// <param name="id">LocationInfo ID</param>        
        protected virtual LocationInfo GetLocationInfoInternal(int id)
        {	
            return GetInfoById(id);
        }


        /// <summary>
        /// Returns LocationInfo with specified name.
        /// </summary>
        /// <param name="name">LocationInfo name</param>        
        protected virtual LocationInfo GetLocationInfoInternal(string name)
        {
            return GetInfoByCodeName(name);
        } 


        /// <summary>
        /// Returns LocationInfo with specified GUID.
        /// </summary>
        /// <param name="guid">LocationInfo GUID</param>
        protected virtual LocationInfo GetLocationInfoInternal(Guid guid)
        {
            return GetInfoByGuid(guid);
        }


        /// <summary>
        /// Sets (updates or inserts) specified LocationInfo.
        /// </summary>
        /// <param name="infoObj">LocationInfo to be set</param>        
        protected virtual void SetLocationInfoInternal(LocationInfo infoObj)
        {
            string address = infoObj.LocationStreetAddress + ", " + infoObj.LocationTown + ", " + infoObj.LocationState + ", " + infoObj.LocationZip;
            string urlAddress = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + HttpUtility.UrlEncode(address) + "&sensor=false";
            XmlDocument objXmlDocument = new XmlDocument();
            objXmlDocument.Load(urlAddress);
            XmlNodeList objXmlNodeList = objXmlDocument.SelectNodes("/GeocodeResponse/result/geometry/location");
            //Get Longitude
            infoObj.LocationLongitude = objXmlNodeList[0].ChildNodes.Item(1).InnerText;
            //Get Latitude
            infoObj.LocationLatitude = objXmlNodeList[0].ChildNodes.Item(0).InnerText;

            infoObj.LocationDirectionsUrl = "http://maps.google.com/?q=" + HttpUtility.UrlEncode(address);

            SetInfo(infoObj);
        }


        /// <summary>
        /// Deletes specified LocationInfo.
        /// </summary>
        /// <param name="infoObj">LocationInfo to be deleted</param>        
        protected virtual void DeleteLocationInfoInternal(LocationInfo infoObj)
        {
            DeleteInfo(infoObj);
        }	

        #endregion
    }
}