using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STL_Edit
{
    public abstract class stlVECTOR
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public stlVECTOR(double X, double Y, double Z)
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
