using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS电离层改正
{
    public partial class Form1 : Form
    {
        public const double ToDeg = 180 / Math.PI;

        public Form1()
        {
            InitializeComponent();
        }
        #region 成员函数
        public void PrintInfo(string str)
        {
            this.toolStripStatusLabel_LStatus.Text = str;
            MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void PrintError(string str)
        {
            this.toolStripStatusLabel_LStatus.Text = str;
            MessageBox.Show(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void ClearAll()
        {
            this.richTextBox_Rep.Text = "";
            this.toolStripStatusLabel_LStatus.Text = "欢迎使用本程序!";
            this.myData = new MyData();

        }
        #endregion

        #region 成员变量
        public MyData myData = new MyData();
        #endregion

        #region 工具栏按钮
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (myData.inFile != "" && MessageBox.Show("是否放弃当前页面内容？", "提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                if (MyFile.OpenFile(myData))
                {
                    this.dataGridView_Data.DataSource = myData.Station.Sats;
                    PrintInfo("文件读取成功! - " + myData.inFile);
                }
            }
            catch (Exception)
            {
                PrintError("文件读取失败！ - " + myData.inFile);
            }

        }

        /// <summary>
        /// 保存报告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyFile.SaveFile(myData, this.richTextBox_Rep.Text))
                {
                    PrintInfo("文件保存成功! - " + myData.outFile);
                }
            }
            catch (Exception)
            {
                PrintError("文件保存失败! - " + myData.outFile);
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Cal_Click(object sender, EventArgs e)
        {
            try
            {
                myData.Station.CalSatellite();
                string str = "卫星号\t高度角\t方位角\tL1(m)\n";
                foreach (var item in myData.Station.Sats)
                {
                    str += $"{item.PRN}\t{item.EL * ToDeg:f3}\t{item.AZ * ToDeg:f3}\t{item.L1D:f3}\n";
                }
                this.richTextBox_Rep.Text = str;
                this.dataGridView_Data.Refresh();//刷新视图！！！
                PrintInfo("计算成功!");
            }
            catch (Exception)
            {
                PrintError("计算失败!");
            }
        }

        /// <summary>
        /// 关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_About_Click(object sender, EventArgs e)
        {
            PrintInfo("2024测绘程序设计大赛\nGNSS电离层改正");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 菜单栏按钮
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Open_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Save_Click(sender, e);
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Cal_Click(sender, e);
        }

        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PrintInfo("帮助文档带完善!");
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_About_Click(sender, e);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 打开数据文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(myData.inFile));
            }
            catch (Exception)
            {
                PrintError("打开失败！");
            }
        }

        private void 打开报告文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(myData.outFile));
            }
            catch (Exception)
            {
                PrintError("打开失败！");
            }
        }

        #endregion

        #region 其他事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出?", "提示", MessageBoxButtons.YesNo,
                MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        #endregion

    }
}
