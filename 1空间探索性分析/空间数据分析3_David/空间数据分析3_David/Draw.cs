using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Math;

namespace 空间数据分析3_David
{
    class Draw
    {
        public void Draw_Ellipse(Ellipse ellipse,List<Point>points,Chart chart,Datacenter datacenter)
        {
            chart.Annotations.Clear();
            chart.ChartAreas.Clear();
            chart.Series.Clear();

            Series P = new Series();
            P.Name = "空间上的点";
            P.ChartType = SeriesChartType.Point;
            P.MarkerSize = 4;
            P.MarkerColor = Color.Gray;
            P.MarkerStyle = MarkerStyle.Circle;
            foreach(var p in points)
            {
                DataPoint dataPoint = new DataPoint( p.y,p.x);
                P.Points.Add(dataPoint);
            }
            chart.Series.Add(P);

            Series ell = new Series();
            ell.Color = Color.Blue;
            ell.Name = "标准椭圆曲线";
            ell.ChartType = SeriesChartType.Line;
            double[] x = new double[1000];
            double[] y = new double[1000];
            double[] row_x = new double[1000]; 
            double[] row_y = new double[1000];
            double X0 = ellipse.X0;
            double Y0 = ellipse.Y0;
            double cos_theta = ellipse.cos_theta;
            double sin_theta = ellipse.sin_theta;
            double SDEx = ellipse.SDEx;
            double SDEy = ellipse.SDEy;
            int num = 0;
            for (double t = 0; t <= 2*PI;t+=0.01)
            {
                x[num] = SDEx * Cos(t);
                y[num] = SDEy * Sin(t);

                row_x[num] = x[num] * cos_theta - y[num] * sin_theta+X0;
                row_y[num] = x[num] * sin_theta + y[num] * cos_theta+Y0;
                DataPoint dataPoint = new DataPoint( row_y[num],row_x[num]);
                ell.Points.Add(dataPoint);

                num++;
            }
            chart.Series.Add(ell);

            double maxX = points.Max(o => o.x);
            double minX = points.Min(o => o.x);
            double maxY = points.Max(o => o.y);
            double minY = points.Min(o => o.y);
            double dertaX = maxX - minX;
            double dertaY = maxY - minY;
            datacenter.dertaX = dertaX;
            datacenter.dertaY = dertaY;

            ChartArea area = new ChartArea();
            area.AxisX.Maximum = maxY + dertaY/10;
            area.AxisX.Minimum = minY - dertaY/10;
            area.AxisY.Maximum = maxX + dertaX/10;
            area.AxisY.Minimum = minX - dertaX/10;
            chart.ChartAreas.Add(area);
            
            chart.DataBind();
        }

        public void Draw_I(Datacenter datacenter,Chart chart)
        {
            chart.Annotations.Clear();
            chart.ChartAreas.Clear();
            chart.Series.Clear();

            Series I_global = new Series();
            I_global.Name = "全局莫兰指数";
            I_global.ChartType = SeriesChartType.Line;
            I_global.Color = Color.Red;

            Series I_local = new Series();
            I_local.Name = "局部莫兰指数";
            I_local.ChartType = SeriesChartType.Line;
            I_local.Color = Color.Blue;
            int num = 0;

            double[] I_l = new double[1000];
            double[] I_g = new double[1000];
            foreach(var p in datacenter.all_points)
            {
                I_g[num] = datacenter.I_global;
                I_l[num] = p.I_local;
                DataPoint point_Ig = new DataPoint(num, I_g[num]);
                DataPoint point_Il = new DataPoint(num, I_l[num]);
                I_local.Points.Add(point_Il);
                I_global.Points.Add(point_Ig);
                num++;
            }
            chart.Series.Add(I_local);
            chart.Series.Add(I_global);

         
            double maxX = num;
            double minX = 0;
            double maxY = I_l.Max();
            double minY = I_l.Min();
            double dertaX = maxX - minX;
            double dertaY = maxY - minY;
            datacenter.I_dertaX = dertaX;
            datacenter.I_dertaY = dertaY;
            ChartArea area = new ChartArea();
            area.AxisX.Maximum = maxX - dertaX / 4;
            area.AxisX.Minimum = minX + dertaX / 4;
            area.AxisY.Maximum = maxY - dertaY / 8;
            area.AxisY.Minimum = minY + dertaY / 8;
            chart.ChartAreas.Add(area);

            chart.DataBind();

        }
    }
}
