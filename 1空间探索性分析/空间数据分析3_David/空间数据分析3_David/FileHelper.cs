using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace 空间数据分析3_David
{
    class FileHelper
    {
        public void Open_File(Datacenter datacenter)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "选择文件";
            op.Filter = "文本数据|*.txt";
            if (op.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(op.FileName);
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string[] str = sr.ReadLine().Trim().Split(',');
                    string name = str[0];
                    double B = double.Parse(str[1]);
                    double L = double.Parse(str[2]);
                    double V = double.Parse(str[3]);
                    Point point = new Point(name, B, L,V);
                    datacenter.all_points.Add(point);
                }
            }
        }

        public void Save_Report(string report)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = "result";
            sf.Filter = "文本数据|*.txt";
            sf.Title = "选择路径";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sf.FileName);
                sw.Write(report);
                sw.Flush();
            }
        }

        public void Save_Image(Chart chart)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.FileName = "示意图";
            sf.Filter = "图片数据|*.jpg";
            sf.Title = "选择路径";
            if (sf.ShowDialog() == DialogResult.OK)
            {
                chart.SaveImage(sf.FileName, ChartImageFormat.Jpeg);
            }
        }
    }
}
