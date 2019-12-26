
namespace DTO
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    public class DTO_DNP3_MAPPING
    {
        // Transform a IEnumerable list of DNP3 point objects for
        // binding in a DataGridView
        public DataTable ToDatatable(IEnumerable<DTO_DNP3_POINT_OBJECT> mapping)
        {
            string[] fields = {
                "IdMap", "_IdDevice", "Object", "Variation",
                "Point", "Tagname", "DNP3_type",
                "Scaling",  "To_save", "Saved_in"};
            List<Type> types = new List<Type>() {
                typeof(int), typeof(int), typeof(int), typeof(int),
                typeof(int), typeof(string), typeof(string),
                typeof(float), typeof(bool), typeof(string)
            };
            DataTable dt = new DataTable();
            int i = 0;
            foreach (string f in fields) {
                dt.Columns.Add(f, types[i++]);
            }
            try
            {
                foreach (DTO_DNP3_POINT_OBJECT map in mapping)
                {
                    DataRow row = dt.NewRow();
                    foreach (string f in fields)
                    {
                        row[f] = map[f];
                    }
                    dt.Rows.Add(row);
                }
                return dt;
            }
            catch {

                return dt;
            }
        }

        // For saving a DataTable in DataBase. 
        // _IdDevice is used to indicate to which device this map belongs to
        public IEnumerable<DTO_DNP3_POINT_OBJECT> ToIEnurablePoints(DataTable mapping, int IdDevice) {
            List<DTO_DNP3_POINT_OBJECT> rlist = new List<DTO_DNP3_POINT_OBJECT>();
            foreach (DataRow row in mapping.Rows)
            {
                try
                {
                    DTO_DNP3_POINT_OBJECT obj = new DTO_DNP3_POINT_OBJECT
                    {
                        IdDevice = IdDevice,
                        Object = Convert.ToInt16(row["Object"]),
                        Variation = Convert.ToInt16(row["Variation"]),
                        Point = Convert.ToInt16(row["Point"]),
                        DNP3_type = row["DNP3_type"].ToString(),
                        Scaling = (float)Convert.ToDouble(row["Scaling"]),
                        Saved_in = row["Saved_in"].ToString(),
                        Tagname = row["Tagname"].ToString(),
                        To_save = Convert.ToBoolean(row["To_save"])
                    };
                    // For a existing point (update purpose)
                    if ((row["IdMap"] + "").Length != 0)
                    {
                        obj.IdMap = (int)row["IdMap"];
                    }
                    rlist.Add(obj);
                }
                catch {
                    continue;
                }
            }
            return rlist;
        }
    }
}
