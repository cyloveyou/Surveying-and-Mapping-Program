using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 空间数据分析3_David
{
    public partial class Form1 : Form
    {
        FileHelper fileHelper = new FileHelper();
        Datacenter datacenter = new Datacenter();
        Algorithm algorithm = new Algorithm();
        Draw draw = new Draw();
        string report = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void O_tool_Click(object sender, EventArgs e)
        {
            try
            {
                fileHelper.Open_File(datacenter);
            }
            catch(Exception)
            {
                MessageBox.Show("打开文件失败");
                throw;
            }
            L_label.Text = "已导入";

            dataGridView1.RowCount = datacenter.all_points.Count;
            int i = 0;
            foreach(var p in datacenter.all_points)
            {
                dataGridView1[0, i].Value = p.name;
                dataGridView1[1, i].Value = p.B;
                dataGridView1[2, i].Value = p.L;
                i++;
            }
            
        }

        private void C_tool_Click(object sender, EventArgs e)
        {
            if(L_label.Text=="未导入")
            {
                MessageBox.Show("请先导入数据");
                return;
            }
            C_label.Text = "已计算";

            algorithm.Let_Ellipse(datacenter.ellipse, datacenter.all_points);
            algorithm.Let_W(datacenter.all_points, ref datacenter.W);
            algorithm.Let_I_Z(datacenter.all_points, datacenter.W, datacenter);
            report += "--------------------------------结果报告--------------------------------\n";
            report += "---------椭圆参数---------\n";
            report += "X0:" + datacenter.ellipse.X0.ToString("F3")+"  Y0:" + datacenter.ellipse.Y0.ToString("F3") + "\n";
            report += "SDEx:" + datacenter.ellipse.SDEx.ToString("F3")+ "  SDEy:" + datacenter.ellipse.SDEy.ToString("F3") + "\n";
            report+= "全局莫兰指数:"+datacenter.I_global.ToString("F8")+"\n";
            report += "---------------------各点信息---------------------\n";
            report += "点名\t观测值\tZ得分\t局部I\t\n";
            foreach(var p in datacenter.all_points)
            {
                report += p.name + "\t" + p.V.ToString("F3") + "\t" + p.Z.ToString("F3") + "\t" + p.I_local.ToString("F3") + "\t\n";
            }
            richTextBox1.Text = report;
            draw.Draw_Ellipse(datacenter.ellipse, datacenter.all_points, chart1,datacenter);
            draw.Draw_I(datacenter,chart2);

            int n = datacenter.W.GetLength(0);
            for (int i=0;i<n;i++)
            {
                dataGridView2.Columns.Add("第" + (i + 1) + "列", "第" + (i + 1) + "列");
            }
            dataGridView2.RowCount = n;
            for(int i=0;i<n;i++)
            {
                for(int j=0;j<n;j++)
                {
                    dataGridView2[j, i].Value = datacenter.W[i, j].ToString("F3");
                }
            }

        }

     
        private void FangDa1_Click(object sender, EventArgs e)
        {
            double dx = datacenter.dertaX/10;
            double dy = datacenter.dertaY/10;
            chart1.ChartAreas[0].AxisX.Maximum += dx;
            chart1.ChartAreas[0].AxisY.Maximum += dy;
        }

        private void SuoXiao1_Click(object sender, EventArgs e)
        {
            double dx = datacenter.dertaX / 10;
            double dy = datacenter.dertaY / 10;
            chart1.ChartAreas[0].AxisX.Maximum -= dx;
            chart1.ChartAreas[0].AxisY.Maximum -= dy;
        }

        private void left_Click(object sender, EventArgs e)
        {
            double dx = datacenter.I_dertaX / 10;
            chart2.ChartAreas[0].AxisX.Maximum -= dx;
        }

        private void right_Click(object sender, EventArgs e)
        {
            double dx = datacenter.I_dertaX / 10;
            chart2.ChartAreas[0].AxisX.Maximum += dx;
        }

        private void R_tool_Click(object sender, EventArgs e)
        {
            if(C_label.Text=="未计算")
            {
                MessageBox.Show("请先计算数据");
                return;
            }
            fileHelper.Save_Report(report);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (C_label.Text == "未计算")
            {
                MessageBox.Show("请先计算数据");
                return;
            }
            fileHelper.Save_Image(chart1);
        }

        private void E_tool_Click(object sender, EventArgs e)
        {
            fileHelper.Save_Image(chart1);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            C_tool_Click(sender, e);
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            O_tool_Click(sender, e);
        }

        private void 保持报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
               R_tool_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            I_tool_Click(sender, e);
        }

        private void I_tool_Click(object sender, EventArgs e)
        {

            if (C_label.Text == "未计算")
            {
                MessageBox.Show("请先计算数据");
                return;
            }
            fileHelper.Save_Image(chart2);
        }

        private void 保存椭圆示意图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            E_tool_Click(sender, e);
        }
    }
}
