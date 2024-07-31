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
    public class MyTime
    {
        #region 成员变量
        public double year;
        public double month;
        public double day;
        public double hour;
        public double min;
        public double sec;

        public double Doy;
        #endregion

        #region 构造函数

        public MyTime()
        {

        }

        public MyTime(string year, string month, string day, string hour, string min, string sec)
        {
            this.year = double.Parse(year);
            this.month = double.Parse(month);
            this.day = double.Parse(day);
            this.hour = double.Parse(hour);
            this.min = double.Parse(min);
            this.sec = double.Parse(sec);
        }
        #endregion
    }
}
