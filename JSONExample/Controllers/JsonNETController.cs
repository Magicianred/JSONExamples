using JSONExample.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONExample.Controllers
{
    public class JsonNETController : Controller
    {
        public string Parse()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JObject.Parse(fileContent);
            return jGlossary.ToString();
        }
        public string ReadSingleRootElement()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jRoot = JToken.Parse(fileContent);

            if (jRoot.Type != JTokenType.Object)
            {
                throw new ArgumentException("It's not a single element");
            }

            var jGlossary = jRoot.ToObject<JObject>();

            var itemProps = jGlossary.Properties();
            foreach (var prop in itemProps)
            {
                var propName = prop.Name;
                var propValue = prop.Value<JToken>();
            }

            return jGlossary.ToString();
        }
        public string ReadArrayRootElement()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/menuItems.json");
            var root = JToken.Parse(fileContent);

            if (root.Type != JTokenType.Array)
            {
                throw new ArgumentException("It's not an Array");
            }

            var jItems = root.ToObject<JArray>();

            // read first element
            var item1 = jItems[0].Value<JObject>();
            var itemValue = item1.Value<string>("value");
            var itemOnclick = item1.Value<string>("onclick");

            // loop througth array
            foreach (var item in jItems)
            {
                var jItem = item.ToObject<JObject>();
                var itemProps = jItem.Properties();
                foreach(var prop in itemProps)
                {
                    var propName = prop.Name;
                    var propValue = prop.Value<JToken>();
                }
            }
            return jItems.ToString();
        }
        public string ReadRootChildren()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);
            var jChildren = jGlossary.First.Children();

            foreach (var child in jChildren)
            {
                var jObj = child as JObject;
                var itemProps = jObj.Properties();
                foreach (var prop in itemProps)
                {
                    var propName = prop.Name;
                    var propValue = prop.Value<JToken>();
                }
            }

            return jGlossary.ToString();
        }
        public string SelectSpecificNodeByName()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            foreach (JToken token in jGlossary.FindTokens("Acronym", false))
            {
                var tokenPath = token.Path;
                var tokenValue = token.ToString();
            }

            return jGlossary.ToString();
        }
        public string SelectSpecificNodeByNameInArrayByValue()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/menuItems.json");
            var jGlossary = JToken.Parse(fileContent);

            var founded = false;

            foreach (JToken token in jGlossary.FindTokens("onclick"))
            {
                var tokenPath = token.Path;
                var tokenValue = token.ToString();
                if(tokenValue == "OpenDoc()")
                {
                    founded = true;
                }
            }

            return jGlossary.ToString();
        }

        public string SelectSpecificNodeByJsonPath()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            // http://jsonpath.com/
            JToken selectedNode = jGlossary.SelectToken("$.*.*.title");

            return selectedNode.ToString();
        }

        public string AddPropertyToNode()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            var author = "{ \"Author\": \"Nome Cognome\" }";
            var jAuthor = JObject.Parse(author);

            JToken selectedNode = jGlossary.SelectToken("$.glossary");
            var jGlossaryNew = selectedNode as JObject;
            jGlossaryNew.Add("Info", jAuthor);

            var newProp = new JProperty("Editor", "Company name");
            jGlossaryNew.Property("Info").AddAfterSelf(newProp);

            return jGlossary.ToString();
        }

        public string AddArrayToNode()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            var items = "{ \"Items\": [] }";
            var jItems = JObject.Parse(items);

            JToken selectedNode = jGlossary.SelectToken("$.glossary");
            var jGlossaryNew = selectedNode as JObject;
            jGlossaryNew.Add("Info", jItems);

            JToken selectedItems = jGlossary.SelectToken("$..Items");
            var sItems = selectedItems as JArray;
            sItems.Add("Item 1");
            sItems.Add("Item 2");
            sItems.Add("Item 3");

            return jGlossary.ToString();
        }
        public string ChangePropertyToNode()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            JToken selectedNode = jGlossary.SelectToken("$.glossary.GlossDiv.title");
            selectedNode.Replace("New title");

            return jGlossary.ToString();
        }

        public string ReplaceNodeValue()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            JObject jObject = new JObject
            {
                ["GlossSpan"] = "text to span"
            };

            JToken selectedNode = jGlossary.SelectToken("$.glossary.GlossDiv");
            selectedNode.Replace(jObject);

            return jGlossary.ToString();
        }

        public string ReplaceNode()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            JProperty jProp = new JProperty("attributo", "valore");

            JToken selectedNode = jGlossary.SelectToken("$.glossary.GlossDiv.GlossList.GlossEntry");
            selectedNode.Parent.Replace(jProp);

            return jGlossary.ToString();
        }

        public string RemoveNode()
        {
            string fileContent = System.IO.File.ReadAllText("Resources/glossary.json");
            var jGlossary = JToken.Parse(fileContent);

            JToken selectedNode = jGlossary.SelectToken("$.glossary.GlossDiv");
            selectedNode.Parent.Remove();

            return jGlossary.ToString();
        }

        // http://json-schema.org/
        // https://www.newtonsoft.com/jsonschema
        // AGPL 3.0	Free with limitations (1000 validations per hour)
        public string ValidateBySchema()
        {
            var jsonSchema = @" {
                    ""type"": ""object"",
                    ""properties"": {
                        ""name"": {
                            ""type"": ""string""
                        },
                        ""age"": {
                            ""type"": ""integer"",
                            ""minimum"": 0
                        },
                        ""isMarried"": {
                            ""type"": ""boolean""
                        }
                    }
                }
                ";
            JSchema schema = JSchema.Parse(jsonSchema);

            IList<string> errorMessages;

            var jsonDataValid = @"{
                ""name"": ""Jay"",
                ""age"": 20,
                ""isMarried"": true
            }
            ";
            var jExample1 = JToken.Parse(jsonDataValid);

            var jsonDataInvalid = @"{
                ""name"": ""Jay"",
                ""age"": ""20"",
                ""isMarried"": ""true""
            }
            ";
            var jExample2 = JToken.Parse(jsonDataInvalid);

            bool isValid1 = jExample1.IsValid(schema, out errorMessages);
            errorMessages.Clear();

            bool isValid2 = jExample2.IsValid(schema, out errorMessages);

            return "";
        }

    }
}
