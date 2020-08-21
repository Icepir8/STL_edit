using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STL_Edit
{
    public class stlNormal: stlVECTOR
    {
        public stlNormal(float X, float Y, float Z) : base(X, Y, Z)
        {
        }

        public stlNormal() : base()
        {
            Z = 1;
        }
    }
}
