using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSS对流层
{
    public class MyFile
    {
        public static bool OpenFile(MyData myData)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择原始数据文件";
            ofd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) { return false; }

            myData.inFile = ofd.FileName;
            using (StreamReader sr = new StreamReader(myData.inFile))
            {
                sr.ReadLine();//忽略首行
                string[] lines = sr.ReadToEnd().Split('\n');
                foreach (var item in lines)
                {
                    string[] Data = item.Split(',');
                    if (Data.Length == 6)
                    {
                        MyStation mp = new MyStation(Data[0], Data[1], Data[2], Data[3], Data[4], Data[5]);
                        myData.myStation.Add(mp);
                    }
                }
                return true;
            }

        }

        public static bool SaveFile(MyData myData, string str)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Title = "请选择保存文件路径";
            sfd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (sfd.ShowDialog() != DialogResult.OK) { return false; }

            myData.outFile = sfd.FileName;
            using (StreamWriter sw = new StreamWriter(myData.outFile))
            {
                sw.Write(str);
                return true;
            }
        }
    }
}
