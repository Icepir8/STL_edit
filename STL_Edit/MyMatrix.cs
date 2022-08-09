using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STL_Edit
{
    public class MyMatrix
    {
        public float[,] Elements { get; set; }

        public MyMatrix()
        {
            Elements = new float[3, 3] { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
        }

        public MyMatrix(float[,] InitElements)
        {
            Elements = InitElements;
        }

        public MyMatrix(MyMatrix mtx)
        {
            Elements = new float[3, 3];
            for (int j = 0; j < 3; j++)
                for (int i = 0; i < 3; i++)
                    Elements[i, j] = mtx.Elements[i, j];
        }

        public void Multiply(float[,] InElements)
        {

        }

        public void Multiply(MyMatrix myMatrix)
        {
            int i, j;
            float[,] a = new float[3, 3];
            float[,] b = myMatrix.Elements;

            for (j = 0; j < 3; j++)
                for (i = 0; i < 3; i++)
                    a[i, j] = Elements[i, 0] * b[0, j] +
                              Elements[i, 1] * b[1, j] +
                              Elements[i, 2] * b[2, j];
            for (j = 0; j < 3; j++)
                for (i = 0; i < 3; i++)
                    Elements[i, j] = a[i, j];
        }

        public float[] TranformVector(float[] src)
        {
            float[] dst = new float[3];

            dst[0] = Elements[0, 0] * src[0]
                   + Elements[1, 0] * src[1]
                   + Elements[2, 0] * src[2];

            dst[1] = Elements[0, 1] * src[0]
                   + Elements[1, 1] * src[1]
                   + Elements[2, 1] * src[2];

            dst[2] = Elements[0, 2] * src[0]
                   + Elements[1, 2] * src[1]
                   + Elements[2, 2] * src[2];

            return dst;
        }

        public double DotProduct(double[] v1, double[] v2)
        {
            double result;
            double normalV1 = (double)Math.Sqrt(v1[0] * v1[0] + v1[1] * v1[1] + v1[2] * v1[2]);
            double normalV2 = (double)Math.Sqrt(v2[0] * v2[0] + v2[1] * v2[1] + v2[2] * v2[2]);

            result = v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];

            return (result);
        }
        public stlVertex CalculateSurfaceNormal(stlTriangle Triangle)
        {

            stlVertex Normal = new stlVertex();

            double[] v1 = new double[3];
            double[] v2 = new double[3];

            v1[0] = Triangle.Vertexes[0].X - Triangle.Vertexes[1].X;
            v1[1] = Triangle.Vertexes[0].Y - Triangle.Vertexes[1].Y;
            v1[2] = Triangle.Vertexes[0].Z - Triangle.Vertexes[1].Z;

            v2[0] = Triangle.Vertexes[2].X - Triangle.Vertexes[1].X;
            v2[1] = Triangle.Vertexes[2].Y - Triangle.Vertexes[1].Y;
            v2[2] = Triangle.Vertexes[2].Z - Triangle.Vertexes[1].Z;

            double[] Cp = CrossProduct(v1, v2);

            Normal.X = Cp[0];
            Normal.Y = Cp[1];
            Normal.Z = Cp[2];

            return Normalize(Normal);

        }
        public stlVertex Normalize(stlVertex vector)
        {
            stlVertex normalized = new stlVertex();
            float length = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y + vector.Z * vector.Z);

            if (length != 0)
            {
                normalized.X = vector.X / length;
                normalized.Y = vector.Y / length;
                normalized.Z = vector.Z / length;
            }

            return normalized;
        }

        public double[] CrossProduct(double[] v1, double[] v2)
        {
            double[] v3 = new double[3];
            v3[0] = v1[1] * v2[2] - v1[2] * v2[1];
            v3[1] = v1[2] * v2[0] - v1[0] * v2[2];
            v3[2] = v1[0] * v2[1] - v1[1] * v2[0];

            return v3;
        }
    }
}
