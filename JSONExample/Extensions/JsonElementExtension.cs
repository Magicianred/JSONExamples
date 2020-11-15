using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace JSONExample.Extensions
{
    public static class JsonElementExtension
    {
        public static List<JsonElement> FindTokens(this JsonElement containerToken, string tokenName, bool onlyValue = true)
        {
            List<JsonElement> matches = new List<JsonElement>();
            FindTokens(containerToken, tokenName, matches, onlyValue);
            return matches;
        }

        private static void FindTokens(JsonElement containerToken, string name, List<JsonElement> matches, bool onlyValue = true)
        {
            if (containerToken.ValueKind == JsonValueKind.Object)
            {
                var item = containerToken;
                var itemProps = item.EnumerateObject();

                // read property
                while (itemProps.MoveNext())
                {
                    var prop = itemProps.Current;
                    var propName = prop.Name;
                    var propValue = prop.Value;

                    if (propName == name)
                    {
                        if (onlyValue)
                        {
                            matches.Add(propValue);
                        }
                        else
                        {
                            // TO FIX: find how retrieve the right node without rewrite it
                            var element = new StringBuilder();
                            element.Append("{ \"");
                            element.Append(propName);
                            element.Append("\": \"");
                            element.Append(propValue);
                            element.Append("\" }");
                            var jsonElement = JsonDocument.Parse(element.ToString()).RootElement;
                            matches.Add(jsonElement);
                        }
                    }
                    FindTokens(propValue, name, matches, onlyValue);
                }
            }
            else if (containerToken.ValueKind == JsonValueKind.Array)
            {
                var items = containerToken.EnumerateArray();
                while (items.MoveNext())
                {
                    var item = items.Current;
                    FindTokens(item, name, matches, onlyValue);
                }
            }
        }
    }
}
