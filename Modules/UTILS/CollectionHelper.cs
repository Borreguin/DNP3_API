﻿

namespace UTILS
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Reflection;

    public class CollectionHelper
    {
        private CollectionHelper()
        {
        }

        public static DataTable ConvertTo<T>(IEnumerable<T> list)
        {
            DataTable table = CreateTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static IEnumerable<T> ConvertTo<T>(IEnumerable<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static IEnumerable<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            var aux = ConvertTo<T>(rows);
            return aux;
        }

        /*
        public static JObject ToJObject(DataRow row) {
            JObject result = new JObject();
            if (row != null) {
                foreach (DataColumn column in row.Table.Columns) { 
                }
            }
            return result;
        }*/


        public static T CreateItem<T>(JObject jObject) {
            T obj = default(T);
            obj = Activator.CreateInstance<T>();
            var tipo = obj.GetType();

            if (jObject != null)
            {
                foreach (var kv in jObject)
                {
                    PropertyInfo prop = tipo.GetProperty(kv.Key);
                    //Console.WriteLine(prop.PropertyType);
                    try
                    {
                        if (prop != null)
                        {
                            if (prop.PropertyType.IsEnum)
                            {
                                prop.SetValue(obj, Transform(kv.Value, prop), null);
                            }
                            else if (prop.PropertyType == typeof(bool)) {
                                prop.SetValue(obj, kv.Value.ToObject<bool>() , null);
                            }
                            else {
                                prop.SetValue(obj, Transform(kv.Value), null);
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
            return obj;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            // prop is the object to fill up
            obj = Activator.CreateInstance<T>();
            var tipo = obj.GetType();
            if (row != null)
            {
                foreach (DataColumn column in row.Table.Columns)
                {
                    
                    PropertyInfo prop = tipo.GetProperty(column.ColumnName);
                    try
                    {
                        if (prop != null)
                        {
                            object value = row[column.ColumnName];
                            if (value.Equals(DBNull.Value)) value = null;
                            if (prop.PropertyType.Name.Equals(typeof(DateTime).Name)
                                && value.GetType().Name.Equals(typeof(string).Name))
                            {
                                DateTime timestamp = toDateTime((string)value);
                                // set value inside object:
                                prop.SetValue(obj, timestamp, null);
                            }
                            else if (prop.PropertyType.Name.Equals(typeof(JObject).Name)
                                && value.GetType().Name.Equals(typeof(string).Name))
                            {
                                if (IsValidJson((string)value))
                                {
                                    // set value inside object:
                                    prop.SetValue(obj, JObject.Parse((string)value), null);
                                }
                                else
                                {
                                    // set value inside object:
                                    prop.SetValue(obj, new JObject(), null);
                                }
                            }
                            else
                            {
                                // set value inside object:
                                prop.SetValue(obj, value, null);
                            }

                        }
                        else
                        {
                            // Este caso sirve para traer datos desde un
                            // formatos JSON de manera recursiva
                            object value = row[column.ColumnName]; // check if value is a JSON string
                            var properties = obj.GetType().GetProperties();
                            foreach (PropertyInfo p in properties)
                            {
                                object jvalue = search_in_json(value, p.Name);

                                if (jvalue != null)
                                {
                                    prop = tipo.GetProperty(p.Name);
                                    Type propertyType = prop.PropertyType;
                                    jvalue = Convert.ChangeType(jvalue, propertyType);
                                    // set value inside object:
                                    prop.SetValue(obj, jvalue, null);
                                }
                            }
                            // object jvalue = search_in_json(value, column.ColumnName);

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Collection Helper: " + e.ToString());
                        // You can log something here
                        //throw;
                    }
                }
            }

            return obj;
        }


        private static object search_in_json(object str_json, string to_search)
        {
            object result = null;
            if (str_json.GetType() == typeof(string))
            {
                // check if is a Json String
                if (IsValidJson((string)str_json))
                {
                    JObject json = JObject.Parse((string)str_json);
                    foreach (var item in json)
                    {
                        if (item.Key.Equals(to_search))
                        {
                            // se encontro el attributo en el objeto json
                            return Transform(item.Value);
                        }
                        // si fuera un Json anidado, se continua 
                        if (item.Value.GetType() == typeof(string))
                        {
                            if (IsValidJson((string)item.Value))
                            {
                                object partial = search_in_json(item, to_search);
                                if (partial != null) return partial;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static object Transform(JToken obj, PropertyInfo prop)
        {
            // var aux = Enum.ToObject(prop.PropertyType, obj.ToObject<String>());
            return Enum.Parse(prop.PropertyType, obj.ToObject<String>());
        }

        public static object Transform(JToken obj)
        {
            // TODO: make this do something
            //var val = obj.Value;
            if (obj.Type.Equals(JTokenType.Integer))
            {
                return obj.ToObject<int>();
            }
            else if (obj.Type.Equals(JTokenType.Float))
            {
                return obj.ToObject<decimal>();
            }
            else if (obj.Type.Equals(JTokenType.Bytes))
            {
                return obj.ToObject<byte>();
            }
            else if (obj.Type.Equals(JTokenType.String))
            {
                string str_value = obj.ToObject<String>();
                if (IsValidJson(str_value)) return JObject.Parse((string)str_value);
                return str_value;
            }
            else if (obj.Type.Equals(JTokenType.Boolean))
            {
                return obj.ToObject<bool>();
            }
            else if (obj.Type.Equals(JTokenType.Null))
            {
                //throw new ApplicationException("NULL Not implemented yet");
                return null; // TODO: Change this to an option
            }
            else
            {
                throw new Exception("Don't know how to transform a " + obj.GetType() + ".");
            }
        }

        public static DataTable CreateTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        public static bool IsValidJson(string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static JObject Get_json_from(object obj_src, string[] AttributeNames)
        {
            var result = new JObject();
            foreach (string att in AttributeNames)
            {
                try
                {
                    object attr_value = GetPropValue(obj_src, att);
                    switch (attr_value.GetType().ToString())
                    {
                        case "System.Int32":
                            result.Add(att, (int)attr_value);
                            break;
                        case "System.Int64":
                            result.Add(att, (long)attr_value);
                            break;
                        case "System.Single":
                            result.Add(att, (float)attr_value);
                            break;
                        case "System.String":
                            result.Add(att, (string)attr_value);
                            break;
                        case "System.Byte":
                            result.Add(att, (byte)attr_value);
                            break;
                        default:
                            result.Add(att, (string)attr_value.ToString());
                            break;
                    }
                }
                catch (NullReferenceException ex)
                {
                    result.Add(att, null);
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            return result;
        }

        public static object GetPropValue(object src, string propName)
        {
            try
            {
                return src.GetType().GetProperty(propName).GetValue(src, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static DateTime toDateTime(string str_datetime)
        {

            DateTime result = new DateTime();
            string[] format = { "yy-MM-dd HH:mm", "yyyy-MM-dd HH:mm", "yyyy-MM-dd HH:mm:ss", "yy-MM-dd HH:mm:ss", "yy-MM-dd" };
            foreach (string f in format)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                try
                {
                    result = DateTime.ParseExact(str_datetime, f, provider);
                    return result;
                }
                catch (FormatException)
                {

                }
            }
            Console.WriteLine("{0} is not in the correct format.", str_datetime);
            return result;
        }
    }
}
