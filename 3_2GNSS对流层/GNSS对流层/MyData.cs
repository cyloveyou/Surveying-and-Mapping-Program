using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSS对流层
{
    public class MyData
    {
        public string inFile = "";
        public string outFile = "";

        public List<MyStation> myStation = new List<MyStation>();

    }
}
