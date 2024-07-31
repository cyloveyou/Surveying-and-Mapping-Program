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
    public class MyFile
    {
        public static bool OpenFile(MyData myData)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "请选择原始数据文件";
            ofd.Filter = "文本文件|*.txt|所有文件|*.*";
            if (ofd.ShowDialog() != DialogResult.OK) return false;

            myData.inFile = ofd.FileName;
            using (StreamReader sr = new StreamReader(myData.inFile))
            {
                //读取参考高程
                myData.myV.H0 = double.Parse(sr.ReadLine().Split(',')[1]);

                //读取k点
                myData.myV.KName = sr.ReadLine().Split(',').ToList();

                //读取A B点
                string[] Data = sr.ReadLine().Split(',');
                myData.A = new MyPoint(Data[0], Data[1], Data[2], "0");
                Data = sr.ReadLine().Split(',');
                myData.B = new MyPoint(Data[0], Data[1], Data[2], "0");

                //读取观测点
                while (!sr.EndOfStream)
                {
                    Data = sr.ReadLine().Split(',');
                    if (Data.Length == 4)
                    {
                        MyPoint mp = new MyPoint(Data[0], Data[1], Data[2], Data[3]);
                        if (myData.myV.KName.Contains(Data[0]))
                        {
                            myData.myV.Kp.Add(mp);
                        }
                        myData.myV.obsP.Add(mp);
                    }

                }
                return true;
            }
        }

        /// <summary>
        /// 保存报告
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
