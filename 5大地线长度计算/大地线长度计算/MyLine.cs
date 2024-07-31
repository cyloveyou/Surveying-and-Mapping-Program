using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大地线长度计算
{
    public class MyLine
    {
        const double PI = Math.PI;
        public MyPoint p1;      //起点
        public MyPoint p2;      //终点

        /// <summary>
        /// 辅助计算结果
        /// </summary>
        public double u1;
        public double u2;
        public double l;
        public double a1;
        public double a2;
        public double b1;
        public double b2;

        /// <summary>
        /// 计算起点大地方位角结果
        /// </summary>
        public double alpha;
        public double beta;
        public double gamma;
        public double A1;
        public double lambda;//经差
        public double sigma;
        public double sinA0;

        /// <summary>
        /// 计算大地线长度
        /// </summary>
        public double A, B, C;
        public double sigma1;
        public double S;


        public MyLine(MyPoint p1, MyPoint p2)
        {
            this.p1 = p1;
            this.p2 = p2;
        }

        /// <summary>
        /// 辅助计算
        /// </summary>
        public void Calab(MyEllipsoid myE)
        {
            this.u1 = Math.Atan(Math.Sqrt(1 - myE.e12) * Math.Tan(p1.B));
            this.u2 = Math.Atan(Math.Sqrt(1 - myE.e12) * Math.Tan(p2.B));
            this.l = p2.L - p1.L;               //注意不要减反了

            //因为计算Math.Sin需要敲很长,故先定义sin cos
            double su1 = Math.Sin(u1);
            double su2 = Math.Sin(u2);
            double cu1 = Math.Cos(u1);
            double cu2 = Math.Cos(u2);

            this.a1 = su1 * su2;
            this.a2 = cu1 * cu2;
            this.b1 = cu1 * su2;
            this.b2 = su1 * cu2;
        }

        /// <summary>
        /// 计算大地线长度
        /// </summary>
        /// <param name="myE"></param>
        public void CalS(MyEllipsoid myE)
        {
            this.Calab(myE);
            //迭代计算起点大地方位角
            double delta = 0;   //新的delta
            double delta_ = 0;  //旧的delta

            double sinA0;
            double cosA02;
            while (true)
            {
                lambda = l + delta;
                double p = Math.Cos(u2) * Math.Sin(lambda);
                double q = b1 - b2 * Math.Cos(lambda);
                A1 = Math.Atan(p / q);

                A1 = Math.Abs(A1);
                if (p > 0 && q < 0) { A1 = PI - A1; }
                else if (p < 0 && q < 0) { A1 = PI + A1; }
                else if (p < 0 && q > 0) { A1 = 2 * PI - A1; }

                if (A1 < 0) { A1 += 2 * PI; }
                else if (A1 > 2 * PI) { A1 -= 2 * PI; }

                //计算sigma
                double sinsigma = p * Math.Sin(A1) + q * Math.Cos(A1);
                double cossigma = a1 + a2 * Math.Cos(lambda);
                sigma = Math.Atan2(sinsigma, cossigma);
                if (cossigma > 0) { sigma = Math.Abs(sigma); }
                else { sigma = PI - Math.Abs(sigma); }

                sinA0 = Math.Cos(u1) * Math.Sin(A1);
                cosA02 = 1 - sinA0 * sinA0;
                sigma1 = Math.Atan(Math.Tan(u1)/ Math.Cos(A1));


                //计算alpha beta gamma
                double e12div2 = myE.e12 / 2;

                double e14div8 = myE.e12 * myE.e12 / 8;
                double e14div16 = myE.e12 * myE.e12 / 16;

                double e16div16 = myE.e12 * myE.e12 * myE.e12 / 16;
                double e16div32 = myE.e12 * myE.e12 * myE.e12 / 32;
                double e16div128 = myE.e12 * myE.e12 * myE.e12 / 128;
                double e16div256 = myE.e12 * myE.e12 * myE.e12 / 256;

                alpha = (e12div2 + e14div8 + e16div16) -
                    (e14div16 + e16div16) * cosA02 +
                    3 * e16div128 * cosA02 * cosA02;
                beta = (e14div16 + e16div16) * cosA02 -
                    e16div32 * cosA02 * cosA02;
                gamma = e16div256 * cosA02 * cosA02;

                delta_ = delta;     //要计算新的delta了,先更新旧的delta,再计算新的delta
                delta = (alpha * sigma +
                    beta * Math.Cos(2 * sigma1 + sigma) * Math.Sin(sigma) +
                    gamma * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma)) * sinA0;

                if (Math.Abs(delta - delta_) < 1e-10) { break; }        //当新旧delta差小于1e-10时,退出
            }

            double k2 = myE.e22 * cosA02;

            double k2div4 = k2 / 4;

            double k4div8 = k2 * k2 / 8;
            double k4div64 = k2 * k2 / 64;
            double k4div128 = k2 * k2 / 128;

            double k6div128 = k2 * k2 * k2 / 128;
            double k6div256 = k2 * k2 * k2 / 256;
            double k6div512 = k2 * k2 * k2 / 512;

            A = (1 - k2div4 + 7 * k4div64 - 15 * k6div256) / myE.b;
            B = (k2div4 - k4div8 + 37 * k6div512);
            C = (k4div128 - k6div128);

            double Xs = C * Math.Sin(2 * sigma) * Math.Cos(4 * sigma1 + 2 * sigma);
            S = (sigma - B * Math.Sin(sigma) * Math.Cos(2 * sigma1 + sigma)) / A;

        }
    }
}
