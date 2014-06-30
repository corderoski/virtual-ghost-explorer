﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VGExplorerTool.Entities;

namespace VGExplorerTool.Helpers
{
    public class JsonHelper
    {

        public static String Serialize(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static String Serialize(IEnumerable<NodeString> nodeString)
        {
            return JsonConvert.SerializeObject(nodeString, Formatting.Indented);
        }

        public static T Deserialize<T>(String content) where T : class
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static IEnumerable<NodeString> DeserializeNodeStringArray(String nodeStringContent)
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
