using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 曲线拟合
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region 自定义函数
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
            myData = new MyData();
            this.toolStripStatusLabel_LStatus.Text = "欢迎使用本程序!";
            this.dataGridView_Data.Rows.Clear();
            this.richTextBox_Rep.Text = "";
        }
        #endregion

        #region 成员变量
        public MyData myData = new MyData();
        #endregion

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (myData.inFile != "" && MessageBox.Show("是否放弃当前页面内容", "提示",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { return; }
                this.ClearAll();
                if (MyFile.OpenFile(myData))
                {
                    //将数据展示到界面
                    for (int i = 0; i < myData.PList.Count; i++)
                    {
                        int ind = dataGridView_Data.Rows.Add();
                        dataGridView_Data.Rows[ind].Cells[0].Value = myData.PList[i].ID;
                        dataGridView_Data.Rows[ind].Cells[1].Value = myData.PList[i].x;
                        dataGridView_Data.Rows[ind].Cells[2].Value = myData.PList[i].y;
                    }
                    PrintInfo("文件读取成功 - " + myData.inFile);
                }
            }
            catch
            {
                PrintError("文件读取失败 - " + myData.inFile);
            }

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyFile.SaveFile(myData, this.richTextBox_Rep.Text))
                {
                    PrintInfo("保存成功! - " + myData.outFile);
                }
            }
            catch
            {
                PrintError("保存失败! - " + myData.outFile);
            }
        }

        private void toolStripButtonFitClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.myData.IsClose = true;
                myData.Fit();
                //将数据展示到界面
                string str = "拟合方式：闭合拟合\n";
                str += "起点ID\t起点x\t起点y\t终点ID\t终点x\t终点y\tE0\tE1\tE2\tE3\tF0\tF1\tF2\tF3\n";
                for (int i = 2; i < myData.PList2.Count - 2; i++)
                {
                    MyPoint sp = myData.PList2[i];
                    MyPoint ep = myData.PList2[i + 1];
                    str += $"{sp.ID}\t{sp.x:f3}\t{sp.y:f3}\t{ep.ID}\t{ep.x:f3}\t{ep.y:f3}\t{sp.E0:f3}\t{sp.E1:f3}\t{sp.E2:f3}\t{sp.E3:f3}\t{sp.F0:f3}\t{sp.F1:f3}\t{sp.F2:f3}\t{sp.F3:f3}\n";
                }
                this.richTextBox_Rep.Text = str;
                PrintInfo("闭合拟合计算成功!");
            }
            catch (Exception)
            {
                PrintError("闭合拟合计算失败!");
            }
        }

        private void toolStripButton_FitNoClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.myData.IsClose = false;
                myData.Fit();
                //将数据展示到界面
                string str = "拟合方式：不闭合拟合\n";
                str += "起点ID\t起点x\t起点y\t终点ID\t终点x\t终点y\tE0\tE1\tE2\tE3\tF0\tF1\tF2\tF3\n";
                for (int i = 2; i < myData.PList2.Count - 3; i++) //不闭合拟合会少一个点
                {
                    MyPoint sp = myData.PList2[i];
                    MyPoint ep = myData.PList2[i + 1];
                    str += $"{sp.ID}\t{sp.x:f3}\t{sp.y:f3}\t{ep.ID}\t{ep.x:f3}\t{ep.y:f3}\t{sp.E0:f3}\t{sp.E1:f3}\t{sp.E2:f3}\t{sp.E3:f3}\t{sp.F0:f3}\t{sp.F1:f3}\t{sp.F2:f3}\t{sp.F3:f3}\n";
                }
                this.richTextBox_Rep.Text = str;
                PrintInfo("不闭合拟合计算成功!");
            }
            catch (Exception)
            {
                PrintError("不闭合拟合计算失败!");
            }
        }

        private void toolStripButton_About_Click(object sender, EventArgs e)
        {
            PrintInfo("2024测绘程序设计大赛\n曲线拟合");
        }

        private void toolStripButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 菜单栏按钮

        #endregion
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Open_Click(sender, e);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_Save_Click(sender, e);
        }

        private void 闭合拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonFitClose_Click(sender, e);
        }

        private void 不闭合拟合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_FitNoClose_Click(sender, e);
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
