using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 空间前方交会
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 辅助函数
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

        /// <summary>
        /// 清空界面数据
        /// </summary>
        public void ClearAll()
        {
            this.toolStripStatusLabel_LStatus.Text = "欢迎使用本程序!";//状态栏还原
            this.richTextBox_Rep.Text = "";             //报告清空
            this.dataGridView_Data.Rows.Clear();    //表格清空
            this.myData = new MyData(); //数据清空
        }
        #endregion

        #region 成员变量
        public MyData myData = new MyData();
        #endregion

        #region 菜单栏选项
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Open_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Save_Click(sender, e);
        }

        private void 打开数据文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(myData.inFile));
            }
            catch (Exception)
            {
                PrintError("打开文件夹失败!");
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
                PrintError("打开文件夹失败!");
            }
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Cal_Click(sender, e);
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintInfo("帮助文档待完善!");
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripButton_About_Click(sender, e);
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 工具栏按钮

        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            if (myData.inFile != "" && MessageBox.Show("是否放弃当前页面内容?", "提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            try
            {
                this.ClearAll();
                if (MyFile.OpenFile(myData))
                {
                    //将x1,y1,x2,y2展示到界面
                    for (int i = 0; i < myData.DP.Count; i++)
                    {
                        this.dataGridView_Data.Rows[i].Cells[0].Value = myData.DP[i].p1.x;
                        this.dataGridView_Data.Rows[i].Cells[1].Value = myData.DP[i].p1.y;
                        this.dataGridView_Data.Rows[i].Cells[2].Value = myData.DP[i].p2.x;
                        this.dataGridView_Data.Rows[i].Cells[3].Value = myData.DP[i].p2.y;
                    }
                    PrintInfo($"文件读取成功! - {myData.inFile}");
                }
            }
            catch (Exception)
            {
                PrintError($"文件读取失败! - {myData.inFile}");
            }
        }

        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyFile.SaveFile(myData, richTextBox_Rep.Text))
                {
                    PrintInfo($"文件保存成功! - {myData.outFile}");
                }
            }
            catch (Exception)
            {
                PrintError($"文件保存失败! - {myData.outFile}");
            }
        }

        private void toolStripButton_Cal_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < myData.DP.Count; i++)
                {
                    myData.DP[i].CalXYZ();
                    this.dataGridView_Data.Rows[i].Cells[4].Value = myData.DP[i].X;
                    this.dataGridView_Data.Rows[i].Cells[5].Value = myData.DP[i].Y;
                    this.dataGridView_Data.Rows[i].Cells[6].Value = myData.DP[i].Z;
                }
                PrintInfo("计算成功!");
            }
            catch (Exception)
            {
                PrintError("计算失败!");
            }
        }

        private void toolStripButton_About_Click(object sender, EventArgs e)
        {
            PrintInfo("2024测绘程序设计\n空间前方交会");
        }

        private void toolStripButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 其他事件
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出?", "提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        #endregion
    }
}
