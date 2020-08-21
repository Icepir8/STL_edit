using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STL_Edit
{
    public class stlTriangle
    {
        public stlNormal Normal { get; set; }
        public stlVertex[] Vertexes { get; set; }

        public stlTriangle()
        {
            this.Normal = new stlNormal();
            this.Vertexes = new stlVertex[3];
        }
    }
}
