using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VGEExplorerTool.Entities;
using VGEExplorerTool.Helpers;

namespace VGEExplorerTool.Forms
{
    public partial class FileCreator : Form
    {

        NodeString _nodeString;

        public FileCreator()
        {
            InitializeComponent();
        }

        #region Functions

        private TreeNode PaintNodes(NodeString node)
        {
            var parent = new TreeNode
                {
                    Name = node.Name,
                    Text = node.Name,
                    Tag = node.Type
                };

            foreach (var child in node.Childs)
            {
                var innerChild = new TreeNode
                    {
                        Name = child.Name,
                        Text = child.Name,
                        Tag = child.Type
                    };

                if (child.Childs.Count > 0)
                {
                    innerChild = PaintNodes(child);
                }

                parent.Nodes.Add(innerChild);
            }
            return parent;
        }
        
        [Obsolete]
        private NodeString GetNodeString(TreeNode node)
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

        // Returns the bounds of the specified node, including the region  
        // occupied by the node label and any node tag displayed. 
        private Rectangle NodeBounds(TreeNode node)
        {
            Font tagFont = itemTreeView.Font;
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null)
            {
                // Retrieve a Graphics object from the TreeView handle 
                // and use it to calculate the display width of the tag.
                Graphics g = itemTreeView.CreateGraphics();
                int tagWidth = (int)g.MeasureString
                    (node.Tag.ToString(), tagFont).Width + 6;

                // Adjust the node bounds using the calculated value.
                bounds.Offset(tagWidth / 2, 0);
                bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
                g.Dispose();
            }

            return bounds;

        }

        #endregion


        #region Components Events

        private void FileCreator_Load(object sender, EventArgs e)
        {
            this.Text = Program.AppName;
            //
            //this.itemTreeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        private void itemTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            Font tagFont = itemTreeView.Font;

            // Draw the background and node text for a selected node. 
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                e.DrawDefault = true;

                //// Draw the background of the selected node. The NodeBounds 
                //// method makes the highlight rectangle large enough to 
                //// include the text of a node tag, if one is present.
                //e.Graphics.FillRectangle(Brushes.Transparent, NodeBounds(e.Node));

                ////// Retrieve the node font. If the node font has not been set, 
                ////// use the TreeView font.
                //Font nodeFont = e.Node.NodeFont;
                //if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                // Draw the node text.
                e.Graphics.DrawString(e.Node.Text, tagFont, Brushes.White,
                    Rectangle.Inflate(e.Bounds, 2, 0));
            }
            // Use the default background and node text. 
            else
            {
                e.DrawDefault = true;
            }

            // If a node tag is present, draw its string representation  
            // to the right of the label text. 
            if (e.Node.Tag != null)
            {
                e.Graphics.DrawString(e.Node.Tag.ToString(), tagFont,
                    Brushes.Yellow, e.Bounds.Right + 2, e.Bounds.Top);
            }

            // If the node has focus, draw the focus rectangle large, making 
            // it large enough to include the text of the node tag, if present. 
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = NodeBounds(e.Node);
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }
        }

        #region Menu Items

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                //  Sets
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                openFileDialog.Multiselect = false;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ValidateNames = true;
                openFileDialog.Filter = FileHelper.VGEFileFilter;
                //  Operation
                var dResult = openFileDialog.ShowDialog(this);

                if (dResult != System.Windows.Forms.DialogResult.OK)
                    return;

                var content = FileHelper.OpenStream(openFileDialog.FileName);

                _nodeString = JsonHelper.Deserialize(content);

                if (_nodeString == null)
                {
                    MessageBox.Show(this, "The indicated file is empty. Virtual Exploring can't be loaded.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                itemTreeView.Nodes.Clear();
                itemTreeView.Nodes.Add(PaintNodes(_nodeString));
                //
                MessageBox.Show(this, "Operation Completed!");
            }
        }
        
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.ShowNewFolderButton = true;
                var dResult = folderDialog.ShowDialog(this);

                if (dResult != System.Windows.Forms.DialogResult.OK)
                    return;

                _nodeString = Factory.NodeStringFactory.CreateNodeString(folderDialog.SelectedPath);

                itemTreeView.Nodes.Clear();
                itemTreeView.Nodes.Add(PaintNodes(_nodeString));
                //
                MessageBox.Show(this, "Operation Completed!");
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var saveFolderDialog = new SaveFileDialog())
            {
                //  Sets
                saveFolderDialog.RestoreDirectory = true;
                saveFolderDialog.ValidateNames = true;
                saveFolderDialog.AddExtension = true;
                saveFolderDialog.OverwritePrompt = true;
                saveFolderDialog.Filter = FileHelper.VGEFileFilter;

                var dResult = saveFolderDialog.ShowDialog(this);

                if (dResult != System.Windows.Forms.DialogResult.OK)
                    return;
                
                var content = JsonHelper.Serialize(_nodeString);
                FileHelper.SaveStream(saveFolderDialog.FileName, content);
                //
                MessageBox.Show(this, "Operation Completed!");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _nodeString = null;
            itemTreeView.Nodes.Clear();
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://bitbucket.org/jcordero/virtual-ghost-explorer");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var aBox = new Forms.AboutBox())
            {
                aBox.ShowDialog(this);
            }
        }

        #endregion

        #endregion

    }
}
