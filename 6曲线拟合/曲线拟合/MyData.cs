using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 曲线拟合
{
    public class MyData
    {
        public string inFile = "";
        public string outFile = "";

        public List<MyPoint> PList = new List<MyPoint>();//点列表
        public bool IsClose;//是否闭合拟合 
        public List<MyPoint> PList2;

        /// <summary>
        /// 曲线拟合
        /// </summary>
        public void Fit()
        {
            int n = PList.Count - 1;//原始数据最后一个点索引

            if (IsClose)
            {
                //进行补点
                PList2 = new List<MyPoint> { PList[n - 1], PList[n] };
                PList2.AddRange(PList);
                PList2.Add(PList[0]);
                PList2.Add(PList[1]);
            }
            else
            {
                //进行补点
                MyPoint pa = this.AddP(PList[0], PList[1], PList[2], "A");
                MyPoint pb = this.AddP(pa, PList[0], PList[1], "B");
                MyPoint pc = this.AddP(PList[n], PList[n - 1], PList[n - 2], "C");
                MyPoint pd = this.AddP(pc, PList[n], PList[n - 1], "D");

                PList2 = new List<MyPoint> { pb, pa };      //注意先B点后A点
                PList2.AddRange(PList);
                PList2.Add(pc);
                PList2.Add(pd);
            }

            //对于闭合和不闭合的情况,都可以计算所有能计算的梯度、拟合参数,只不过展示时不一样
            for (int i = 2; i < this.PList2.Count - 2; i++)
            {
                PList2[i].CalGrid(PList2[i - 2], PList2[i - 1], PList2[i + 1], PList2[i + 2]);//注意是从-2,-1,1,2这个顺序
            }
            for (int i = 2; i < this.PList2.Count - 2; i++)
            {
                PList2[i].CalEF(PList2[i + 1]);//涉及到i+1,所以要先计算完所有的点的梯度,才方便计算曲线参数
            }
        }

        /// <summary>
        /// 增量补点(由近到远的三个点)
        /// </summary>
        public MyPoint AddP(MyPoint p1, MyPoint p2, MyPoint p3, string ID)
        {
            double x = p3.x - 3 * p2.x + 3 * p1.x;
            double y = p3.y - 3 * p2.y + 3 * p1.y;
            return new MyPoint(ID, x, y);
        }
    }
}
