
namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    public class DTO_DNP3_POINT_OBJECT
    {
        public int IdMap { get; set; }
        public int IdDevice { get; set; }
        public int Object { get; set; }
        public int Variation { get; set; }
        public int Point { get; set; }
        public string Tagname { get; set; }
        public string DNP3_type { get; set; }
        public float Scaling { get; set; }
        public bool To_save { get; set; }
        public string Saved_in { get; set; }

        public object this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(DTO_DNP3_POINT_OBJECT);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(DTO_DNP3_POINT_OBJECT);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);

            }

        }

    }


}
