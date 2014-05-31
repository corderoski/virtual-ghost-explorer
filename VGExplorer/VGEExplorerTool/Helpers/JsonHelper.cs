using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VGExplorerTool.Entities;

namespace VGExplorerTool.Helpers
{
    public class JsonHelper
    {

        public static String Serialize(NodeString nodeString)
        {
            return JsonConvert.SerializeObject(nodeString, Formatting.Indented);
        }

        public static String Serialize(IEnumerable<NodeString> nodeString)
        {
            return JsonConvert.SerializeObject(nodeString, Formatting.Indented);
        }

        public static NodeString Deserialize(String nodeStringContent)
        {
            return JsonConvert.DeserializeObject<NodeString>(nodeStringContent);
        }

        public static IEnumerable<NodeString> DeserializeArray(String nodeStringContent)
        {
            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<NodeString>>(nodeStringContent);
            }
            catch (JsonSerializationException)
            {
                //TODO: Handle this Exception
                return null;
            }
        }

    }

}
