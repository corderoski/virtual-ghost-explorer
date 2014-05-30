using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VGEExplorerTool.Entities;

namespace VGEExplorerTool.Helpers
{
    public class JsonHelper
    {

        public static String Serialize(NodeString nodeString)
        {
            return JsonConvert.SerializeObject(nodeString, Formatting.Indented);
        }

        public static NodeString Deserialize(String nodeStringContent)
        {
            return JsonConvert.DeserializeObject<NodeString>(nodeStringContent);
        }

    }

}
