using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace STL_Edit
{
    public class stlObject
    {
        public List<stlTriangle> Facettes = new List<stlTriangle>();
        public string DesignName = "Unnamed";

        public double MaxX = float.MinValue;
        public double MinX = float.MaxValue;

        public double MaxY = float.MinValue;
        public double MinY = float.MaxValue;

        public double MaxZ = float.MinValue;
        public double MinZ = float.MaxValue;

        MyMatrix _rotMtx = new MyMatrix();

        float scaleby = 634;

        public List<stlObject> parts = new List<stlObject>();

        public EditSTL Parent;

        public stlObject(EditSTL parent)
        {
            Parent = parent;
        }

        public STLERROR Load(string FileName, LOADTYPE loadType)
        {
            DesignName = Path.GetFileNameWithoutExtension(FileName);
            Stream stlFile = File.Open(FileName, FileMode.Open, FileAccess.Read);

            try
            {
                switch (loadType)
                {
                    default:
                    case LOADTYPE.STL_FILE:
                        return Load(stlFile);

                    case LOADTYPE.OBJ_WAVEFRONT:
                        return LoadWavefrontObj(stlFile);
                }
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

        public STLERROR Save(string FileName, SAVETYPE saveType)
        {
            DesignName = Path.GetFileNameWithoutExtension(FileName);

            Stream stlFile = File.Create(FileName);
            try
            {
                switch (saveType)
                {
                    default:
                    case SAVETYPE.STL_ASCII:
                        return SaveASCII(stlFile);
                    case SAVETYPE.STL_BINARY:
                        return SaveBinary(stlFile);
                    case SAVETYPE.OBJ_WAVEFRONT:
                        return SaveWavefrontObj(stlFile);
                }
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

            StlFile.Close();

            //ExplodeDesign();
            return LoadResult;
        }

        public STLERROR LoadASCII(Stream StlFile)
        {
            STLERROR LoadResult = STLERROR.SUCCESS;

            TextReader ModelFile = new StreamReader(StlFile);

            string line = ModelFile.ReadLine();
            stlTriangle Facette = new stlTriangle();
            int vtxidx = 0;

            while (line != null)
            {
                string[] tokens = line.Replace(",", "").Split(" \r\n\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                switch (tokens[0])
                {
                    case "solid":
                        break;
                    case "facet":
                        float XVector, YVector, ZVector;
                        float.TryParse(tokens[2], out XVector);
                        float.TryParse(tokens[3], out YVector);
                        float.TryParse(tokens[4], out ZVector);
                        Facette.Normal.X = XVector;
                        Facette.Normal.Y = YVector;
                        Facette.Normal.Z = ZVector;
                        break;
                    case "outer":
                        break;
                    case "vertex":
                        float XCoord, YCoord, ZCoord;
                        float.TryParse(tokens[1], out XCoord);
                        float.TryParse(tokens[2], out YCoord);
                        float.TryParse(tokens[3], out ZCoord);

                        Facette.Vertexes[vtxidx] = new stlVertex();
                        Facette.Vertexes[vtxidx].X = XCoord;
                        Facette.Vertexes[vtxidx].Y = YCoord;
                        Facette.Vertexes[vtxidx].Z = ZCoord;

                        vtxidx++;
                        break;
                    case "endloop":
                        break;
                    case "endfacet":
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
                        vtxidx = 0;
                        Facette = new stlTriangle();
                        break;
                    case "endsolid":
                        break;
                }
                line = ModelFile.ReadLine();
            }

            StlFile.Close();

            //ExplodeDesign();
            return LoadResult;
        }

        public STLERROR LoadWavefrontObj(Stream StlFile)
        {
            STLERROR LoadResult = STLERROR.SUCCESS;
            List<stlVertex> vertices = new List<stlVertex>();

            TextReader ModelFile = new StreamReader(StlFile);

            string line = ModelFile.ReadLine();

            while (line != null)
            {
                string[] tokens = line.Replace(",", "").Split(" \r\n\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 0)
                {
                    switch (tokens[0])
                    {
                        case "o":
                        case "s":
                        case "g":
                            break;
                        case "f":
                            int vertex1, vertex2, vertex3;

                            int.TryParse(tokens[1].Split(new string[]{ "//"}, StringSplitOptions.RemoveEmptyEntries)[0] , out vertex1);
                            int.TryParse(tokens[2].Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0], out vertex2);
                            int.TryParse(tokens[3].Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[0], out vertex3);

                            stlTriangle Facette = new stlTriangle();
                            Facette.Vertexes[0] = vertices[vertex1 - 1];
                            Facette.Vertexes[1] = vertices[vertex2 - 1];
                            Facette.Vertexes[2] = vertices[vertex3 - 1];

                            Facette.Vertexes.ToList().ForEach(n =>
                            {
                                MaxX = (MaxX >= n.X) ? MaxX : n.X;
                                MinX = (MinX <= n.X) ? MinX : n.X;
                                MaxY = (MaxY >= n.Y) ? MaxY : n.Y;
                                MinY = (MinY <= n.Y) ? MinY : n.Y;
                                MaxZ = (MaxZ >= n.Z) ? MaxZ : n.Z;
                                MinZ = (MinZ <= n.Z) ? MinZ : n.Z;
                            });
                            this.Facettes.Add(Facette);
                            break;
                        case "v":
                            float XCoord, YCoord, ZCoord;
                            float.TryParse(tokens[1], out XCoord);
                            float.TryParse(tokens[2], out YCoord);
                            float.TryParse(tokens[3], out ZCoord);

                            stlVertex vertex = new stlVertex() { X = XCoord * scaleby, Y = YCoord * scaleby, Z = ZCoord * scaleby };

                            vertices.Add(vertex);
                            break;
                    }
                }
                line = ModelFile.ReadLine();
            }

            StlFile.Close();

            //ExplodeDesign();
            return LoadResult;
        }

        public STLERROR SaveASCII(Stream StlFile)
        {
            STLERROR SaveResult = STLERROR.SUCCESS;
            TextWriter saveFile = new StreamWriter(StlFile);

            saveFile.WriteLine($"solid {DesignName}");
            foreach(stlTriangle facette in Facettes)
            {
                saveFile.WriteLine($"facet normal 0 0 0");
                saveFile.WriteLine($" outer loop");
                foreach(stlVertex vert in facette.Vertexes)
                {
                    saveFile.WriteLine($"  vertex {vert.X} {vert.Y} {vert.Z}");
                }
                saveFile.WriteLine($" endloop");
                saveFile.WriteLine($"endfacet");

            }

            saveFile.WriteLine($"endsolid {DesignName}");

            saveFile.Close();

            return SaveResult;
        }

        public STLERROR SaveWavefrontObj(Stream StlFile)
        {
            STLERROR SaveResult = STLERROR.SUCCESS;
            TextWriter saveFile = new StreamWriter(StlFile);
            int vidx = 1;

            StringBuilder sbVert = new StringBuilder();
            StringBuilder sbFacc = new StringBuilder();

            saveFile.WriteLine($"#object {DesignName}\r\n");
            saveFile.WriteLine($"#o obj_0");
            
            foreach (stlTriangle facette in Facettes)
            {
                foreach (stlVertex vert in facette.Vertexes)
                {
                    sbVert.AppendLine($"v {vert.X} {vert.Y} {vert.Z}");
                }
                sbFacc.AppendLine($"f {vidx++} {vidx++} {vidx++}");
            }

            saveFile.Write(sbVert.ToString());

            saveFile.WriteLine($"# {vidx - 1} vertices\r\n");

            saveFile.WriteLine($"g group_0_{DateTime.Now.Millisecond}\r\n");
            saveFile.WriteLine($"s off\r\n");

            saveFile.Write(sbFacc.ToString());

            saveFile.WriteLine($"# {(vidx - 1) / 3} faces\r\n");
            saveFile.WriteLine($" #end of obj_0\r\n");

            saveFile.Close();

            return SaveResult;
        }

        public STLERROR SaveBinary(Stream StlFile)
        {
            STLERROR SaveResult = STLERROR.SUCCESS;
            byte[] Header = new byte[80];

            BinaryWriter rawFile = new BinaryWriter(StlFile);

            StlFile.Write(Header, 0, 80);

            rawFile.Write(this.Facettes.Count);

            foreach (stlTriangle Facette in this.Facettes)
            {
                rawFile.Write((float)Facette.Normal.X);
                rawFile.Write((float)Facette.Normal.Y);
                rawFile.Write((float)Facette.Normal.Z);

                rawFile.Write((float)Facette.Vertexes[0].X);
                rawFile.Write((float)Facette.Vertexes[0].Y);
                rawFile.Write((float)Facette.Vertexes[0].Z);

                rawFile.Write((float)Facette.Vertexes[1].X);
                rawFile.Write((float)Facette.Vertexes[1].Y);
                rawFile.Write((float)Facette.Vertexes[1].Z);

                rawFile.Write((float)Facette.Vertexes[2].X);
                rawFile.Write((float)Facette.Vertexes[2].Y);
                rawFile.Write((float)Facette.Vertexes[2].Z);

                rawFile.Write((Int16)0);
            }
            return SaveResult;
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
            GraphicsPath gPath1 = new GraphicsPath();
            GraphicsPath gPath2 = new GraphicsPath();

            float scaleX = Canvas.Width / (float)(MaxX - MinX);
            float scaleY = Canvas.Height / (float)(MaxY - MinY);

            float scaleBy = (scaleX < scaleY) ? scaleX : scaleY;

            float centerX = (float)(MaxX + MinX) / 2;
            float centerY = (float)(MaxY + MinY) / 2;
            float centerZ = (float)(MaxZ + MinZ) / 2;

            for (int select = start; select < end; select++)
            {
                stlTriangle triangle = Facettes[select];
                PointF[] points = new PointF[4];

                int idx = 0;
                stlVertex[] tfVertexes = new stlVertex[3];

                foreach (stlVertex vertex in triangle.Vertexes)
                {
                    float X = (float)(vertex.X - centerX) * scaleBy;// + (Canvas.Width / 2);
                    float Y = (float)(vertex.Y - centerY) * -scaleBy;// + (Canvas.Height / 2);
                    float Z = (float)(vertex.Z - centerZ) * scaleBy;

                    float[] rotVtx = rotMtx.TranformVector(new float[] { X, Y, Z });

                    tfVertexes[idx] = new stlVertex(rotVtx[0], rotVtx[1], rotVtx[2]);

                    points[idx++] = new PointF(rotVtx[0] + (Canvas.Width / 2), rotVtx[1] + (Canvas.Height / 2));
                }

                points[idx++] = points[0];
                double[] v1 = new double[3];
                double[] v2 = new double[3];

                stlVertex Normal = rotMtx.CalculateSurfaceNormal(new stlTriangle() { Vertexes = tfVertexes });

                v1[0] = Normal.X;
                v1[1] = Normal.Y;
                v1[2] = Normal.Z;

                v2[0] = 0.707F;
                v2[1] = 0.707F;
                v2[2] = 0;

                double normal = _rotMtx.DotProduct(v1, v2);

                gPath1.AddLines(points);
                gPath1.CloseFigure();

                _gfx1.DrawPath(Pens.Black, gPath1);

                gPath1 = new GraphicsPath();
                gPath2 = new GraphicsPath();
            }
            gPath1.FillMode = FillMode.Alternate;
            gPath2.FillMode = FillMode.Alternate;

            _gfx1.DrawPath(Pens.Black, gPath1);

        }

        public void Render(Bitmap Canvas,int start, int end)
        {
            if (gfx1 == null)
            {
                gfx1 = Graphics.FromImage(Canvas);
            }
            GraphicsPath gPath1 = new GraphicsPath();
            GraphicsPath gPath2 = new GraphicsPath();

            double scaleX = Canvas.Width / (MaxX - MinX);
            double scaleY = Canvas.Height / (MaxY - MinY);

            double scaleBy = (scaleX < scaleY) ? scaleX : scaleY;

            double centerX = (MaxX + MinX) / 2;
            double centerY = (MaxY + MinY) / 2;

            for(int select = start; select < end; select++)
            {
                stlTriangle triangle = Facettes[select];
                PointF[] points = new PointF[4];

                int idx = 0;

                foreach (stlVertex vertex in triangle.Vertexes)
                {
                    double X = (vertex.X - centerX) * scaleBy + (Canvas.Width / 2);
                    double Y = (vertex.Y - centerY) * -scaleBy + (Canvas.Height / 2);

                    points[idx++] = new PointF((float)X, (float)Y);
                }

                points[idx++] = points[0];
                double[] v1 = new double[3];
                double[] v2 = new double[3];

                v1[0] = triangle.Vertexes[1].X - triangle.Vertexes[0].X;
                v1[1] = triangle.Vertexes[1].Y - triangle.Vertexes[0].Y;
                v1[2] = triangle.Vertexes[1].Z - triangle.Vertexes[0].Z;

                v2[0] = triangle.Vertexes[2].X - triangle.Vertexes[1].X;
                v2[1] = triangle.Vertexes[2].Y - triangle.Vertexes[1].Y;
                v2[2] = triangle.Vertexes[2].Z - triangle.Vertexes[1].Z;

                double normal = _rotMtx.DotProduct(v1, v2);
                if (normal < 0)
                {
                    gPath1.AddLines(points);
                    gPath1.CloseFigure();
                }
                else
                {
                    gPath2.AddLines(points);
                    gPath2.CloseFigure();
                }
            }
            gfx1.DrawPath(Pens.Red, gPath2);
            gfx1.DrawPath(Pens.Black, gPath1);
        }

        public void Render(Bitmap Canvas1, Bitmap Canvas2, Bitmap Canvas3, int start, int end)
        {
            GraphicsPath gPath = new GraphicsPath();

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

            double scaleX1 = Canvas1.Width / (MaxX - MinX);
            double scaleY1 = Canvas1.Height / (MaxY - MinY);

            double scaleX2 = Canvas1.Width / (MaxX - MinX);
            double scaleY2 = Canvas1.Height / (MaxZ - MinZ);

            double scaleX3 = Canvas1.Width / (MaxY - MinY);
            double scaleY3 = Canvas1.Height / (MaxZ - MinZ);

            double scaleBy1 = 0.98f * ((scaleX1 < scaleY1) ? scaleX1 : scaleY1);
            double scaleBy2 = 0.98f * ((scaleX2 < scaleY2) ? scaleX2 : scaleY2);
            double scaleBy3 = 0.98f * ((scaleX3 < scaleY3) ? scaleX3 : scaleY3);

            double centerX = (MaxX + MinX) / 2;
            double centerY = (MaxY + MinY) / 2;
            double centerZ = (MaxZ + MinZ) / 2;

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
                    double X1 = (vertex.X - centerX) * scaleBy1 + (Canvas1.Width / 2);
                    double Y1 = (vertex.Y - centerY) * -scaleBy1 + (Canvas1.Height / 2);

                    double X2 = (vertex.X - centerX) * scaleBy2 + (Canvas1.Width / 2);
                    double Y2 = (vertex.Z - centerZ) * -scaleBy2 + (Canvas1.Height / 2);

                    double X3 = (vertex.Y - centerY) * scaleBy3 + (Canvas1.Width / 2);
                    double Y3 = (vertex.Z - centerZ) * -scaleBy3 + (Canvas1.Height / 2);

                    points1[idx] = new PointF((float)X1, (float)Y1);
                    points2[idx] = new PointF((float)X2, (float)Y2);
                    points3[idx++] = new PointF((float)X3, (float)Y3);
                }

                points1[idx] = points1[0];
                points2[idx] = points2[0];
                points3[idx] = points3[0];

                gfx1.DrawLines(Pens.Black, points1);
                gfx2.DrawLines(Pens.Red, points2);
                gfx3.DrawLines(Pens.Blue, points3);
            }
        }

        public void ExplodeDesign()
        {
            parts.Clear();

            foreach (stlTriangle facette in Facettes)
            {
                List<stlObject> touchingParts = new List<stlObject>();

                stlObject[] Parts = parts.ToArray();
                foreach(stlObject part in Parts)
                {
                    bool isTouching = false;
                    bool isInside = false;

                    foreach (stlVertex vtx in facette.Vertexes)
                    {
                        if ((vtx.X > part.MaxX) || (vtx.X < part.MinX)) continue;

                        if ((vtx.Y > part.MaxY) || (vtx.Y < part.MinY)) continue;

                        if ((vtx.Z > part.MaxZ) || (vtx.Z < part.MinZ)) continue;

                        isInside = true;
                    }

                    if (!isInside) continue;

                    foreach (stlTriangle triangle in part.Facettes)
                    {
                        foreach (stlVertex vertex_tri in triangle.Vertexes)
                        {
                            foreach (stlVertex vertex_fct in facette.Vertexes)
                            {
                                if (vertex_fct.X == vertex_tri.X && vertex_fct.Y == vertex_tri.Y && vertex_fct.Z == vertex_tri.Z)
                                {
                                    isTouching = true;
                                    break;
                                }
                                if (isTouching)
                                    break;
                            }
                            if (isTouching)
                                break;
                        }
                        if (isTouching)
                            break;
                    }
                    if (isTouching)
                    {
                        touchingParts.Add(part);
                    }

                    //if (touchingParts.Count > 1)
                    //    break;
                }
                if (touchingParts.Any())
                {
                    stlObject[] tp = touchingParts.ToArray();
                    stlObject obj = tp[0];
                    obj.Facettes.Add(facette);

                    foreach (stlVertex vtx in facette.Vertexes)
                    {
                        if (vtx.X > obj.MaxX) obj.MaxX = vtx.X;
                        if (vtx.X < obj.MinX) obj.MinX = vtx.X;

                        if (vtx.Y > obj.MaxY) obj.MaxY = vtx.Y;
                        if (vtx.Y < obj.MinY) obj.MinY = vtx.Y;

                        if (vtx.Z > obj.MaxZ) obj.MaxZ = vtx.Z;
                        if (vtx.Z < obj.MinZ) obj.MinZ = vtx.Z;
                    }

                    for (int idx = 1; idx<touchingParts.Count;idx++)
                    {
                        obj.Facettes.AddRange(tp[idx].Facettes.ToArray());

                        if (tp[idx].MaxX > obj.MaxX) obj.MaxX = tp[idx].MaxX;
                        if (tp[idx].MinX < obj.MinX) obj.MinX = tp[idx].MinX;

                        if (tp[idx].MaxY > obj.MaxY) obj.MaxY = tp[idx].MaxY;
                        if (tp[idx].MinY < obj.MinY) obj.MinY = tp[idx].MinY;

                        if (tp[idx].MaxZ > obj.MaxZ) obj.MaxZ = tp[idx].MaxZ;
                        if (tp[idx].MinZ < obj.MinZ) obj.MinZ = tp[idx].MinZ;

                        parts.Remove(tp[idx]);
                    }
                }
                else
                {
                    stlObject newpart = new stlObject(null);
                    newpart.Facettes.Add(facette);

                    foreach (stlVertex vtx in facette.Vertexes)
                    {
                        if (vtx.X > newpart.MaxX) newpart.MaxX = vtx.X;
                        if (vtx.X < newpart.MinX) newpart.MinX = vtx.X;

                        if (vtx.Y > newpart.MaxY) newpart.MaxY = vtx.Y;
                        if (vtx.Y < newpart.MinY) newpart.MinY = vtx.Y;

                        if (vtx.Z > newpart.MaxZ) newpart.MaxZ = vtx.Z;
                        if (vtx.Z < newpart.MinZ) newpart.MinZ = vtx.Z;
                    }

                    parts.Add(newpart);
                }
            }
            int partNum = 1;
            foreach(stlObject part in parts)
            {
                part.DesignName = $"{this.DesignName}-part {partNum++}";

                foreach(stlTriangle triangle in part.Facettes)
                    triangle.Vertexes.ToList().ForEach(n =>
                {
                    part.MaxX = (part.MaxX >= n.X) ? part.MaxX : n.X;
                    part.MinX = (part.MinX <= n.X) ? part.MinX : n.X;
                    part.MaxY = (part.MaxY >= n.Y) ? part.MaxY : n.Y;
                    part.MinY = (part.MinY <= n.Y) ? part.MinY : n.Y;
                    part.MaxZ = (part.MaxZ >= n.Z) ? part.MaxZ : n.Z;
                    part.MinZ = (part.MinZ <= n.Z) ? part.MinZ : n.Z;
                });

            }
        }

        public void HallowOut()
        {
            List<stlTriangle> newFacettes = new List<stlTriangle>();

            foreach (stlTriangle facette in Facettes)
            {
                stlVertex Normal = _rotMtx.CalculateSurfaceNormal(facette);

                stlTriangle newFacette = new stlTriangle();

                for(int idx = 0; idx < 3; idx++)
                {
                    stlVertex vtx = facette.Vertexes[idx];
                    newFacette.Vertexes[2 - idx] = new stlVertex(vtx.X + Normal.X * 2, vtx.Y + Normal.Y * 2, vtx.Z + Normal.Z * 2);
                }

                newFacettes.Add(newFacette);
            }

            Facettes.AddRange(newFacettes);
        }
    }

    public enum SAVETYPE
    {
        STL_ASCII = 1,
        STL_BINARY = 2,
        OBJ_WAVEFRONT = 3
    }

    public enum LOADTYPE
    {
        STL_FILE = 1,
        OBJ_WAVEFRONT = 2
    }

    public enum STLERROR
    {
        SUCCESS = 0,
        LOADERROR = 1,
        SAVEERROR = 1
    }
}
