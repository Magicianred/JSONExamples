using JSONExample.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace JSONExample.Controllers
{
    public class TextJSONController : Controller
    {
        public string Parse()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            using JsonDocument jGlossary = JsonDocument.Parse(fileContent);
            JsonElement root = jGlossary.RootElement;
            return root.ToString();
        }
        public string ReadSingleRootElement()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            using JsonDocument jGlossary = JsonDocument.Parse(fileContent);
            JsonElement root = jGlossary.RootElement;

            if (root.ValueKind != JsonValueKind.Object)
            {
                throw new ArgumentException("It's not a single element");
            }

            var props = root.EnumerateObject();

            // read property
            while (props.MoveNext())
            {
                var prop = props.Current;
                var propName = prop.Name;
                var propValue = prop.Value;
            }

            return jGlossary.ToString();
        }
        public string ReadArrayRootElement()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/menuItems.json");
            using JsonDocument jMenuItems = JsonDocument.Parse(fileContent);
            JsonElement root = jMenuItems.RootElement;

            if (root.ValueKind != JsonValueKind.Array)
            {
                throw new ArgumentException("It's not an Array");
            }

            // read first element
            var item1 = root[0];
            var itemValue = item1.GetProperty("value");
            var itemOnclick = item1.GetProperty("onclick");

            // loop througth array
            var items = root.EnumerateArray();
            while (items.MoveNext())
            {
                var item = items.Current;
                var props = item.EnumerateObject();

                // read property
                while (props.MoveNext())
                {
                    var prop = props.Current;
                    var propName = prop.Name;
                    var propValue = prop.Value;
                }
            }

            return root.ToString();
        }


        public string ReadRootChildren()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            using JsonDocument jGlossary = JsonDocument.Parse(fileContent);
            JsonElement root = jGlossary.RootElement;

            // read first element
            var firstElement = root.EnumerateObject();
            if (firstElement.MoveNext())
            {
                // read children
                var jChildren = firstElement.Current.Value.EnumerateObject();
                while (jChildren.MoveNext())
                {
                    var child = jChildren.Current;
                    var childName = child.Name;
                    var childValue = child.Value;
                }
            }

            return jGlossary.ToString();
        }

        public string SelectSpecificNodeByName()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            using JsonDocument jGlossary = JsonDocument.Parse(fileContent);
            JsonElement root = jGlossary.RootElement;

            foreach (JsonElement token in root.FindTokens("Acronym", false))
            {
                //var tokenPath = token.Path;
                var tokenValue = token.ToString();
            }

            return jGlossary.ToString();
        }
        public string SelectSpecificNodeByNameInArrayByValue()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/menuItems.json");
            using JsonDocument jItems = JsonDocument.Parse(fileContent);
            JsonElement root = jItems.RootElement;

            var founded = false;

            foreach (JsonElement token in root.FindTokens("onclick"))
            {
                //var tokenPath = token.Path;
                var tokenValue = token.ToString();
                if (tokenValue == "OpenDoc()")
                {
                    founded = true;
                }
            }

            return jItems.ToString();
        }

        public string SelectSpecificNodeByJsonPath()
        {
            // System.Text.Json has no support for JSONPath which is often quite convenient in such applications.
            throw new ArgumentException("Use https://github.com/azambrano/JsonDocumentPath based on Newtonsoft");
        }

        public string AddPropertyToNode()
        {
            throw new ArgumentException();
        }

        public string AddArrayToNode()
        {
            throw new ArgumentException();
        }

        public string ChangePropertyToNode()
        {
            throw new ArgumentException();
        }
        public string ReplaceNodeValue()
        {
            throw new ArgumentException();
        }

        public string ReplaceNode()
        {
            throw new ArgumentException();
        }

        public string RemoveNode()
        {
            throw new ArgumentException();
        }

        public string ValidateBySchema()
        {
            throw new ArgumentException();
        }
    }
}
