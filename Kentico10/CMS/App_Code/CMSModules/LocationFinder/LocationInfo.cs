using System;
using System.Data;
using System.Runtime.Serialization;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using LocationFinder;

[assembly: RegisterObjectType(typeof(LocationInfo), LocationInfo.OBJECT_TYPE)]
    
namespace LocationFinder
{
    /// <summary>
    /// LocationInfo data container class.
    /// </summary>
	[Serializable]
    public partial class LocationInfo : AbstractInfo<LocationInfo>
    {
        #region "Type information"

        /// <summary>
        /// Object type
        /// </summary>
        public const string OBJECT_TYPE = "locationfinder.location";


        /// <summary>
        /// Type information.
        /// </summary>
        public static ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(LocationInfoProvider), OBJECT_TYPE, "LocationFinder.Location", "LocationID", "LocationLastModified", "LocationGuid", null, "LocationName", null, null, null, null)
        {

            ModuleName = "LocationFinder",
			TouchCacheDependencies = true,
            OrderColumn = "LocationOrder",
        };

        #endregion


        #region "Properties"

        /// <summary>
        /// Location ID
        /// </summary>
        [DatabaseField]
        public virtual int LocationID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("LocationID"), 0);
            }
            set
            {
                SetValue("LocationID", value);
            }
        }


        /// <summary>
        /// Location name
        /// </summary>
        [DatabaseField]
        public virtual string LocationName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationName"), String.Empty);
            }
            set
            {
                SetValue("LocationName", value);
            }
        }


        /// <summary>
        /// Location active
        /// </summary>
        [DatabaseField]
        public virtual bool LocationActive
        {
            get
            {
                return ValidationHelper.GetBoolean(GetValue("LocationActive"), true);
            }
            set
            {
                SetValue("LocationActive", value);
            }
        }


        /// <summary>
        /// Location street address
        /// </summary>
        [DatabaseField]
        public virtual string LocationStreetAddress
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationStreetAddress"), String.Empty);
            }
            set
            {
                SetValue("LocationStreetAddress", value);
            }
        }


        /// <summary>
        /// Location town
        /// </summary>
        [DatabaseField]
        public virtual string LocationTown
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationTown"), String.Empty);
            }
            set
            {
                SetValue("LocationTown", value);
            }
        }


        /// <summary>
        /// Location state
        /// </summary>
        [DatabaseField]
        public virtual string LocationState
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationState"), String.Empty);
            }
            set
            {
                SetValue("LocationState", value);
            }
        }


        /// <summary>
        /// Location zip
        /// </summary>
        [DatabaseField]
        public virtual string LocationZip
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationZip"), String.Empty);
            }
            set
            {
                SetValue("LocationZip", value);
            }
        }


        /// <summary>
        /// Location directions url
        /// </summary>
        [DatabaseField]
        public virtual string LocationDirectionsUrl
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationDirectionsUrl"), String.Empty);
            }
            set
            {
                SetValue("LocationDirectionsUrl", value, String.Empty);
            }
        }


        /// <summary>
        /// Location latitude
        /// </summary>
        [DatabaseField]
        public virtual string LocationLatitude
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationLatitude"), String.Empty);
            }
            set
            {
                SetValue("LocationLatitude", value, String.Empty);
            }
        }


        /// <summary>
        /// Location longitude
        /// </summary>
        [DatabaseField]
        public virtual string LocationLongitude
        {
            get
            {
                return ValidationHelper.GetString(GetValue("LocationLongitude"), String.Empty);
            }
            set
            {
                SetValue("LocationLongitude", value, String.Empty);
            }
        }


        /// <summary>
        /// Location order
        /// </summary>
        [DatabaseField]
        public virtual int LocationOrder
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("LocationOrder"), 0);
            }
            set
            {
                SetValue("LocationOrder", value, 0);
            }
        }


        /// <summary>
        /// Location guid
        /// </summary>
        [DatabaseField]
        public virtual Guid LocationGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("LocationGuid"), Guid.Empty);
            }
            set
            {
                SetValue("LocationGuid", value);
            }
        }


        /// <summary>
        /// Location last modified
        /// </summary>
        [DatabaseField]
        public virtual DateTime LocationLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("LocationLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("LocationLastModified", value);
            }
        }

        #endregion


        #region "Type based properties and methods"

        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            LocationInfoProvider.DeleteLocationInfo(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            LocationInfoProvider.SetLocationInfo(this);
        }

        #endregion


        #region "Constructors"

		/// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected LocationInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates an empty LocationInfo object.
        /// </summary>
        public LocationInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Constructor - Creates a new LocationInfo object from the given DataRow.
        /// </summary>
        /// <param name="dr">DataRow with the object data</param>
        public LocationInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }

        #endregion
    }
}