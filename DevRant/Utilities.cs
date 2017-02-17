using Newtonsoft.Json.Linq;

namespace DevRant
{
    internal class Utilities
    {
        public static object GetValue(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Integer:
                    return token.ToObject<int>();
                case JTokenType.Boolean:
                    return token.ToObject<bool>();
                case JTokenType.Float:
                    return token.ToObject<float>();
                case JTokenType.String:
                    return token.ToObject<string>();
                default:
                    return token.ToString();
            }
        }
    }
}