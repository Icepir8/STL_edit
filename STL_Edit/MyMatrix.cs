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

        float DotProduct(float[] v1, float[] v2)
        {
            float result;

            result = v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];

            return (result);
        }

        float[] CrossProduct(float[] v1, float[] v2)
        {
            float[] v3 = new float[3];
            v3[0] = v1[1] * v2[2] - v1[2] * v2[1];
            v3[1] = v1[2] * v2[0] - v1[0] * v2[2];
            v3[2] = v1[0] * v2[1] - v1[1] * v2[0];

            return v3;
        }
    }
}
