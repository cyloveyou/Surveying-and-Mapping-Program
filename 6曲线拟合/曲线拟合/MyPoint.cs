using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 曲线拟合
{
    public class MyPoint
    {
        public string ID = "";
        public double x;
        public double y;

        /// <summary>
        /// 以当前点为起点的曲线参数
        /// </summary>
        public double E0;
        public double E1;
        public double E2;
        public double E3;

        public double F0;
        public double F1;
        public double F2;
        public double F3;

        /// <summary>
        /// 梯度
        /// </summary>
        public double sini;
        public double cosi;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MyPoint(string ID, string x, string y)
        {
            this.ID = ID;
            this.x = double.Parse(x);
            this.y = double.Parse(y);
        }

        public MyPoint(string ID, double x, double y)
        {
            this.ID = ID;
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// 计算梯度
        /// </summary>
        /// <param name="lp2"></param>
        /// <param name="lp1"></param>
        /// <param name="np1"></param>
        /// <param name="np2"></param>
        public void CalGrid(MyPoint lp_2, MyPoint lp_1, MyPoint np_1, MyPoint np_2)
        {
            //命名给命名成等宽,这样可以拷贝批量修改
            double a1 = lp_1.x - lp_2.x;
            double a2 = this.x - lp_1.x;
            double a3 = np_1.x - this.x;
            double a4 = np_2.x - np_1.x;

            //拷贝将a改成b,x改成y
            double b1 = lp_1.y - lp_2.y;
            double b2 = this.y - lp_1.y;
            double b3 = np_1.y - this.y;
            double b4 = np_2.y - np_1.y;

            double w2 = Math.Abs(a3 * b4 - a4 * b3);
            double w3 = Math.Abs(a1 * b2 - a2 * b1);
            double a0 = w2 * a2 + w3 * a3;
            double b0 = w2 * b2 + w3 * b3;

            double temp = Math.Sqrt(a0 * a0 + b0 * b0);
            //计算梯度
            this.cosi = a0 / temp;
            this.sini = b0 / temp;
        }

        /// <summary>
        /// 与另一个点的距离
        /// </summary>
        /// <param name="op"></param>
        public double DistToOP(MyPoint op)
        {
            double dx = op.x - this.x;
            double dy = op.y - this.y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 计算曲线参数
        /// </summary>
        /// <param name="p1"></param>
        public void CalEF(MyPoint p1)
        {
            double r = this.DistToOP(p1);
            double dx = p1.x - this.x;
            double dy = p1.y - this.y;

            this.E0 = this.x;
            this.E1 = r * this.cosi;
            this.E2 = 3 * dx - r * (p1.cosi + 2 * this.cosi);
            this.E3 = -2 * dx + r * (p1.cosi + this.cosi);

            //拷贝将x改成y;cosi改成sini
            this.F0 = this.y;
            this.F1 = r * this.sini;
            this.F2 = 3 * dy - r * (p1.sini + 2 * this.sini);
            this.F3 = -2 * dy + r * (p1.sini + this.sini);
        }
    }
}
