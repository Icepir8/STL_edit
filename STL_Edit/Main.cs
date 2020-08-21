using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STL_Edit
{
    public partial class Main : Form
    {
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
            dlgOpen.Filter = "STL 3D Model *.stl|*.stl|All files *.*|*.*";

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                EditSTL newMDIChild = newEditSTL();
                newMDIChild.LoadSTL(dlgOpen.FileName);

                newMDIChild.RenderSTL();
            }
        }

        EditSTL newEditSTL()
        {
            bool isFirst = (ActiveMdiChild == null);
            EditSTL newMDIChild = new EditSTL();
            // Set the Parent Form of the Child window.
            newMDIChild.MdiParent = this;
            //newMDIChild.WindowState = WindowState = FormWindowState.Minimized;

            newMDIChild.Show();
            if (isFirst)
            {
                newMDIChild.WindowState = FormWindowState.Maximized;
            }
/*
            else
            {
                newMDIChild.WindowState = ActiveMdiChild.WindowState;
            }
*/

            return newMDIChild;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
