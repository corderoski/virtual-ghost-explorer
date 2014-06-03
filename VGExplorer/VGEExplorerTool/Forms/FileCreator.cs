﻿using System;
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

        Entities.Configuration _appConfiguration;

        public FileCreator()
        {
            InitializeComponent();
            _imageList = new ImageList();
            _imageList.Images.Add(Properties.Resources.folder, Color.Transparent);
            _imageList.Images.Add(Properties.Resources.text, Color.Transparent);

            _nodeString = new Collection<NodeString>();
            _appConfiguration = FileHelper.GetAppConfiguration();
        }

        
        #region Components Events

        private void FileCreator_Load(object sender, EventArgs e)
        {
            //
            this.optionsToolStripMenuItem.Visible = false;  //  for next deploy
            //
            this.Text = Program.AppName;
            this.Icon = Helpers.ResourcesHelper.GetAppIcon();
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
                var result = JsonHelper.DeserializeNodeStringArray(content);

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

        private void createSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.ShowNewFolderButton = true;
                folderDialog.Description = "Select a root folder for creating the current schema";
                //
                var dResult = folderDialog.ShowDialog(this);

                if (dResult != System.Windows.Forms.DialogResult.OK)
                    return;

                Factory.NodeStringFactory.CreateFolderSchema(folderDialog.SelectedPath, _nodeString);
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

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Configuration();
            var dResult = frm.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                _appConfiguration = FileHelper.GetAppConfiguration();
            }
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



    }
}
