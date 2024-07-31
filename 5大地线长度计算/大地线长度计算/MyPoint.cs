using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大地线长度计算
{
    public class MyPoint
    {
        public string Name;
        public double B_dms;
        public double L_dms;
        public double B;//纬度弧度制
        public double L;//经度弧度制

        public MyPoint(string Name, string B_dms, string L_dms)
        {
            this.Name = Name;
            this.B_dms = double.Parse(B_dms);
            this.L_dms = double.Parse(L_dms);

            this.B = DmsToRad(this.B_dms);
            this.L = DmsToRad(this.L_dms);
        }

        /// <summary>
        /// 度分秒转弧度运算
        /// </summary>
        /// <param name="dms"></param>
        /// <returns></returns>
        public double DmsToRad(double dms)
        {
            double angle = dms * 10000;             //先乘上10000
            int temp = (int)angle;                  //转为整数参与运算
            int d = temp / 10000;                   //整数除10000 int 得到度
            int m = (temp - d * 10000) / 100;       //
            double s = angle - d * 10000 - m * 100;

            double deg = d + m / 60.0 + s / 3600.0; //浮点数参加运算!!!
            return deg / 180 * Math.PI;
        }
    }
}
