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
    /// <summary>
    /// 一对像片上的一个点
    /// </summary>
    public class DoublePic
    {
        public MyPic p1;
        public MyPic p2;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public DoublePic(MyPic p1, MyPic p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// 摄影基线分量
        /// </summary>
        public double Bu;
        public double Bv;
        public double Bw;

        /// <summary>
        /// 投影系数
        /// </summary>
        public double N1;
        public double N2;

        /// <summary>
        /// 地面坐标
        /// </summary>
        public double X;
        public double Y;
        public double Z;

        /// <summary>
        /// 计算投影系数
        /// </summary>
        private void CalN()
        {
            this.Bu = p2.Xs - p1.Xs;
            this.Bv = p2.Ys - p1.Ys;
            this.Bw = p2.Zs - p1.Zs;

            double down = p1.u * p2.w - p2.u * p1.w;
            this.N1 = (this.Bu * p2.w - this.Bw * p2.u) / down;
            this.N2 = (this.Bu * p1.w - this.Bw * p1.u) / down;
        }

        /// <summary>
        /// 计算地面坐标
        /// </summary>
        public void CalXYZ()
        {
            //计算像空间辅助坐标
            p1.CalUVW();
            p2.CalUVW();
            //计算投影系数
            this.CalN();
            //计算地面坐标
            this.X = p1.Xs + N1 * p1.u;
            this.Y = 0.5 * ((p1.Ys + N1 * p1.v) + (p2.Ys + N2 * p2.v));
            this.Z = p1.Zs + N1 * p1.w;
        }
    }
}
