using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VGExplorer.Framework.Entities;
using VGExplorer.Framework.Helpers;

namespace VGExplorerTool.Helpers
{
    internal class TreeNodesHelper
    {

        public static void DeleteObject(TreeView treeView, IEnumerable<NodeString> nodeString, TreeNode node)
        {
            var result = VGExplorer.Framework.Factory.NodeStringFactory.Delete(nodeString, node.Tag as NodeString);
            nodeString = result as IList<NodeString>;

            treeView.Nodes.Remove(node);
        }

        [Obsolete]
        public static NodeString GetNodeString(TreeNode node)
        {
            var result = new NodeString
            {
                Name = node.Name,
                Type = node.GetNodeCount(true) > 0 ? NodeStringType.Folder : NodeStringType.File
            };

            foreach (var treeNode in node.Nodes.OfType<TreeNode>())
            {
                var childs = new NodeString
                {
                    Name = treeNode.Name,
                    Type = node.GetNodeCount(true) > 0 ? NodeStringType.Folder : NodeStringType.File
                };

                if (treeNode.GetNodeCount(true) > 0)
                {
                    childs = GetNodeString(treeNode);
                }
                result.Childs.Add(childs);
            }
            return result;
        }

        public static TreeNode PaintNodes(NodeString node, bool showNodeInfo = false)
        {
            var parent = new TreeNode
            {
                Name = node.Name,
                Text = showNodeInfo && node.Type == NodeStringType.File && !String.IsNullOrEmpty(node.Size)
                ? node.ToLongString() : node.Name,
                Tag = node,
                ImageIndex = (int)node.Type,
                SelectedImageIndex = (int)node.Type,
            };

            foreach (var child in node.Childs)
            {
                var innerChild = new TreeNode
                {
                    Name = child.Name,
                    Tag = child,
                    ImageIndex = (int)child.Type,
                    SelectedImageIndex = (int)child.Type,
                    Text = showNodeInfo && child.Type == NodeStringType.File
                           && !String.IsNullOrEmpty(child.Size) ? child.ToLongString() : child.Name,
                };

                if (child.Childs.Count > 0)
                {
                    innerChild = PaintNodes(child);
                }

                parent.Nodes.Add(innerChild);
            }
            return parent;
        }

    }
}
