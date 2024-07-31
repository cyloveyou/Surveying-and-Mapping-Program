using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 空间数据分析3_David
{
    class Point
    {
        public string name;
        public double x;
        public double y;
        public double z;

        public double I_local;//局部莫兰指数
        public double Z;//Z得分
        public double P;//P值

        public double B;
        public double L;
        public double V;
        public Point() { }
        public Point(string name,double B,double L,double V)
        {
            this.V = V;
            this.name = name;
            this.B = B;
            this.L = L;
            
            //大地坐标转化
            double b= 6356752.31424517;// WGS84椭球体
            double a = 6378137.0;//长半轴

         
            double e2 = 0.0066943799901413165;

            double h = 0;
            double BRad = B * Math.PI / 180;
            double LRad = L * Math.PI / 180;
            // 辅助变量
            double sinB= Math.Sin(BRad);
            double cosB= Math.Cos(BRad);
            double N = a / Math.Sqrt(1 - e2 * sinB * sinB);
            //转换到空间直角坐标
            x = (N + h) * cosB * Math.Cos(LRad);
            y= (N + h) * cosB * Math.Sin(LRad);
            z = ((1 - e2) * N + h) * sinB;
        }
    }
}
