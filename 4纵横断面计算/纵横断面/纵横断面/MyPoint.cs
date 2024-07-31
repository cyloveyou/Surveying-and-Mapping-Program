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
    [DebuggerDisplay("Name={Name},x={X},y={Y},H={H},Dist={Dist}")]
    public class MyPoint
    {
        public string Name { get; set; }
        public double X { get; set; }       //X坐标
        public double Y { get; set; }       //Y坐标
        public double H { get; set; }       //高程值

        public double Dist;//与另一个点的距离,插值时使用

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public MyPoint() { }
        /// <summary>
        /// 从字符串数组构造
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="H"></param>
        public MyPoint(string Name, string X, string Y, string H, List<MyPoint> pList = null)
        {
            this.Name = Name;
            this.X = double.Parse(X);
            this.Y = double.Parse(Y);
            this.H = double.Parse(H);
            if (pList != null) //如果传入了点列表,重新计算高程
            {
                this.InterPH(pList);
            }
        }
        /// <summary>
        /// 从浮点数构造
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="H"></param>
        public MyPoint(string Name, double X, double Y, double H, List<MyPoint> pList = null)
        {
            this.Name = Name;
            this.X = X;
            this.Y = Y;
            this.H = H;
            if (pList != null)
            {
                this.InterPH(pList);
            }
        }


        /// <summary>
        /// 计算与另一个点之间的坐标方位角
        /// </summary>
        /// <param name="OP"></param>
        public double AZToOther(MyPoint OP)
        {
            double dx = OP.X - this.X;
            double dy = OP.Y - this.Y;
            return Math.Atan2(dy, dx) + (dy < 0 ? 1 : 0) * 2 * Math.PI;
        }

        /// <summary>
        /// 计算与另一个点之间的距离
        /// </summary>
        /// <param name="OP"></param>
        /// <returns></returns>
        public double DistToOther(MyPoint OP)
        {
            double dx = OP.X - this.X;
            double dy = OP.Y - this.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 根据PList,插值当前点高程,赋值并返回高程值
        /// </summary>
        /// <param name="PList"></param>
        /// <returns></returns>
        public double InterPH(List<MyPoint> PList)
        {
            List<MyPoint> temp = new List<MyPoint>();//新建一个List,防止覆盖PList
            foreach (var item in PList)
            {
                if (item.Name == this.Name) continue;//移除与当前点同名的点
                item.Dist = item.DistToOther(this);
                temp.Add(item);
            }

            temp = temp.OrderBy(t => t.Dist).ToList();//按照距离排序
            //选取最近的五个点计算
            double up = 0;
            double down = 0;
            for (int i = 0; i < 5; i++)
            {
                up += temp[i].H / temp[i].Dist;
                down += 1 / temp[i].Dist;
            }
            this.H = up / down;
            return this.H;
        }

        /// <summary>
        /// 计算与另一个点形成的面积
        /// </summary>
        /// <returns></returns>
        public double CalS(MyPoint OP, double H0)
        {
            return (this.H + OP.H - 2 * H0) / 2 * (this.DistToOther(OP));
        }
    }
}
