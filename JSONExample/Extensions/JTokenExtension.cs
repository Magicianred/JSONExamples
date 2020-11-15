using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace JSONExample.Extensions
{
    /// <summary>
    /// https://stackoverflow.com/questions/19645501/searching-for-a-specific-jtoken-by-name-in-a-jobject-hierarchy
    /// https://code-examples.net/en/q/12bc43d
    /// </summary>
    public static class JTokenExtension
    {
        public static List<JToken> FindTokens(this JToken containerToken, string tokenName, bool onlyValue = true)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, tokenName, matches, onlyValue);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches, bool onlyValue = true)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        if (onlyValue)
                        {
                            matches.Add(child.Value);
                        }
                        else { 
                            matches.Add(child); 
                        }
                    }
                    FindTokens(child.Value, name, matches, onlyValue);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches, onlyValue);
                }
            }
        }
    }
}
