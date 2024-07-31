using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空间数据分析3_David
{
    class Datacenter
    {
        public List<Point> all_points = new List<Point>();
        public double[,] W;
        public double I_global;
        public Ellipse ellipse = new Ellipse();
        public double dertaX;
        public double dertaY;

        public double I_dertaX;
        public double I_dertaY;
    }
}
