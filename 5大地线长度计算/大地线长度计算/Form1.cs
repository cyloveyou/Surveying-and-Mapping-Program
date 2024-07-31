using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 大地线长度计算
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
            this.toolStripStatusLabel_Lstatus.Text = str;
            MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void PrintError(string str)
        {
            this.toolStripStatusLabel_Lstatus.Text = str;
            MessageBox.Show(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 清出界面所有数据
        /// </summary>
        public void ClearAll()
        {
            this.dataGridView_Data.Rows.Clear();
            this.toolStripStatusLabel_Lstatus.Text = "欢迎使用本程序!";
            this.richTextBox_Rep.Text = "";
            myData = new MyData();
        }
        #endregion

        public MyData myData = new MyData();

        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            if (myData.inFile != "" && MessageBox.Show("是否放弃界面所有数据?", "提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            { return; }
            this.ClearAll();
            if (MyFile.OpenFile(myData))
            {
                foreach (var item in myData.myLine)
                {
                    int ind = this.dataGridView_Data.Rows.Add();
                    dataGridView_Data.Rows[ind].Cells[0].Value = item.p1.Name;
                    dataGridView_Data.Rows[ind].Cells[1].Value = item.p1.B_dms;
                    dataGridView_Data.Rows[ind].Cells[2].Value = item.p1.L_dms;

                    dataGridView_Data.Rows[ind].Cells[3].Value = item.p2.Name;
                    dataGridView_Data.Rows[ind].Cells[4].Value = item.p2.B_dms;
                    dataGridView_Data.Rows[ind].Cells[5].Value = item.p2.L_dms;
                }

                PrintInfo("文件读取成功 - " + myData.inFile);
            }
        }

        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyFile.SaveFile(myData, this.richTextBox_Rep.Text))
                {
                    PrintInfo("保存成功 - " + myData.outFile);
                }
            }
            catch (Exception)
            {
                PrintError("保存失败 - " + myData.outFile);
            }
        }

        private void toolStripButton_Cal_Click(object sender, EventArgs e)
        {
            foreach (var item in myData.myLine)
            {
                item.CalS(myData.myE);
            }
            for (int i = 0; i < myData.myLine.Count; i++)
            {
                dataGridView_Data.Rows[i].Cells[0].Value = myData.myLine[i].p1.Name;
                dataGridView_Data.Rows[i].Cells[1].Value = myData.myLine[i].p1.B_dms;
                dataGridView_Data.Rows[i].Cells[2].Value = myData.myLine[i].p1.L_dms;

                dataGridView_Data.Rows[i].Cells[3].Value = myData.myLine[i].p2.Name;
                dataGridView_Data.Rows[i].Cells[4].Value = myData.myLine[i].p2.B_dms;
                dataGridView_Data.Rows[i].Cells[5].Value = myData.myLine[i].p2.L_dms;
                dataGridView_Data.Rows[i].Cells[6].Value = myData.myLine[i].S;
            }
            StringBuilder str = new StringBuilder();
            str.AppendLine($"序号, 说明, 输出格式要求");

            str.AppendLine("1, 椭球长半轴a, {myData.myE.a:f0}");
            str.AppendLine("2, 扁率倒数1/f, {myData.myE.ff:f3}");
            str.AppendLine("3, 扁率f, {myData.myE.f:f8}");
            str.AppendLine("4, 椭球短半轴b, {myData.myE.b:f3}");
            str.AppendLine("5, 第一偏心率平方e2, {myData.myE.b:f3}");
            str.AppendLine("6, 第二偏心率平方e’² {myData.myE.b:f3}");
            str.AppendLine("7, 第1条大地线u1, {myData.myLine[0].u2:f8}");
            str.AppendLine("8, 第1条大地线u2, {myData.myLine[0].u1:f8}");
            str.AppendLine("9, 第1条大地线经差l（弧度）, {myData.myLine[0].l:f8}");
            str.AppendLine("10, 第1条大地线a1, {myData.myLine[0].a1:f8}");
            str.AppendLine("11, 第1条大地线a2, {myData.myLine[0].a2:f8}");
            str.AppendLine("12, 第1条大地线b1, {myData.myLine[0].b1:f8}");
            str.AppendLine("13, 第1条大地线b2, {myData.myLine[0].b2:f8}");
            str.AppendLine("14, 第1条大地线系数α, {myData.myLine[0].alpha:f8}");
            str.AppendLine("15, 第1条大地线系数β, {myData.myLine[0].beta:f8}");
            str.AppendLine("16, 第1条大地线系数γ, {myData.myLine[0].gamma:f8}");
            str.AppendLine("17, 第1条大地线A1（弧度）, {myData.myLine[0].A1:f8}");
            str.AppendLine("18, 第1条大地线λ, {myData.myLine[0].lambda:f8}");
            str.AppendLine("19, 第1条大地线σ, {myData.myLine[0].sigma:f8}");
            str.AppendLine("20, 第1条大地线sinA0, {myData.myLine[0].sinA0:f8}");
            str.AppendLine("21, 第1条大地线系数A, {myData.myLine[0].A:f8}");
            str.AppendLine("22, 第1条大地线系数B, {myData.myLine[0].B:f8}");
            str.AppendLine("23, 第1条大地线系数C, {myData.myLine[0].C:f8}");
            str.AppendLine("24, 第1条大地线σ1, {myData.myLine[0].sigma1:f8}");
            str.AppendLine("25, 第1条大地线长S, {myData.myLine[0].S:f3}");
            str.AppendLine("26, 第2条大地线长S2, {myData.myLine[1].S:f3}");
            str.AppendLine("27, 第3条大地线长S3, {myData.myLine[2].S:f3}");
            str.AppendLine("28, 第4条大地线长S4, {myData.myLine[3].S:f3}");
            str.AppendLine("29, 第5条大地线长S5, {myData.myLine[4].S:f3}");
            this.richTextBox_Rep.Text = str.ToString();
            PrintInfo("计算成功!");
        }

        private void toolStripButton_About_Click(object sender, EventArgs e)
        {
            PrintInfo("2024测绘程序设计\n大地线长度计算");
        }

        private void toolStripButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出?", "提示",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }

        }

        private void 打开数据文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Path.GetDirectoryName(myData.inFile));
            }
            catch (Exception)
            {
                PrintError("打开文件夹失败！");
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
                PrintError("打开文件夹失败！");
            }
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
