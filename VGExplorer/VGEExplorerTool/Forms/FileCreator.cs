using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VGExplorerTool.Entities;
using VGExplorerTool.Helpers;

namespace VGExplorerTool.Forms
{
    public partial class FileCreator : Form
    {

        ICollection<NodeString> _nodeString;

        ImageList _imageList;

        public FileCreator()
        {
            InitializeComponent();
            _imageList = new ImageList();
            _imageList.Images.Add(Properties.Resources.folder, Color.Transparent);
            _imageList.Images.Add(Properties.Resources.text, Color.Transparent);

            _nodeString = new Collection<NodeString>();
        }

        #region Functions

        private void CleanObjects()
        {
            itemTreeView.Nodes.Clear();
            _nodeString.Clear();
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

        private TreeNode PaintNodes(NodeString node)
        {
            var parent = new TreeNode
            {
                Name = node.Name,
                Text = node.Name,
                Tag = node.Type,
                ImageIndex = (int)node.Type,
                SelectedImageIndex = (int)node.Type,
            };

            foreach (var child in node.Childs)
            {
                var innerChild = new TreeNode
                {
                    Name = child.Name,
                    Text = child.Name,
                    Tag = child.Type,
                    ImageIndex = (int)child.Type,
                    SelectedImageIndex = (int)child.Type,
                };

                if (child.Childs.Count > 0)
                {
                    innerChild = PaintNodes(child);
                    
                }

                parent.Nodes.Add(innerChild);
            }
            return parent;
        }

        private void ShowOperationCompletedMessage()
        {
            MessageBox.Show(this, "The has operation been completed.", Program.AppName,
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion

        #region Components Events

        private void FileCreator_Load(object sender, EventArgs e)
        {
            this.Text = Program.AppName;
            //
            itemTreeView.ImageList = _imageList;
            itemTreeView.Font = new Font("Tahoma", 8, FontStyle.Regular);
            this.itemTreeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        private void itemTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            Font font = itemTreeView.Font;

            e.DrawDefault = true;

           
        }

        #region Menu Items

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
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
                var result = JsonHelper.DeserializeArray(content);

                if (result == null)
                {
                    MessageBox.Show(this, "The indicated file is empty. Virtual Exploring can't be loaded.",
                        Program.AppName + " - Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CleanObjects();
                foreach (var resultItem in result)
                {
                    itemTreeView.Nodes.Add(PaintNodes(resultItem));
                    _nodeString.Add(resultItem);
                }
                //
                this.ShowOperationCompletedMessage();
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

                var result = Factory.NodeStringFactory.CreateNodeString(folderDialog.SelectedPath);

                CleanObjects();
                itemTreeView.Nodes.Add(PaintNodes(result));
                _nodeString.Add(result);
                //
                this.ShowOperationCompletedMessage();
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.ShowNewFolderButton = true;
                var dResult = folderDialog.ShowDialog(this);

                if (dResult != System.Windows.Forms.DialogResult.OK)
                    return;

                var result = Factory.NodeStringFactory.CreateNodeString(folderDialog.SelectedPath);

                itemTreeView.Nodes.Add(PaintNodes(result));
                _nodeString.Add(result);
                //
                this.ShowOperationCompletedMessage();
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
                this.ShowOperationCompletedMessage();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CleanObjects();
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
