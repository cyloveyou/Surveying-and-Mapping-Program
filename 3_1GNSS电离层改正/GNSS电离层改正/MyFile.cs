using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSS电离层改正
{
    public class MyFile
    {
        public const double ToRad = Math.PI / 180;//角度乘上ToRad转为弧度

        /// <summary>
        /// 打开文件
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
                myData.Station = new MyStation(-2225669.7744, 4998936.1598, 3265908.9678, 30 * ToRad, 114 * ToRad);
                MyTime myTime = new MyTime();
                while (!sr.EndOfStream)
                {
                    string Line = sr.ReadLine();
                    if (Line[0] == '*')//读取时间列
                    {
                        string[] Data = Line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        myTime = new MyTime(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6]);
                    }
                    else
                    {
                        string PRN = Line.Substring(0, 3);
                        string X = Line.Substring(3, 14);
                        string Y = Line.Substring(17, 14);
                        string Z = Line.Substring(31, 14);

                        myData.Station.Sats.Add(new MySatellite(myTime, PRN, X, Y, Z));
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

            myData.outFile = sfd.FileName;
            using (StreamWriter sw = new StreamWriter(myData.outFile))
            {
                sw.Write(str);
                return true;
            }
        }
    }
}
