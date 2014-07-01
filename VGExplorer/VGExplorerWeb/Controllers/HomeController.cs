using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace VGExplorerWeb.Controllers
{
    public class HomeController : Controller
    {

        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Request != null)
            {
                var file = Request.Files["fileControl"];

                if ((file != null) && (file.ContentLength > 0) && !String.IsNullOrEmpty(file.FileName))
                {
                    var reader = new BinaryReader(file.InputStream);
                    byte[] bytes = reader.ReadBytes((int)file.InputStream.Length);
                    //  to string
                    string result = System.Text.Encoding.UTF8.GetString(bytes);
                    //  conversion + output
                    var nodeString = DeserializeNodeStringArray(result);
                    ViewBag.Data = CreateHtmlListStructure(nodeString);
                    ViewBag.IsData = true;
                }
            }
            return View();
        }

        private string CreateHtmlListStructure(IEnumerable<NodeString> nodes)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<ul>");

            foreach (var nodeString in nodes)
            {
                string li;
                if (nodeString.Childs.Any())
                {
                    li = "<li>" +
                    "<span class='folder'>" + nodeString.Name + "</span>" +
                     CreateHtmlListStructure(nodeString.Childs) + "</li>";
                }
                else
                {
                    li = "<li>" + String.Format("<span class='file'>{0}</span>", nodeString.Name) + "</li>";
                }
                sb.AppendLine(li);
            }
            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        private static IEnumerable<NodeString> DeserializeNodeStringArray(String nodeStringContent)
        {
            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<NodeString>>(nodeStringContent);
            }
            catch (Exception)
            {
                //TODO: Handle this Exception
                return null;
            }
        }

    }

    public class NodeString
    {

        public NodeString()
        {
            Childs = new Collection<NodeString>();
        }

        public String Name { get; set; }

        public NodeStringType Type { get; set; }

        public ICollection<NodeString> Childs { get; set; }

        public String Size { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Name, Type);
        }

    }

    public enum NodeStringType
    {
        Folder = 0,
        File = 1
    }

}
