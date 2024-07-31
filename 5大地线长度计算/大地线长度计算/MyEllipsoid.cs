using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 大地线长度计算
{
    public class MyEllipsoid
    {
        /// <summary>
        /// 椭球参数
        /// </summary>
        public double a;        //长半轴
        public double b;        //短半轴
        public double ff;       //扁率倒数
        public double f;        //变扁率
        public double e12;      //第一偏心率
        public double e22;      //第二偏心率

        /// <summary>
        /// 从字符串构造椭球
        /// </summary>
        /// <param name="a"></param>
        /// <param name="ff"></param>
        public MyEllipsoid(string a, string ff)
        {
            this.a = double.Parse(a);
            this.ff = double.Parse(ff);

            this.CalParm();//构造椭球时计算椭球参数
        }

        /// <summary>
        /// 计算椭球参数
        /// </summary>
        private void CalParm()
        {
            f = 1 / ff;
            b = a * (1 - f);
            e12 = (a * a - b * b) / a / a;
            e22 = (a * a - b * b) / b / b;
        }
    }
}
