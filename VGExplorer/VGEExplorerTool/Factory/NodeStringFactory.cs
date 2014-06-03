using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VGExplorerTool.Entities;
using System.IO;

namespace VGExplorerTool.Factory
{

    public class NodeStringFactory
    {

        /// <summary>
        /// Rips a folder and her inner items creating a NodeString with the structure.
        /// </summary>
        /// <param name="folderPath">The folder's path for ripping.</param>
        /// <returns>a NodeString representing the the folder's virtual structure</returns>
        public static NodeString CreateNodeString(string folderPath)
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

        /// <summary>
        /// Creates or replicates a folder's schema given a NodeString
        /// </summary>
        /// <param name="rootFolder">a root folder</param>
        /// <param name="nodes">NodeStrins containing the schema</param>
        public static void CreateFolderSchema(string rootFolder, IEnumerable<NodeString> nodes)
        {
            rootFolder += Path.DirectorySeparatorChar;

            foreach (var nodeString in nodes.Where(p => p.Type == NodeStringType.Folder))
            {
                //  Principal Folder
                Directory.CreateDirectory(rootFolder + nodeString.Name);
                //  Filter, there could be files...
                foreach (var itemNode in nodeString.Childs.Where(p => p.Type == NodeStringType.Folder))
                {
                    Directory.CreateDirectory(rootFolder + nodeString.Name + Path.DirectorySeparatorChar + itemNode.Name);

                    if (itemNode.Childs.Any(p => p.Type == NodeStringType.Folder))
                    {
                        CreateFolderSchema(rootFolder + nodeString.Name + Path.DirectorySeparatorChar + itemNode.Name, itemNode.Childs);
                    }

                }// end-inner foreach
            }// end-outer foreach

        }


    }
}
