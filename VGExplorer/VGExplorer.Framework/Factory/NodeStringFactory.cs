using System.Collections.Generic;
using System.Linq;
using System.IO;
using VGExplorer.Framework.Entities;
using VGExplorer.Framework.Helpers;

namespace VGExplorer.Framework.Factory
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
                Type = NodeStringType.Folder,
                Date = folder.LastWriteTime
            };

            foreach (var itemNode in folder.GetDirectories())
            {
                var childs = new NodeString
                {
                    Name = itemNode.Name,
                    Type = NodeStringType.Folder,
                    Date = folder.LastWriteTime
                };

                try
                {
                    if (itemNode.GetFiles().Any() || itemNode.GetDirectories().Any())
                        childs = CreateNodeString(itemNode.FullName);
                }
                catch
                {
                    // TODO: Must handle or do something with exceptions...
                    //continue;
                }
                result.Childs.Add(childs);
            }

            foreach (var itemNode in folder.GetFiles())
            {
                var childs = new NodeString
                {
                    Name = itemNode.Name,
                    Type = NodeStringType.File,
                    Date = folder.LastWriteTime,
                    Size = FileHelper.GetFileSizeString(itemNode.Length)
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
                    Directory.CreateDirectory(Path.Combine(rootFolder + nodeString.Name, itemNode.Name));
                    if (itemNode.Childs.Any(p => p.Type == NodeStringType.Folder))
                    {
                        CreateFolderSchema(Path.Combine(rootFolder + nodeString.Name, itemNode.Name), itemNode.Childs);
                    }

                }// end-inner foreach
            }// end-outer foreach

        }

        /// <summary>
        /// Deletes an object from the passed source.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delObject">the NodeString to delete</param>
        /// <returns>a NodeString List without the indicated objects if found, otherwise, same passed list.</returns>
        public static IEnumerable<NodeString> Delete(IEnumerable<NodeString> source, NodeString delObject)
        {
            var nodeStrings = source as NodeString[] ?? source.ToArray();
            if (nodeStrings.Contains(delObject))
            {
                var tempList = nodeStrings.ToList();
                tempList.Remove(delObject);
                return tempList;
            }

            var itemsWithChilds = nodeStrings.Where(p => p.Childs.Any());
            foreach (var result in itemsWithChilds
                .Select(item => Delete(item.Childs, delObject)).Where(result => result != null))
            {
                return result;
            }
            return nodeStrings;
        }

    }

}
