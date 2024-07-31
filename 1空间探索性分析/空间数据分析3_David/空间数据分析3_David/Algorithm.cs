using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
namespace 空间数据分析3_David
{
    class Algorithm
    {
        public void Let_Ellipse(Ellipse ellipse,List<Point>points)
        {
            double avg_x = points.Average(o => o.x);
            double avg_y = points.Average(o => o.y);
            ellipse.X0 = avg_x;
            ellipse.Y0 = avg_y;

            int n = points.Count;
            double[] a = new double[n];
            double[] b = new double[n];
            int i = 0;
            
            double aibi=0;
            foreach(var p in points)
            {
                a[i] = p.x - avg_x;
                b[i] = p.y - avg_y;
                aibi += a[i] * b[i];
                i++;
            }
            double aibi2 = Pow(aibi, 2);
            double ai2 = a.Sum(o => o*o);
            double bi2 = b.Sum(o => o*o);

            double up1 = ai2 - bi2;
            double up2 = Sqrt(Pow(ai2 - bi2, 2) + 4 * aibi2);
            double up = up1 + up2;
            double down = 2 * aibi;
            double theta = Atan(up / down);
            double sin_theta = Sin(theta);
            double cos_theta = Cos(theta);
            ellipse.sin_theta = sin_theta;
            ellipse.cos_theta = cos_theta;

            double up_Ex = 0;
            double up_Ey = 0;
            for (int j=0;j<n;j++)
            {
                up_Ex += Pow(a[j] * cos_theta - b[j] * sin_theta,2);
                up_Ey += Pow(a[j] * sin_theta + b[j] * cos_theta,2);
            }
            ellipse.SDEx = Sqrt(2) * Sqrt(up_Ex / n);
            ellipse.SDEy = Sqrt(2) * Sqrt(up_Ey / n);
        }

        public double Get_Distance(Point A,Point B)
        {
            double dx = B.x - A.x;
            double dy = B.y - A.y;
            return Sqrt(dx * dx + dy * dy);
        }

        public void Let_W(List<Point>points, ref double[,] W)
        {
            int n = points.Count;
            W = new double[n,n];

            for(int i=0;i<n;i++)//反距离权重
            {
                for(int j=0;j<n;j++)
                {
                    if (i == j)
                    {
                        W[i, j] = 0;
                    }
                    else
                    {
                        double d = Get_Distance(points[i], points[j]);
                        W[i, j] = 1.0 / Pow(d,2);//1/(d^2)
                    }
                }
            }

            //双重标准化
           
        

            for (int i = 0; i < n; i++)
            {
                double sumW = 0;
                for (int j = 0; j < n; j++)
                {
                    sumW += W[i,j];
                }

                for (int j = 0; j < n; j++)
                {
                    W[i, j] = W[i, j] / sumW;
                }
            }
            
        }

        public void Let_I_Z(List<Point>p,double[,]W,Datacenter datacenter)//计算莫兰指数和Z得分
        {
            double S0 = 0;
            int n = p.Count;
            double avgV = p.Average(o => o.V);

            double down_I_global = p.Sum(o => Pow(o.V - avgV,2));//计算全局莫兰指数
            double up_I_global =0;
            for (int i=0;i<n;i++)
            {
                for(int j=0;j<n;j++)
                {
                    S0 += W[i, j];
                    up_I_global += W[i, j] * (p[i].V - avgV) * (p[j].V - avgV);
                }
            }
            datacenter.I_global = (n / S0)*(up_I_global/down_I_global);

          
            for (int i = 0; i < n; i++)//计算局部莫兰指数
            {
                double right_I_local=0;
                double up_Si2 = 0;
                for (int j = 0; j < n; j++)
                {
                    if (i == j) { continue; }
                    else
                    {
                        right_I_local += W[i, j] * (p[j].V - avgV);
                        up_Si2 += Pow(p[j].V - avgV, 2);
                    }
                }
                double Si2 = up_Si2 / (n - 1);
                double left_I_local = (p[i].V - avgV) / Si2;
                p[i].I_local = left_I_local * right_I_local;
            }

            double Z_down = p.Sum(o => Pow(o.V - avgV, 2));//计算Z得分,这里由于求P需要查Z表,没有直接的公式,所以没有计算P值
            Z_down = Sqrt(Z_down / n);
            foreach(var P in p)
            {
                double Z_up = P.V - avgV;
                P.Z = Z_up / Z_down;
            }
        }   



    }
}
