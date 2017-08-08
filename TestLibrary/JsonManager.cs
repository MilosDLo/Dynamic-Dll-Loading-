using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class JsonManager
    {
        public static Dictionary<string, Object> Deserialize(string json)
        {
            return (Dictionary<string, Object>)ToObject(JToken.Parse(json));
        }

        private static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }
        public static bool CheckIfExist(string key, Dictionary<string, object> obj)
        {
            bool exist = (obj.ContainsKey(key)) ? true : false;
            return exist;
        }
    }
}
