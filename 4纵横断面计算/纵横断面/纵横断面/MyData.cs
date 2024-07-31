using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 纵横断面
{
    public class MyData
    {
        public string inFile = "";
        public string outFile = "";

        public MyV myV = new MyV();//纵断面

        public MyPoint A, B;

    }
}
