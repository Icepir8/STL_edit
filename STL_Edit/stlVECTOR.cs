using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STL_Edit
{
    public abstract class stlVECTOR
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public stlVECTOR(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public stlVECTOR()
        {
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
        }
    }
}
