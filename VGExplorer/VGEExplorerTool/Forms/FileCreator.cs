using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VGExplorer.Framework.Entities;
using VGExplorer.Framework.Helpers;
using VGExplorerTool.Helpers;

namespace VGExplorerTool.Forms
{
    public partial class FileCreator : Form
    {

        readonly ICollection<NodeString> _nodeString;

        readonly ImageList _imageList;

        bool _showNodeInfo;

        public FileCreator()
        {
            InitializeComponent();
            _imageList = new ImageList();
            _imageList.Images.Add(Properties.Resources.folder, Color.Transparent);
            _imageList.Images.Add(Properties.Resources.text, Color.Transparent);

            _nodeString = new Collection<NodeString>();
            //-Entities.Configurations _appConfiguration = FileHelper.GetAppConfiguration();
        }

        #region Components Events

        private void FileCreator_Load(object sender, EventArgs e)
        {
            optionsToolStripMenuItem.Visible = false;  //  for next deploy
            //
            Text = Program.APP_NAME;
            Icon = ResourcesHelper.GetAppIcon();
            //
            itemTreeView.ImageList = _imageList;
            itemTreeView.Font = new Font("Tahoma", 8, FontStyle.Regular);
            itemTreeView.DrawMode = TreeViewDrawMode.OwnerDrawAll;
        }

        private void itemTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var font = itemTreeView.Font;

            e.DrawDefault = true;
        }
        
        #endregion

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
                openFileDialog.Filter = FileHelper.VGE_FILE_FILTER;
                //  Operation
                var dResult = openFileDialog.ShowDialog(this);
                if (dResult != System.Windows.Forms.DialogResult.OK)
                    return;

                var content = FileHelper.OpenStream(openFileDialog.FileName);
                var result = JsonHelper.DeserializeNodeStringArray(content);

                if (result == null)
                {
                    MessageBox.Show(this, "The indicated file is empty. Virtual Exploring can't be loaded.",
                        Program.APP_NAME + " - Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                CleanObjects();
                foreach (var resultItem in result)
                {
                    itemTreeView.Nodes.Add(TreeNodesHelper.PaintNodes(resultItem, _showNodeInfo));
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

                if (dResult != DialogResult.OK)
                    return;

                CleanObjects();

                var result = VGExplorer.Framework.Factory.NodeStringFactory.CreateNodeString(folderDialog.SelectedPath);

                itemTreeView.Nodes.Add(TreeNodesHelper.PaintNodes(result, _showNodeInfo));
                _nodeString.Add(result);
                //
                ShowOperationCompletedMessage();
            }
        }

        private void addFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.ShowNewFolderButton = true;
                var dResult = folderDialog.ShowDialog(this);

                if (dResult != DialogResult.OK)
                    return;

                var result = VGExplorer.Framework.Factory.NodeStringFactory.CreateNodeString(folderDialog.SelectedPath);

                itemTreeView.Nodes.Add(TreeNodesHelper.PaintNodes(result, _showNodeInfo));
                _nodeString.Add(result);
                //
                ShowOperationCompletedMessage();
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
                saveFolderDialog.Filter = FileHelper.VGE_FILE_FILTER;

                var dResult = saveFolderDialog.ShowDialog(this);

                if (dResult != DialogResult.OK)
                    return;

                var content = JsonHelper.Serialize(_nodeString);
                FileHelper.SaveStream(saveFolderDialog.FileName, content);
                //
                ShowOperationCompletedMessage();
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

                if (dResult != DialogResult.OK)
                    return;

                VGExplorer.Framework.Factory.NodeStringFactory.CreateFolderSchema(folderDialog.SelectedPath, _nodeString);
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
            var selection = itemTreeView.SelectedNode;
            if (selection == null)
                CleanObjects();
            else
                TreeNodesHelper.DeleteObject(itemTreeView, _nodeString, selection);
            itemTreeView.SelectedNode = null;
        }

        private void showInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showNodeInfo = !_showNodeInfo;
            itemTreeView.Nodes.Clear();
            foreach (var item in _nodeString)
                itemTreeView.Nodes.Add(TreeNodesHelper.PaintNodes(item, _showNodeInfo));
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            /*
             * No need for using configurations yet.
            var frm = new Configurations();
            var dResult = frm.ShowDialog(this);
            if (dResult == System.Windows.Forms.DialogResult.OK)
            {
                _appConfiguration = FileHelper.GetAppConfiguration();
            }
             * */
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://corderoski.com/");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var aBox = new AboutBox())
            {
                aBox.ShowDialog(this);
            }
        }

        #endregion

        #region Functions

        private void CleanObjects()
        {
            itemTreeView.Nodes.Clear();
            _nodeString.Clear();
        }

        private void ShowOperationCompletedMessage()
        {
            MessageBox.Show(this, "The has operation been completed.", Program.APP_NAME,
                MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        #endregion

    }
}
