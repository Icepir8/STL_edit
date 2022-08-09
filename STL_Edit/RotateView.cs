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

        public MyMatrix rotateMtx = new MyMatrix();

        MyMatrix Xrotate = new MyMatrix(new float[3, 3] { { 1, 0, 0 }, { 0, 0.9807853F, -0.1950903F }, { 0, 0.1950903F, 0.9807853F } });
        MyMatrix Yrotate = new MyMatrix(new float[3, 3] { { 0.9807853F, 0, 0.1950903F }, { 0, 1, 0 }, { -0.1950903F, 0, 0.9807853F } });
        MyMatrix Zrotate = new MyMatrix(new float[3, 3] { { 0.9807853F, -0.1950903F, 0 }, { 0.1950903F, 0.9807853F, 0 }, { 0, 0, 1 } });

        PointF[] scaledCircle = new PointF[33];
        PointF[] XCircle = new PointF[33];
        PointF[] YCircle = new PointF[33];
        PointF[] ZCircle = new PointF[33];
        PointF[] RCircle = new PointF[33];

        Pen Xpen = new Pen(Brushes.Blue, 2);
        Pen Ypen = new Pen(Brushes.Black, 2);
        Pen Zpen = new Pen(Brushes.Red, 2);
        Pen RPen = new Pen(Brushes.Green, 3);

        Point refPoint;
        MODE mouseMode = MODE.INACTIVE;
        MyMatrix refMtx = new MyMatrix();

        Timer timer = new Timer();

        public event EventHandler ViewChanged;

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
            
            grfx.Clear(Color.White);

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

                x = (circle[i].X * width) / 2;
                y = (circle[i].Y * height) / 2;

                RCircle[i].X = x + width / 2;
                RCircle[i].Y = y + height / 2;
            }

            RCircle[32] = RCircle[0];

            grfx.DrawLines(Xpen, XCircle);
            grfx.DrawLines(Ypen, YCircle);
            grfx.DrawLines(Zpen, ZCircle);
            grfx.DrawLines(RPen, RCircle);
            pictureBox1.Image = bitmap;
            //timer.Enabled = true;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            refPoint = e.Location;

            float CenterX = pictureBox1.Width / 2.0F;
            float CenterY = pictureBox1.Height / 2.0F;

            float posX = (e.Location.X - CenterX) / CenterX;
            float posY = (e.Location.Y - CenterY) / CenterY;

            float length = (float)Math.Sqrt(posX * posX + posY * posY);

            if (length > 0.8F)
            {
                mouseMode = MODE.ZAXIS;
            }
            else
            {
                mouseMode = MODE.XYAXIS;
            }
            refMtx = new MyMatrix(rotateMtx);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            float height = pictureBox1.Height * 0.8F / 2.0F;
            float width = pictureBox1.Width * 0.8F / 2.0f;

            MyMatrix tempMtx = new MyMatrix(refMtx);

            switch (mouseMode)
            {
                case MODE.XYAXIS:
                    float difX = (e.Location.X - refPoint.X) / height;
                    float difY = (e.Location.Y - refPoint.Y) / width;

                    float sinX = (difX > 1) ? 1 : (difX < -1) ? -1 : difX;
                    float sinY = (difY > 1) ? 1 : (difY < -1) ? -1 : difY;

                    float cosX = (float)Math.Sqrt(1 - (sinX * sinX));
                    float cosY = (float)Math.Sqrt(1 - (sinY * sinY));

                    MyMatrix rotX = new MyMatrix();
                    {
                        rotX.Elements[0, 0] = cosX;
                        rotX.Elements[0, 2] = -sinX;
                        rotX.Elements[2, 0] = sinX;
                        rotX.Elements[2, 2] = cosX;
                    };

                    MyMatrix rotY = new MyMatrix();
                    {
                        rotY.Elements[1, 1] = cosY;
                        rotY.Elements[1, 2] = -sinY;
                        rotY.Elements[2, 1] = sinY;
                        rotY.Elements[2, 2] = cosY;
                    };

                    tempMtx.Multiply(rotX);
                    tempMtx.Multiply(rotY);

                    rotateMtx = tempMtx;
                    RotateView_Paint(null, null);

                    if (ViewChanged != null)
                    {
                        ViewChanged(this, new EventArgs());
                    }
                    break;
                case MODE.ZAXIS:

                    float CenterX = pictureBox1.Width / 2.0F;
                    float CenterY = pictureBox1.Height / 2.0F;

                    float posX = (e.Location.X - CenterX) / CenterX;
                    float posY = (e.Location.Y - CenterY) / CenterY;

                    float length = (float)Math.Sqrt(posX * posX + posY * posY);

                    if (length > 0)
                    {
                        float sinZ = posY / length;
                        float cosZ = posX / length;

                        MyMatrix rotZ = new MyMatrix();
                        {
                            rotZ.Elements[0, 0] = -cosZ;
                            rotZ.Elements[0, 1] = -sinZ;
                            rotZ.Elements[1, 0] = sinZ;
                            rotZ.Elements[1, 1] = -cosZ;
                        };

                        tempMtx.Multiply(rotZ);

                        rotateMtx = tempMtx;
                        RotateView_Paint(null, null);

                        if (ViewChanged != null)
                        {
                            ViewChanged(this, new EventArgs());
                        }
                    }

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
