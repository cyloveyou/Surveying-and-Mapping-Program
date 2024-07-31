using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大地线长度计算
{
    public class MyFile
    {
        public static bool OpenFile(MyData myData)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "请选择原始数据文件路径",
                Filter = "文本文件|*.txt|所有文件|*.*"
            };
            if (ofd.ShowDialog() != DialogResult.OK) { return false; }
            myData.inFile = ofd.FileName;

            using (StreamReader sr = new StreamReader(myData.inFile))
            {
                string[] aff = sr.ReadLine().Split(',');    //读取长半轴和扁率
                myData.myE = new MyEllipsoid(aff[0], aff[1]);

                string[] lines = sr.ReadToEnd().Split('\n');
                foreach (var item in lines)
                {
                    string[] Data = item.Split(',');
                    //对于有空行的数据,一定要做一个if判断
                    if (Data.Length == 6)
                    {
                        MyPoint p1 = new MyPoint(Data[0], Data[1], Data[2]);//起点
                        MyPoint p2 = new MyPoint(Data[3], Data[4], Data[5]);//终点
                        myData.myLine.Add(new MyLine(p1, p2));              //大地线
                    }
                }
                return true;
            }
        }

        public static bool SaveFile(MyData myData, string str)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "请选择原始数据文件路径",
                Filter = "文本文件|*.txt|所有文件|*.*"
            };
            if (sfd.ShowDialog() != DialogResult.OK) { return false; }
            myData.outFile = sfd.FileName;

            using (StreamWriter sw = new StreamWriter(myData.outFile))
            {
                sw.Write(str);
            }
            return true;
        }
    }
}
