using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSS对流层
{
    public class MyStation
    {
        /// <summary>
        /// 界面数据
        /// </summary>
        public string Name { get; set; }
        public int Date { get; set; }

        public double L_deg { get; set; }//经度,NEIL模型似乎与对流层没有关系
        public double B_deg { get; set; }//纬度
        public double H { get; set; }
        public double E_deg { get; set; }//高度角,为十进制度

        public double se;//sinE,后面会经常用到

        /// <summary>
        /// 时间系统
        /// </summary>
        public DateTime DT;//DT.DayOfYear为年积日
        public int Doy0;//测站年积日
        public int Doy;//年积日
        public int year;
        public int month;
        public int day;

        /// <summary>
        /// 干 湿分量
        /// </summary>
        public double md;
        public double mw;
        public double Trop;//对流层延迟
        public double ZHD, ZWD;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Date"></param>
        /// <param name="L_deg">经度,单位十进制度</param>
        /// <param name="B_deg">纬度,单位十进制度</param>
        /// <param name="H"></param>
        /// <param name="E_deg">高度角,单位十进制度</param>
        public MyStation(string Name, string Date,
            string L_deg, string B_deg, string H, string E_deg)
        {
            this.Name = Name;
            this.Date = int.Parse(Date);
            this.L_deg = double.Parse(L_deg);
            this.B_deg = double.Parse(B_deg);
            this.H = double.Parse(H);
            this.E_deg = double.Parse(E_deg);
            this.se = Math.Sin(this.E_deg / 180 * Math.PI);//计算sinE,记得转弧度

            this.year = int.Parse(Date.Substring(0, 4)); //包括0号索引向后取四位
            this.month = int.Parse(Date.Substring(4, 2));
            this.day = int.Parse(Date.Substring(6, 2));

            this.DT = new DateTime(year, month, day);
            this.Doy = DT.DayOfYear;
        }

        /// <summary>
        /// 湿分量投影函数
        /// </summary>
        /// <param name="aw"></param>
        /// <param name="bw"></param>
        /// <param name="cw"></param>
        /// <returns></returns>
        public double FunMW(double aw, double bw, double cw)
        {
            //每个除号后面必须加括号
            double up = 1 + aw / (1 + bw / (1 + cw));
            //下边可以拷贝然后部分的1改成se
            double down = se + aw / (se + bw / (se + cw));
            return up / down;
        }

        /// <summary>
        /// 干分量投影函数
        /// </summary>
        /// <param name="ad"></param>
        /// <param name="bd"></param>
        /// <param name="cd"></param>
        /// <param name="aht"></param>
        /// <param name="bht"></param>
        /// <param name="cht"></param>
        /// <returns></returns>
        public double FunMD(double ad, double bd, double cd, double aht, double bht, double cht)
        {
            double up1 = 1 + ad / (1 + bd / (1 + cd));
            double down1 = se + ad / (se + bd / (se + cd));

            double up2 = 1 + aht / (1 + bht / (1 + cht));
            double down2 = se + aht / (se + bht / (se + cht));

            return up1 / down1 + (1 / se - up2 / down2) * H / 1000;//H的单位是米
        }

        /// <summary>
        /// 线性内插,可以直接插值出湿分量系数
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <param name="B"></param>
        /// <param name="n"></param>
        public double LinearInterp(double a1, double a2,
            double B, double B1, double B2)
        {
            return a1 + (a2 - a1) * (B - B1) / (B2 - B1);
        }

        /// <summary>
        /// 内插干分量系数
        /// </summary>
        /// <param name="avg1"></param>
        /// <param name="avg2"></param>
        /// <param name="amp1"></param>
        /// <param name="amp2"></param>
        /// <param name="B"></param>
        /// <param name="B1"></param>
        /// <param name="B2"></param>
        /// <param name="t0">参考时刻的年积日</param>
        /// <param name="t">年积日</param>
        /// <returns></returns>
        public double InteD(double avg1, double avg2, double amp1, double amp2,
            double B, double B1, double B2, double t0, double t)
        {
            double temp1 = LinearInterp(avg1, avg2, B, B1, B2);
            double temp2 = LinearInterp(amp1, amp2, B, B1, B2);
            return (temp1) + temp2 * Math.Cos(2 * Math.PI * (t - t0) / 365.25);
        }

        /// <summary>
        /// 计算本站点的对流层延迟校正
        /// </summary>
        public void TropDelay()
        {
            this.Doy0 = 28;

            #region 系数表
            List<double> aw = new List<double>()
        {
           0.00058021897,
           0.00056794847,
           0.00058118019,
           0.00059727542,
           0.00061641693,
        };
            List<double> bw = new List<double>()
        {
            0.0014275268,
            0.0015138625,
            0.0014572752,
            0.0015007428,
            0.0017599082,
        };
            List<double> cw = new List<double>()
        {
            0.043472961,
            0.046729510,
            0.043908931,
            0.044626982,
            0.054736038
        };

            List<double> ah1 = new List<double>()
        {
            0.0012769934,
            0.0012683230,
            0.0012465397,
            0.0012196049,
            0.0012045996
        };
            List<double> bh1 = new List<double>() {
            0.0029153695,
            0.0029152299,
            0.0029288445,
            0.0029022565,
            0.0029024912
        };
            List<double> ch1 = new List<double>() {
            0.062610505,
            0.062837393,
            0.063721774,
            0.063824265,
            0.064258455
        };

            List<double> ah2 = new List<double>()
        {
            0.0,
            0.000012709626,
            0.000026523662,
            0.000034000452,
            0.000041202191
        };
            List<double> bh2 = new List<double>()
        {
            0.0,
            0.000021414979,
            0.000030160779,
            0.000072562722,
            0.000117233750
        };
            List<double> ch2 = new List<double>()
        {
            0.0,
            0.000090128400,
            0.000043497037,
            0.000847953480,
            0.001703720600
        };
            #endregion

            //根据纬度进行插值,计算干湿分量系数,注意纬度转绝对值!!!
            int n = (int)(Math.Abs(this.B_deg) / 15);

            //定义干 湿分量系数
            double a_w, b_w, c_w;
            double a_d, b_d, c_d;
            //根据n的值来确定,n==0,意味着B小于15,n==5意味着B在75-90之间
            if (n == 0)
            {
                a_w = aw[0];
                b_w = bw[0];
                c_w = cw[0];

                a_d = ah1[0] + ah1[0] * Math.Cos(2 * Math.PI * (Doy - Doy0) / 365.25);
                b_d = bh1[0] + bh1[0] * Math.Cos(2 * Math.PI * (Doy - Doy0) / 365.25);
                c_d = ch1[0] + ch1[0] * Math.Cos(2 * Math.PI * (Doy - Doy0) / 365.25);
            }
            else if (n == 5)
            {
                a_w = aw[4];
                b_w = bw[4];
                c_w = cw[4];

                a_d = ah1[4] + ah1[4] * Math.Cos(2 * Math.PI * (Doy - Doy0) / 365.25);
                b_d = bh1[4] + bh1[4] * Math.Cos(2 * Math.PI * (Doy - Doy0) / 365.25);
                c_d = ch1[4] + ch1[4] * Math.Cos(2 * Math.PI * (Doy - Doy0) / 365.25);
            }
            else
            {
                a_w = LinearInterp(aw[n - 1], aw[n], B_deg, n * 15, (n + 1) * 15);
                b_w = LinearInterp(bw[n - 1], bw[n], B_deg, n * 15, (n + 1) * 15);
                c_w = LinearInterp(cw[n - 1], cw[n], B_deg, n * 15, (n + 1) * 15);

                a_d = InteD(ah1[n - 1], ah1[n], ah2[n - 1], ah2[n], B_deg, n * 15, (n + 1) * 15, Doy0, Doy);
                b_d = InteD(bh1[n - 1], bh1[n], bh2[n - 1], bh2[n], B_deg, n * 15, (n + 1) * 15, Doy0, Doy);
                c_d = InteD(ch1[n - 1], ch1[n], ch2[n - 1], ch2[n], B_deg, n * 15, (n + 1) * 15, Doy0, Doy);
            }

            //根据映射函数计算MW,MD
            double aht = 2.53e-5;
            double bht = 5.49e-3;
            double cht = 1.14e-3;
            this.md = this.FunMD(a_d, b_d, c_d, aht, bht, cht);
            this.mw = this.FunMW(a_w, b_w, c_w);

            //计算对流层延迟
            this.ZHD = 2.2995 * Math.Pow(Math.E, -0.000116 * H);
            this.ZWD = 0.1;

            this.Trop = ZHD * md * ZWD * mw;
        }
    }
}
