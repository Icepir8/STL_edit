using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STL_Edit
{
    public partial class Main : Form
    {
        string path = string.Empty;

        public Main()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSTL newMDIChild = newEditSTL();
            // Display the new form.
            newMDIChild.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlgOpen = new OpenFileDialog();
            dlgOpen.Filter = "STL 3D Model *.stl|*.stl|Wavefront *.obj|*.obj";

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                EditSTL newMDIChild = newEditSTL();

                int loadType = dlgOpen.FilterIndex;

                path = Path.GetDirectoryName(dlgOpen.FileName);

                newMDIChild.LoadSTL(dlgOpen.FileName,loadType);

                newMDIChild.RenderSTL();
                int partnum = 1;
                string name = newMDIChild.Model.DesignName;

                //foreach (stlObject part in newMDIChild.Model.parts)
                //{
                //    part.DesignName = $"{name}-{partnum}";
                //    EditSTL partMDIChild = newEditSTL(part);
                //    partMDIChild.RenderSTL();
                //}
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlgSave = new SaveFileDialog();
            dlgSave.Filter = "STL 3D Model (ASCII) *.stl|*.stl|STL 3D Model (Binary) *.stl|*.stl|Wavefront *.obj|*.obj";
            dlgSave.DefaultExt = "stl";
            dlgSave.AddExtension = true;
            dlgSave.FileName = ((EditSTL)(this.ActiveMdiChild)).Text.Replace(" ","-");

            path = Path.GetDirectoryName(dlgSave.FileName);

            if (dlgSave.ShowDialog() == DialogResult.OK)
            {
                EditSTL newMDIChild = (EditSTL)(this.ActiveMdiChild);
                int saveType = dlgSave.FilterIndex;

                newMDIChild.SaveSTL(dlgSave.FileName, saveType);
            }
        }

        public EditSTL newEditSTL()
        {
            bool isFirst = (ActiveMdiChild == null);
            EditSTL newMDIChild = new EditSTL();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            newMDIChild.Show();
            if (isFirst)
            {
                newMDIChild.WindowState = FormWindowState.Maximized;
            }

            return newMDIChild;
        }


        public EditSTL newEditSTL(stlObject newModel)
        {
            bool isFirst = (ActiveMdiChild == null);
            EditSTL newMDIChild = new EditSTL(newModel);
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;

            newModel.Parent = newMDIChild;

            newMDIChild.Show();
            return newMDIChild;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void explodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            stlObject[] parts = ((EditSTL)ActiveMdiChild).Explode();
            foreach(stlObject part in parts)
            {
                EditSTL newMDIChild = newEditSTL();
                // Set the Parent Form of the Child window.
                //newMDIChild.MdiParent = this;
                newMDIChild.Model = part;
                newMDIChild.RenderSTL();
                part.Parent = newMDIChild;
                newMDIChild.Text = part.DesignName;
            }

            this.Cursor = Cursors.Default;
        }

        private void hallowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((EditSTL)ActiveMdiChild).HallowOut();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderPicker dialog = new FolderPicker();

            dialog.InputPath = path;
            //dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == true)
            {
                foreach(Form client in this.MdiChildren)
                {
                    EditSTL newMDIChild = (EditSTL)(client);
                    string filename = $"{dialog.ResultPath}\\{((EditSTL)client).Model.DesignName.Replace(" ","-")}.stl";
                    int saveType = (int)SAVETYPE.STL_ASCII;

                    newMDIChild.SaveSTL(filename, saveType);
                }
            }
        }
    }
}
