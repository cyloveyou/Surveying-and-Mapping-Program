using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 曲线拟合
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
                string[] lines = sr.ReadToEnd().Split('\n');
                foreach (var item in lines)
                {
                    string[] Data = item.Split(',');
                    myData.PList.Add(new MyPoint(Data[0], Data[1], Data[2]));
                }
                return true;
            }
        }

        public static bool SaveFile(MyData myData, string str)
        {
            //记得更改outFile
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请选择保存文件路径文件";
            sfd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (sfd.ShowDialog() != DialogResult.OK) { return false; }

            myData.outFile = sfd.FileName;
            using (StreamWriter sw = new StreamWriter(myData.outFile))//记得更改outFile
            {
                sw.Write(str);
                return true;
            }
        }
    }
}
