using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VGEExplorerTool.Entities;
using System.IO;

namespace VGEExplorerTool.Factory
{

    public class NodeStringFactory
    {

        /// <summary>
        /// Rips a folder and her inner items creating a NodeString with the structure.
        /// </summary>
        /// <param name="folderPath">The folder's path for ripping.</param>
        /// <returns>a NodeString representing the the folder's virtual structure</returns>
        internal static NodeString CreateNodeString(string folderPath)
        {
            var folder = new DirectoryInfo(folderPath);

            var result = new NodeString
            {
                Name = folder.Name,
                Type = NodeStringType.Folder
            };

            foreach (var itemNode in folder.GetDirectories())
            {
                var childs = new NodeString
                {
                    Name = itemNode.Name,
                    Type = NodeStringType.Folder
                };
                try
                {
                    if (itemNode.GetFiles().Count() > 0 || itemNode.GetDirectories().Count() > 0)
                    {
                        childs = CreateNodeString(itemNode.FullName);
                    }
                }
                catch
                {
                    // TODO: Must handle or do something with exceptions...
                    continue;
                }
                result.Childs.Add(childs);
            }

            foreach (var itemNode in folder.GetFiles())
            {
                var childs = new NodeString
                {
                    Name = itemNode.Name,
                    Type = NodeStringType.File
                };
                result.Childs.Add(childs);
            }
            return result;
        }

    }
}
