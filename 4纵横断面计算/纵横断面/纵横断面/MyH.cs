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
    /// 单个横断面
    /// </summary>
    public class MyH
    {
        public MyPoint CP = new MyPoint();//横断面中心点
        public double alpha; //横断面坐标方位角
        public List<MyPoint> PList;//纵断面上的点
        public double S;

        /// <summary>
        /// 根据横断面中心点和方位角构造纵断面上的点
        /// </summary>
        /// <param name="CP"></param>
        /// <param name="alpha"></param>
        public MyH(MyPoint CP, double alpha)
        {
            this.CP = CP;
            this.alpha = alpha;
        }

        /// <summary>
        /// 对横断面进行点插值
        /// </summary>
        /// <param name="obsP"></param>
        /// <param name="delta"></param>
        public void interP(List<MyPoint> obsP, double delta = 5)
        {
            PList = new List<MyPoint>();
            int count = 0;
            for (int i = -5; i <= 5; i++)
            {
                if (i == 0)     //等于0时给关键点插入进去
                {
                    PList.Add(CP);
                    continue;
                }
                count += 1;
                double xj = CP.X + i * delta * Math.Cos(alpha); //记得乘delta
                double yj = CP.Y + i * delta * Math.Sin(alpha);
                MyPoint mp = new MyPoint($"{count}", xj, yj, 0);
                mp.InterPH(obsP);
                PList.Add(mp);
            }
        }

        /// <summary>
        /// 根据插值点计算横断面面积
        /// </summary>
        /// <param name="H0"></param>
        public void CalS(double H0)
        {
            this.S = 0;
            for (int i = 0; i < PList.Count - 1; i++)
            {
                this.S += PList[i].CalS(PList[i + 1], H0);
            }
        }
    }
}
