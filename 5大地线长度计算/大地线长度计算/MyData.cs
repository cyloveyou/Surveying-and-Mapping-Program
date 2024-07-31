using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大地线长度计算
{
    public class MyData
    {
        public string inFile = "";
        public string outFile = "";

        public MyEllipsoid myE;

        public List<MyLine> myLine = new List<MyLine>();
    }
}
