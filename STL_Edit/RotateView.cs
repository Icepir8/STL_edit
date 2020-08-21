using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STL_Edit
{
    public partial class RotateView : UserControl
    {
        PointF[] circle = new PointF[33]
        {
            new PointF(1F, 0F),
            new PointF(0.9807853F, 0.1950903F),
            new PointF(0.9238795F, 0.3826834F),
            new PointF(0.8314696F, 0.5555702F),
            new PointF(0.7071068F, 0.7071068F),
            new PointF(0.5555702F, 0.8314696F),
            new PointF(0.3826834F, 0.9238795F),
            new PointF(0.1950903F, 0.9807853F),
            new PointF(0F, 1F),
            new PointF(-0.1950903F, 0.9807853F),
            new PointF(-0.3826834F, 0.9238795F),
            new PointF(-0.5555702F, 0.8314696F),
            new PointF(-0.7071068F, 0.7071068F),
            new PointF(-0.8314696F, 0.5555702F),
            new PointF(-0.9238795F, 0.3826834F),
            new PointF(-0.9807853F, 0.1950903F),
            new PointF(-1F, 0),
            new PointF(-0.9807853F, -0.1950903F),
            new PointF(-0.9238795F, -0.3826834F),
            new PointF(-0.8314696F, -0.5555702F),
            new PointF(-0.7071068F, -0.7071068F),
            new PointF(-0.5555702F, -0.8314696F),
            new PointF(-0.3826834F, -0.9238795F),
            new PointF(-0.1950903F, -0.9807853F),
            new PointF(0F, -1F),
            new PointF(0.1950903F, -0.9807853F),
            new PointF(0.3826834F, -0.9238795F),
            new PointF(0.5555702F, -0.8314696F),
            new PointF(0.7071068F, -0.7071068F),
            new PointF(0.8314696F, -0.5555702F),
            new PointF(0.9238795F, -0.3826834F),
            new PointF(0.9807853F, -0.1950903F),
            new PointF(0F, 0F),
        };

        MyMatrix rotateMtx = new MyMatrix();

        MyMatrix Xrotate = new MyMatrix(new float[3, 3] { { 1, 0, 0 }, { 0, 0.9807853F, -0.1950903F }, { 0, 0.1950903F, 0.9807853F } });
        MyMatrix Yrotate = new MyMatrix(new float[3, 3] { { 0.9807853F, 0, 0.1950903F }, { 0, 1, 0 }, { -0.1950903F, 0, 0.9807853F } });
        MyMatrix Zrotate = new MyMatrix(new float[3, 3] { { 0.9807853F, -0.1950903F, 0 }, { 0.1950903F, 0.9807853F, 0 }, { 0, 0, 1 } });

        PointF[] scaledCircle = new PointF[33];
        PointF[] XCircle = new PointF[33];
        PointF[] YCircle = new PointF[33];
        PointF[] ZCircle = new PointF[33];

        Pen Xpen = new Pen(Brushes.Blue, 2);
        Pen Ypen = new Pen(Brushes.Black, 2);
        Pen Zpen = new Pen(Brushes.Red, 2);

        Point refPoint;
        MODE mouseMode = MODE.INACTIVE;
        MyMatrix refMtx = new MyMatrix();

        Timer timer = new Timer();
        public RotateView()
        {
            InitializeComponent();
            timer.Interval = 100;
            timer.Tick += Timer_Tick;
            timer.Enabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            rotateMtx.Multiply(Xrotate);
            rotateMtx.Multiply(Yrotate);
            rotateMtx.Multiply(Zrotate);
            RotateView_Paint(null, null);
        }

        private void RotateView_Load(object sender, EventArgs e)
        {

        }

        private void RotateView_Paint(object sender, PaintEventArgs e)
        {
            int height = pictureBox1.Height;
            int width = pictureBox1.Width;
            timer.Enabled = false;

            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            Graphics grfx = Graphics.FromImage(bitmap);

            for (int i =0; i<circle.Length;i++)
            {
                float x = (circle[i].X * width * 0.8F) /2;
                float y = (circle[i].Y * height * 0.8F) / 2;

                float[] Xcoords = rotateMtx.TranformVector(new float[] { x, y, 0 });
                float[] Ycoords = rotateMtx.TranformVector(new float[] { x, 0, y });
                float[] Zcoords = rotateMtx.TranformVector(new float[] { 0, y, x });

                XCircle[i].X = Xcoords[0] + width / 2;
                XCircle[i].Y = Xcoords[1] + height / 2;

                YCircle[i].X = Ycoords[0] + width / 2;
                YCircle[i].Y = Ycoords[1] + height / 2;

                ZCircle[i].X = Zcoords[0] + width / 2;
                ZCircle[i].Y = Zcoords[1] + height / 2;
            }

            grfx.DrawLines(Xpen, XCircle);
            grfx.DrawLines(Ypen, YCircle);
            grfx.DrawLines(Zpen, ZCircle);

            pictureBox1.Image = bitmap;
            //timer.Enabled = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            refPoint = e.Location;
            mouseMode = MODE.XYAXIS;
            refMtx = new MyMatrix(rotateMtx);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            switch(mouseMode)
            {
                case MODE.XYAXIS:
                    break;
                case MODE.ZAXIS:
                    break;
                case MODE.OTHER:
                case MODE.INACTIVE:
                default:
                    break;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseMode = MODE.INACTIVE;
        }

        enum MODE
        {
            INACTIVE,
            ZAXIS,
            XYAXIS,
            OTHER
        }
    }
}
