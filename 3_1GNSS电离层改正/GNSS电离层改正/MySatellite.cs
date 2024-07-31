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
    public class MySatellite
    {
        /// <summary>
        /// 卫星原始数据
        /// </summary>
        public string PRN { get; set; }
        //卫星地心坐标
        public double Xs { get; set; }
        public double Ys { get; set; }
        public double Zs { get; set; }

        //卫星站心坐标,计算的时候一定要注意是地心坐标还是站心坐标
        public double X;
        public double Y;
        public double Z;

        //卫星高度角和方位角
        public double AZ { get; set; }
        public double EL { get; set; }

        //观测到该卫星的时刻
        public MyTime obsTime;

        //电离层延迟
        public double L1T;//时间
        public double L1D { get; set; }//距离

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ObsTime"></param>
        /// <param name="PRN"></param>
        /// <param name="Xs"></param>
        /// <param name="Ys"></param>
        /// <param name="Zs"></param>
        public MySatellite(MyTime ObsTime, string PRN, string Xs, string Ys, string Zs)
        {
            this.obsTime = ObsTime;
            this.PRN = PRN; 
            this.Xs = double.Parse(Xs) * 1000;
            this.Ys = double.Parse(Ys) * 1000;
            this.Zs = double.Parse(Zs) * 1000;
            //this.EL = double.NaN;
            //this.AZ = double.NaN;
            //this.L1D = double.NaN;
        }

        /// <summary>
        /// 计算该卫星的高度角和方位角
        /// </summary>
        /// <param name="Station">测站</param>
        public void CalELAZ(MyStation Station)
        {
            //计算站心坐标，对于有sin、cos的计算，先将sin和cos计算，这样可以使用自动补全
            double sb = Math.Sin(Station.Bp);       //注意使用弧度参与运算
            double cb = Math.Cos(Station.Bp);
            double sl = Math.Sin(Station.Lp);
            double cl = Math.Cos(Station.Lp);

            double dx = this.Xs - Station.Xp;
            double dy = this.Ys - Station.Yp;
            double dz = this.Zs - Station.Zp;

            this.X = -sb * cl * dx - sb * sl * dy + cb * dz;
            this.Y = -sl * dx + cl * dy;
            this.Z = cb * cl * dx + cb * sl * dy + sb * dz;

            //计算卫星方位角A
            this.AZ = Math.Atan2(this.Y, this.X) + (Y < 0 ? 1 : 0) * 2 * Math.PI;
            //计算高度角E
            this.EL = Math.Atan(Z / Math.Sqrt(X * X + Y * Y));
        }

        /// <summary>
        /// 计算电离层延迟
        /// </summary>
        /// <param name="Station"></param>
        /// <param name="H0"></param>
        public void IonoDelay(MyStation Station, double H0 = 350)
        {
            if (EL < 0)//高度角小于0没有延迟
            {
                this.L1D = 0;
                this.L1T = 0;
                return;
            }
            //穿刺点IP的坐标
            double psi = 0.0137 / (EL / Math.PI + 0.11) - 0.022;
            double phi = Station.Bp / Math.PI + psi * Math.Cos(AZ);
            if (phi > 0.416) phi = 0.416;
            else if (phi < -0.416) phi = -0.416;

            double lambda = Station.Lp / Math.PI + psi * Math.Sin(AZ) / Math.Cos(phi * Math.PI);

            //穿刺点地磁纬度
            double phi_m = phi + 0.064 * Math.Cos((lambda - 1.617) * Math.PI);

            //克罗布歇模型改正
            double[] alpha = new double[] { 0.1397e-7, -0.7451e-8, -0.5960e-7, 0.1192e-6 };
            double[] beta = new double[] { 0.1270e6, -0.1966e6, 0.6554e5, 0.2621e6 };

            double A1 = 5e-9;
            double A2 = alpha[0] + alpha[1] * phi_m + alpha[2] * phi_m * phi_m + alpha[3] * phi_m * phi_m * phi_m;
            double A3 = 50400;
            double A4 = beta[0] + beta[1] * phi_m + beta[2] * phi_m * phi_m + beta[3] * phi_m * phi_m * phi_m;

            A2 = A2 < 0 ? 0 : A2;
            A4 = A4 < 72000 ? 72000 : A4;

            double F = 1 + 16 * Math.Pow((0.53 - EL / Math.PI), 3);

            //计算当地时间
            //double localHour = this.obsTime.hour + Station.Lp * 180 / Math.PI / 15;
            //localHour = localHour > 24 ? localHour - 24 : localHour;
            double T = obsTime.hour * 3600.0 + obsTime.min * 60.0 + obsTime.sec;
            double t = 43200 * lambda + T;

            double flag = Math.Abs(2 * Math.PI * (t - A3)) / A4;

            if (flag < 1.57)
                this.L1T = F * (A1 + A2 * (1 - Math.Pow(flag, 2) / 2 + Math.Pow(flag, 4) / 24));
            else
                this.L1T = F * A1;

            this.L1D = this.L1T * 299792458;
        }
    }
}
