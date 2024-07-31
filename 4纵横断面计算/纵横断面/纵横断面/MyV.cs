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
    /// <summary>
    /// 单个纵断面
    /// </summary>
    public class MyV
    {
        public double H0;                                   //纵断面参考高程值
        public List<string> KName = new List<string>();     //关键点的点名
        public List<MyPoint> Kp = new List<MyPoint>();      //关键点列表
        public List<MyPoint> obsP = new List<MyPoint>();    //外业采集带高程的点

        public double D;                                    //纵断面长度
        public List<double> DList;                          //分段长度

        public double S;                                    //纵断面面积
        public List<double> SList;                          //分段面积

        public List<double> alphaList = new List<double>(); //分段方位角
        public List<MyPoint> NewPList = new List<MyPoint>();//内插后的点

        public List<MyH> myH_S = new List<MyH>();           //纵断面上的横断面

        /// <summary>
        /// 根据k点,计算纵断面长度
        /// </summary>
        /// <returns></returns>
        public void CalD()
        {
            this.D = 0;
            DList = new List<double>();
            for (int i = 0; i < Kp.Count - 1; i++)
            {
                double temp = Kp[i].DistToOther(Kp[i + 1]);
                DList.Add(temp);
            }
            this.D = DList.Sum();
        }

        /// <summary>
        /// 根据插入点和k点,计算纵断面面积
        /// </summary>
        /// <param name="H0"></param>
        public void CalS()
        {
            double temps = 0;
            SList = new List<double>();
            for (int i = 0; i < NewPList.Count - 1; i++)
            {
                if (i != 0 && NewPList[i].Name.Contains('K'))//如果不是第0个点,并且是k点
                {
                    SList.Add(temps);
                    temps = 0;      //面积置为零
                }
                double NowS = NewPList[i].CalS(NewPList[i + 1], H0);
                temps += NowS;
            }

            SList.Add(temps);
            this.S = SList.Sum();
        }

        /// <summary>
        /// 计算纵断面上的点
        /// </summary>
        public void interPoint()
        {
            this.NewPList = new List<MyPoint>();
            double delta = 10;
            double L = 0;//剩余的距离

            for (int i = 0; i < Kp.Count - 1; i++)
            {
                double alpha = Kp[i + 1].AZToOther(Kp[i]);      //方位角k1->k0
                this.alphaList.Add(Kp[i].AZToOther(Kp[i + 1])); //方位角k0->k1
                this.NewPList.Add(Kp[i]);


                L += Kp[i].DistToOther(Kp[i + 1]);
                int count = 0;
                while (L > delta)
                {
                    count += 1;
                    L -= delta;
                    double x = Kp[i + 1].X + L * Math.Cos(alpha);
                    double y = Kp[i + 1].Y + L * Math.Sin(alpha);

                    MyPoint mp = new MyPoint();
                    if (i == 0)//第一段的点
                    {
                        mp = new MyPoint($"Z{count}", x, y, 0);
                    }
                    if (i == 1)//第二段的点
                    {
                        mp = new MyPoint($"Y{count}", x, y, 0);
                    }
                    mp.InterPH(this.obsP);
                    this.NewPList.Add(mp);
                }
            }
            this.NewPList.Add(Kp[Kp.Count - 1]);
        }

        /// <summary>
        /// 计算纵断面上的横断面
        /// </summary>
        public void CalMyH()
        {
            //计算横断面中心
            for (int i = 0; i < Kp.Count - 1; i++)
            {
                double x = (Kp[i].X + Kp[i + 1].X) / 2;
                double y = (Kp[i].Y + Kp[i + 1].Y) / 2;
                MyPoint mp = new MyPoint($"M{i + 1}", x, y, 0, obsP);//计算高程值

                double angle = Kp[i].AZToOther(Kp[i + 1]) + Math.PI / 2;

                this.myH_S.Add(new MyH(mp, angle));
            }

            //计算横断面上的点
            foreach (var item in myH_S)
            {
                item.interP(this.obsP);
            }

            //计算横断面面积
            foreach (var item in myH_S)
            {
                item.CalS(H0);
            }
        }
    }
}
