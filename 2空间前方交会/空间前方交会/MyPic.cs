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
    /// 单张像片参数
    /// </summary>
    public class MyPic
    {
        public const double ToRad = Math.PI / 180;//十进制度转弧度

        public double f;        //像片主距



        /// <summary>
        /// 像点坐标
        /// </summary>
        public double x;
        public double y;

        /// <summary>
        /// 模型基线分量
        /// </summary>
        public double Xs;
        public double Ys;
        public double Zs;

        /// <summary>
        /// 像空间辅助坐标
        /// </summary>
        public double u;
        public double v;
        public double w;


        /// <summary>
        /// 姿态元素
        /// </summary>
        public double phi;      //偏角
        public double omega;    //倾角
        public double kappa;    //旋角

        /// <summary>
        /// 旋转矩阵
        /// </summary>
        public double a1, a2, a3;
        public double b1, b2, b3;
        public double c1, c2, c3;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Xs"></param>
        /// <param name="Ys"></param>
        /// <param name="Zs"></param>
        /// <param name="phi"></param>
        /// <param name="omega"></param>
        /// <param name="kappa"></param>
        public MyPic(double f, double x, double y,
            double Xs, double Ys, double Zs,
            double phi, double omega, double kappa)
        {
            this.f = Math.Abs(f);
            this.x = x;
            this.y = y;
            this.Xs = Xs;
            this.Ys = Ys;
            this.Zs = Zs;
            this.phi = phi * ToRad;
            this.omega = omega * ToRad;
            this.kappa = kappa * ToRad;
        }

        /// <summary>
        /// 计算像空间辅助坐标
        /// </summary>
        public void CalUVW()
        {
            //计算旋转矩阵
            double sinphi = Math.Sin(phi);
            double cosphi = Math.Cos(phi);
            double sinomega = Math.Sin(omega);
            double cosomega = Math.Cos(omega);
            double sinkappa = Math.Sin(kappa);
            double coskappa = Math.Cos(kappa);

            a1 = cosphi * coskappa - sinphi * sinomega * sinkappa;
            a2 = -cosphi * sinkappa - sinphi * sinomega * sinkappa;
            a3 = -sinphi * cosomega;
            b1 = cosomega * sinkappa;
            b2 = cosomega * coskappa;
            b3 = -sinomega;
            c1 = sinphi * coskappa + cosphi * sinomega * sinkappa;
            c2 = -sinomega * sinkappa + cosphi * sinomega * coskappa;
            c3 = cosphi * cosomega;

            //计算像空间辅助坐标
            this.u = a1 * x + a2 * y + a3 * (-f);
            this.v = b1 * x + b2 * y + b3 * (-f);
            this.w = c1 * x + c2 * y + c3 * (-f);
        }
    }
}
