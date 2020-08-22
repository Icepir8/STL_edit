using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STL_Edit
{
    class stlObject
    {
        public List<stlTriangle> Facettes = new List<stlTriangle>();
        public string DesignName = "Unnamed";

        float MaxX = float.MinValue;
        float MinX = float.MaxValue;

        float MaxY = float.MinValue;
        float MinY = float.MaxValue;

        float MaxZ = float.MinValue;
        float MinZ = float.MaxValue;

        public STLERROR Load(string FileName)
        {
            DesignName = Path.GetFileNameWithoutExtension(FileName);

            Stream stlFile = File.Open(FileName, FileMode.Open, FileAccess.Read);
            try
            {
                return Load(stlFile);
            }
            catch (Exception exp)
            {
                return STLERROR.LOADERROR;
            }
            finally
            {
                stlFile.Close();
            }
        }

        public STLERROR Load(Stream StlFile)
        {
            BinaryReader rawFile = new BinaryReader(StlFile);

            byte[] buffer = rawFile.ReadBytes(6);

            StlFile.Seek(0, SeekOrigin.Begin);

            string SolidText = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

            if (SolidText.StartsWith("solid ", StringComparison.InvariantCultureIgnoreCase))
                return LoadASCII(StlFile);
            else
                return LoadBinary(StlFile);
        }

        public STLERROR LoadBinary(Stream StlFile)
        {
            STLERROR LoadResult = STLERROR.SUCCESS;

            BinaryReader rawFile = new BinaryReader(StlFile);

            byte[] buffer = rawFile.ReadBytes(80);

            int NumberFacettes = rawFile.ReadInt32();

            for (int cnt = 0; cnt < NumberFacettes; cnt++)
            {
                stlTriangle Facette = new stlTriangle();

                Facette.Normal.X = rawFile.ReadSingle();
                Facette.Normal.Y = rawFile.ReadSingle();
                Facette.Normal.Z = rawFile.ReadSingle();

                Facette.Vertexes[0] = new stlVertex();
                Facette.Vertexes[0].X = rawFile.ReadSingle();
                Facette.Vertexes[0].Y = rawFile.ReadSingle();
                Facette.Vertexes[0].Z = rawFile.ReadSingle();

                Facette.Vertexes[1] = new stlVertex();
                Facette.Vertexes[1].X = rawFile.ReadSingle();
                Facette.Vertexes[1].Y = rawFile.ReadSingle();
                Facette.Vertexes[1].Z = rawFile.ReadSingle();

                Facette.Vertexes[2] = new stlVertex();
                Facette.Vertexes[2].X = rawFile.ReadSingle();
                Facette.Vertexes[2].Y = rawFile.ReadSingle();
                Facette.Vertexes[2].Z = rawFile.ReadSingle();

                int bytesToSkip = rawFile.ReadInt16();

                if (bytesToSkip > 0)
                {
                    byte[] filler = rawFile.ReadBytes(bytesToSkip);
                }

                Facette.Vertexes.ToList().ForEach(n =>
                {
                    MaxX = (MaxX >= n.X) ? MaxX : n.X;
                    MinX = (MinX <= n.X) ? MinX : n.X;
                    MaxY = (MaxY >= n.Y) ? MaxY : n.Y;
                    MinY = (MinY <= n.Y) ? MinY : n.Y;
                    MaxZ = (MaxZ >= n.Z) ? MaxZ : n.Z;
                    MinZ = (MinZ <= n.Z) ? MinZ : n.Z;
                });
                Facettes.Add(Facette);
            }

            return LoadResult;
        }

        public STLERROR LoadASCII(Stream StlFile)
        {
            STLERROR LoadResult = STLERROR.SUCCESS;

            return LoadResult;
        }

        public void Render(Bitmap Canvas)
        {
            Render(Canvas, 0, Facettes.Count);
        }

        Graphics gfx1;
        Graphics gfx2;
        Graphics gfx3;

        Graphics gfxrot;

        public void RenderRot(Bitmap Canvas)
        {
            if (gfxrot == null)
            {
                gfxrot = Graphics.FromImage(Canvas);
            }

            PointF[] axisX = new PointF[32];
            PointF[] axisY = new PointF[32];
            PointF[] axisZ = new PointF[32];

            for (int i = 0;i < 8; i++)
            {

            }
        }

        public void RenderRot(Bitmap Canvas,MyMatrix rotMtx, int start, int end)
        {
            Graphics _gfx1 = Graphics.FromImage(Canvas);

            float scaleX = Canvas.Width / (MaxX - MinX);
            float scaleY = Canvas.Height / (MaxY - MinY);

            float scaleBy = (scaleX < scaleY) ? scaleX : scaleY;

            float centerX = (MaxX + MinX) / 2;
            float centerY = (MaxY + MinY) / 2;
            float centerZ = (MaxZ + MinZ) / 2;

            for (int select = start; select < end; select++)
            {
                stlTriangle triangle = Facettes[select];
                PointF[] points = new PointF[4];

                int idx = 0;

                foreach (stlVertex vertex in triangle.Vertexes)
                {
                    float X = (vertex.X - centerX) * scaleBy;// + (Canvas.Width / 2);
                    float Y = (vertex.Y - centerY) * -scaleBy;// + (Canvas.Height / 2);
                    float Z = (vertex.Z - centerZ) * scaleBy;

                    float[] rotVtx = rotMtx.TranformVector(new float[] { X, Y, Z });

                    points[idx++] = new PointF(rotVtx[0] + (Canvas.Width / 2), rotVtx[1] + (Canvas.Height / 2));
                }

                points[idx++] = points[0];

                _gfx1.DrawLines(Pens.Black, points);
            }
        }

        public void Render(Bitmap Canvas,int start, int end)
        {
            if (gfx1 == null)
            {
                gfx1 = Graphics.FromImage(Canvas);
            }

            float scaleX = Canvas.Width / (MaxX - MinX);
            float scaleY = Canvas.Height / (MaxY - MinY);

            float scaleBy = (scaleX < scaleY) ? scaleX : scaleY;

            float centerX = (MaxX + MinX) / 2;
            float centerY = (MaxY + MinY) / 2;

            for(int select = start; select < end; select++)
            {
                stlTriangle triangle = Facettes[select];
                PointF[] points = new PointF[4];

                int idx = 0;

                foreach (stlVertex vertex in triangle.Vertexes)
                {
                    float X = (vertex.X - centerX) * scaleBy + (Canvas.Width / 2);
                    float Y = (vertex.Y - centerY) * -scaleBy + (Canvas.Height / 2);

                    points[idx++] = new PointF(X, Y);
                }

                points[idx++] = points[0];

                gfx1.DrawLines(Pens.Black, points);
            }
        }

        public void Render(Bitmap Canvas1, Bitmap Canvas2, Bitmap Canvas3, int start, int end)
        {
            if (gfx1 == null || start <= 0)
            {
                gfx1 = Graphics.FromImage(Canvas1);
                gfx2 = Graphics.FromImage(Canvas2);
                gfx3 = Graphics.FromImage(Canvas3);

                Rectangle rect = new Rectangle(new Point(0, 0), Canvas1.Size);
                int height = Canvas1.Height - 3;
                int width = Canvas1.Width - 3;

                Point[] pnts = new Point[5] { new Point(2, 2), new Point(height, 2), new Point(height, width), new Point(2, width), new Point(2, 2) };

                Pen pen1 = new Pen(Color.Black, 2);
                Pen pen2 = new Pen(Color.Red, 2);
                Pen pen3 = new Pen(Color.Blue, 2);

                gfx1.DrawRectangle(pen1, rect);
                gfx2.DrawRectangle(pen2, rect);
                gfx3.DrawRectangle(pen3, rect);
            }

            float scaleX1 = Canvas1.Width / (MaxX - MinX);
            float scaleY1 = Canvas1.Height / (MaxY - MinY);

            float scaleX2 = Canvas1.Width / (MaxX - MinX);
            float scaleY2 = Canvas1.Height / (MaxZ - MinZ);

            float scaleX3 = Canvas1.Width / (MaxY - MinY);
            float scaleY3 = Canvas1.Height / (MaxZ - MinZ);

            float scaleBy1 = 0.98f * ((scaleX1 < scaleY1) ? scaleX1 : scaleY1);
            float scaleBy2 = 0.98f * ((scaleX2 < scaleY2) ? scaleX2 : scaleY2);
            float scaleBy3 = 0.98f * ((scaleX3 < scaleY3) ? scaleX3 : scaleY3);

            float centerX = (MaxX + MinX) / 2;
            float centerY = (MaxY + MinY) / 2;
            float centerZ = (MaxZ + MinZ) / 2;

            for (int select = start; select < end; select++)
            {
                if (select >= Facettes.Count)
                    break;

                stlTriangle triangle = Facettes[select];
                PointF[] points1 = new PointF[4];
                PointF[] points2 = new PointF[4];
                PointF[] points3 = new PointF[4];

                int idx = 0;

                foreach (stlVertex vertex in triangle.Vertexes)
                {
                    float X1 = (vertex.X - centerX) * scaleBy1 + (Canvas1.Width / 2);
                    float Y1 = (vertex.Y - centerY) * -scaleBy1 + (Canvas1.Height / 2);

                    float X2 = (vertex.X - centerX) * scaleBy2 + (Canvas1.Width / 2);
                    float Y2 = (vertex.Z - centerZ) * -scaleBy2 + (Canvas1.Height / 2);

                    float X3 = (vertex.Y - centerY) * scaleBy3 + (Canvas1.Width / 2);
                    float Y3 = (vertex.Z - centerZ) * -scaleBy3 + (Canvas1.Height / 2);

                    points1[idx] = new PointF(X1, Y1);
                    points2[idx] = new PointF(X2, Y2);
                    points3[idx++] = new PointF(X3, Y3);
                }

                points1[idx] = points1[0];
                points2[idx] = points2[0];
                points3[idx] = points3[0];

                gfx1.DrawLines(Pens.Black, points1);
                gfx2.DrawLines(Pens.Red, points2);
                gfx3.DrawLines(Pens.Blue, points3);
            }
        }
    }

    public enum STLERROR
    {
        SUCCESS = 0,
        LOADERROR = 1,
    }
}
