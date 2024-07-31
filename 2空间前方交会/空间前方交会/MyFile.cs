using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空间前方交会
{
    public class MyFile
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="myData"></param>
        /// <returns></returns>
        public static bool OpenFile(MyData myData)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择原始数据文件";
            ofd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) return false;

            myData.inFile = ofd.FileName;
            using (StreamReader sr = new StreamReader(myData.inFile))
            {
                string[] lines = sr.ReadToEnd().Split('\n');
                double[] parm1 = lines[0].Split(',').ToList().
                    ConvertAll(t => double.Parse(t)).ToArray();
                double[] parm2 = lines[1].Split(',').ToList().
                    ConvertAll(t => double.Parse(t)).ToArray();

                for (int i = 2; i < lines.Length; i++)
                {
                    string[] Data = lines[i].Split(',');
                    if (Data.Length == 4)
                    {
                        double[] dData = Data.ToList().ConvertAll(t => double.Parse(t)).ToArray();
                        MyPic p1 = new MyPic(parm1[0], dData[0], dData[1],
                            parm1[1], parm1[2], parm1[3],
                            parm1[4], parm1[5], parm1[6]);
                        MyPic p2 = new MyPic(parm2[0], dData[2], dData[3],
                            parm2[1], parm2[2], parm2[3],
                            parm2[4], parm2[5], parm2[6]);
                        myData.DP.Add(new DoublePic(p1, p2));
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="myData"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool SaveFile(MyData myData, string str)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "请选择保存文件路径"; 
            sfd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (sfd.ShowDialog() != DialogResult.OK) return false;

            myData.outFile = sfd.FileName;//记得改为outFile
            using (StreamWriter sw = new StreamWriter(myData.outFile))//记得改为outFile
            {
                sw.Write(str);
                return true;
            }
        }
    }
}
