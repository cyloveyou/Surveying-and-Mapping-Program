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
    public class MyStation
    {
        //测站地心坐标
        public double Xp;
        public double Yp;
        public double Zp;

        //测站大地坐标
        public double Bp;
        public double Lp;

        //测站所观测的卫星
        public List<MySatellite> Sats = new List<MySatellite>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Xp">测站地心坐标,米</param>
        /// <param name="Yp">测站地心坐标,米</param>
        /// <param name="Zp">测站地心坐标,米</param>
        /// <param name="Bp">测站大地坐标,弧度</param>
        /// <param name="Lp">测站大地坐标,弧度</param>
        public MyStation(double Xp, double Yp, double Zp, double Bp, double Lp)
        {
            this.Xp = Xp;
            this.Yp = Yp;
            this.Zp = Zp;
            this.Bp = Bp;
            this.Lp = Lp;
        }

        /// <summary>
        /// 计算所有卫星的高度角,电离层延迟
        /// </summary>
        public void CalSatellite()
        {
            foreach (var item in Sats)
            {
                item.CalELAZ(this);
                item.IonoDelay(this);
            }
        }
    }
}
