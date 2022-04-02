using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace DataProviderApi.Tools
{
    public static class JsonTools
    {
        public static List<DynamicObject> Convert(JsonObject instanceOfATypes)
        {
            var result = new List<DynamicObject>();
            try
            {
                foreach (var dynamicObject in instanceOfATypes)
                {
                    var className = dynamicObject.Key;
                    var listsClass = dynamicObject.Value?.AsArray();

                    foreach (var property in listsClass)
                    {
                        var a = property?.AsObject();
                        var dic = new Tuple<string, int>("", 1);
                        foreach (var (propertyName, jsonNode) in a)
                        {
                            if (propertyName.ToUpper().Equals("ID"))
                            {
                                 dic = new Tuple<string,int>(className,int.Parse(jsonNode?.ToString() ?? string.Empty));
                            }
                            var value = jsonNode?.ToString();
                            result.Add(new DynamicObject(dic, propertyName, value, "نامشخص"));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //log
            }
            return result;
        }
    }
    public class DynamicObject
    {
        public DynamicObject(Tuple<string, int> classNameAndId, string propertyName, string propertyValue, string propertyType)
        {
            ClassNameAndId = classNameAndId;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            PropertyType = propertyType;
        }

        public Tuple<string,int> ClassNameAndId { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public string PropertyType { get; set; }
    }
}

//public static Dictionary<int, Tuple<string?, string?>> GetDicFromJsonObject(object jsonObj)
//{
//    var dic = new Dictionary<int, Tuple<string?, string?>>();
//    var command = JsonConvert.DeserializeObject(jsonObj.ToString() ?? string.Empty);
//    var i = 0;
//    var myType = command?.GetType();
//    IList<PropertyInfo> props = new List<PropertyInfo>(myType?.GetProperties() ?? Array.Empty<PropertyInfo>());
//    foreach (var item in props)
//    {
//        var propertyInfo = item.GetType().GetProperty(item.Name)?.ToString();
//        var value = item.GetValue(command, null)?.ToString();
//        dic.Add(i, new Tuple<string?, string?>(propertyInfo, value));
//        i++;
//    }
//    return dic;
//}

//public static object GetDicFromJsonObjects(List<object> jsonObj)
//{
//    foreach (var VARIABLE in jsonObj)
//    {
//        var oMycustomclassname = JsonConvert.DeserializeObject<dynamic>(VARIABLE.ToString());
//    }

//    return "dd";
//}

//public static Dictionary<string, PropertyInfo> DictionaryOfPropertiesFromInstance(List<dynamic> InstanceOfATypes)
//{
//    var result = new Dictionary<string, PropertyInfo>();

//    foreach (var d in InstanceOfATypes)
//    {
//        dynamic dd = Newtonsoft.Json.Linq.JObject.Parse("{number:1000, str:'string', array: [1,2,3,4,5,6]}");
//        var o = d;
//        var propertyNames = o.GetType().GetProperties().ToArray();
//        foreach (var prop in propertyNames)
//        {
//            var propValue = o.GetType().GetProperty(prop.Name)?.GetValue(o, null);
//            var z = prop.Attributes;
//            var a = prop.CustomAttributes;
//            var s = prop.GetMethod;
//            var x = prop.IsSpecialName;
//            var t = prop.PropertyType;
//        }
//    }

//    return result;
//}