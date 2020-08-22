using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STL_Edit
{
    public partial class EditSTL : Form
    {
        stlObject Model;

        public EditSTL()
        {
            InitializeComponent();
            Form1_Resize(null, null);
        }

        public void LoadSTL(string FileName)
        {
            Model = new stlObject();
            Model.Load(FileName);

            RenderSTL();
        }

        private void btnLookZ_Click(object sender, EventArgs e)
        {
            int height = panel2.Height;
            int width = panel2.Width;
            rotateView1.rotateMtx = new MyMatrix();

            pictureBox1.Height = height;
            pictureBox1.Width = width;
            pictureBox1.Top = 0;
            pictureBox1.Left = 0;

            Bitmap stlBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap2 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap3 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Model.Render(stlBitmap1, stlBitmap2, stlBitmap3, 0, Model.Facettes.Count);

            pictureBox1.Image = stlBitmap1;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;

            label1.Text = $"Design Name: {Model.DesignName}";
            Application.DoEvents();
        }

        private void btnLookY_Click(object sender, EventArgs e)
        {
            int height = panel2.Height;
            int width = panel2.Width;

            pictureBox1.Height = height;
            pictureBox1.Width = width;
            pictureBox1.Top = 0;
            pictureBox1.Left = 0;

            Bitmap stlBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap2 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap3 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Model.Render(stlBitmap1, stlBitmap2, stlBitmap3, 0, Model.Facettes.Count);

            pictureBox1.Image = stlBitmap2;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;

            label1.Text = $"Design Name: {Model.DesignName}";
            Application.DoEvents();
        }

        private void btnLookX_Click(object sender, EventArgs e)
        {
            int height = panel2.Height;
            int width = panel2.Width;

            pictureBox1.Height = height;
            pictureBox1.Width = width;
            pictureBox1.Top = 0;
            pictureBox1.Left = 0;

            Bitmap stlBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap2 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap3 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Model.Render(stlBitmap1, stlBitmap2, stlBitmap3, 0, Model.Facettes.Count);

            pictureBox1.Image = stlBitmap3;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;

            label1.Text = $"Design Name: {Model.DesignName}";
            Application.DoEvents();
        }

        private void btnLookXYZ_Click(object sender, EventArgs e)
        {
            int height = panel2.Height;
            int width = panel2.Width;

            Form1_Resize(null, null);

            Bitmap stlBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap2 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap3 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Model.Render(stlBitmap1, stlBitmap2, stlBitmap3, 0, Model.Facettes.Count);

            pictureBox1.Image = stlBitmap1;
            pictureBox2.Image = stlBitmap2;
            pictureBox3.Image = stlBitmap3;

            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            pictureBox4.Visible = true;

            label1.Text = $"Design Name: {Model.DesignName}";
            Application.DoEvents();
        }

        public void RenderSTL()
        {
            Bitmap stlBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap2 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Bitmap stlBitmap3 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            //for (int fIndx = 0; fIndx < (Model.Facettes.Count - 1); fIndx++)
            //for (int fIndx = 0; fIndx < 20500; fIndx++)
            {
                Model.Render(stlBitmap1, stlBitmap2, stlBitmap3, 0, Model.Facettes.Count);
                
                pictureBox1.Image = stlBitmap1;
                pictureBox2.Image = stlBitmap2;
                pictureBox3.Image = stlBitmap3;
                label1.Text = $"Design Name: {Model.DesignName}"; 
                Application.DoEvents();
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            int height = panel2.Height / 2;
            int width = panel2.Width / 2;

            pictureBox1.Height = height;
            pictureBox1.Width = width;
            pictureBox1.Top = 0;
            pictureBox1.Left = 0;

            pictureBox2.Height = height;
            pictureBox2.Width = width;
            pictureBox2.Top = 0;
            pictureBox2.Left = width;

            pictureBox3.Height = height;
            pictureBox3.Width = width;
            pictureBox3.Top = height;
            pictureBox3.Left = 0;

            pictureBox4.Height = height;
            pictureBox4.Width = width;
            pictureBox4.Top = height;
            pictureBox4.Left = width;

            Application.DoEvents();
        }

        private void EditSTL_SizeChanged(object sender, EventArgs e)
        {
            Form1_Resize(null, null);
        }

        private void rotateView1_ViewChanged_1(object sender, EventArgs e)
        {
            int height = panel2.Height;
            int width = panel2.Width;

            pictureBox1.Height = height;
            pictureBox1.Width = width;
            pictureBox1.Top = 0;
            pictureBox1.Left = 0;

            Bitmap stlBitmap1 = new Bitmap(pictureBox1.Width, pictureBox1.Height, PixelFormat.Format32bppArgb);
            Model.RenderRot(stlBitmap1, rotateView1.rotateMtx, 0, Model.Facettes.Count);

            pictureBox1.Image = stlBitmap1;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;

            label1.Text = $"Design Name: {Model.DesignName}";
            Application.DoEvents();
        }
    }
}
