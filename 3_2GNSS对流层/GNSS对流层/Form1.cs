using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS对流层
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region 自定义函数
        /// <summary>
        /// 弹窗提示
        /// </summary>
        /// <param name="str"></param>
        public void PrintInfo(string str)
        {
            this.toolStripStatusLabel_LStatus.Text = str;
            MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 弹窗错误
        /// </summary>
        /// <param name="str"></param>
        public void PrintError(string str)
        {
            this.toolStripStatusLabel_LStatus.Text = str;
            MessageBox.Show(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        public void ClearAll()
        {
            this.toolStripStatusLabel_LStatus.Text = "欢迎使用本程序!";
            this.richTextBox_Rep.Text = "";
            this.myData = new MyData();
        }
        #endregion

        #region 成员变量
        public MyData myData = new MyData();

        #endregion

        #region 工具栏按钮
        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (myData.inFile != "" && MessageBox.Show("是否放弃当前页面?", "提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                this.ClearAll();
                if (MyFile.OpenFile(myData))
                {
                    this.dataGridView_Data.DataSource = myData.myStation;
                    PrintInfo($"文件读取成功!- {myData.inFile}");
                }
            }
            catch (Exception)
            {
                PrintError($"文件读取失败!- {myData.inFile}");
            }
        }

        private void toolStripButton_Cal_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> header = new List<string> { "测站名", "高度角", "ZHD", "md", "ZWD", "mw" };
                string str = string.Join("\t", header) + "\n";
                foreach (var item in myData.myStation)
                {
                    item.TropDelay();
                    string temp = $"{item.Name}\t{item.E_deg:f2}\t{item.ZHD:f3}\t{item.md:f3}\t{item.ZWD:f3}\t{item.mw:f3}\n";
                    str += temp;
                }
                richTextBox_Rep.Text = str;
                PrintInfo("计算成功!");
            }
            catch (Exception)
            {
                PrintError("计算失败!");
            }
        }

        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (MyFile.SaveFile(myData, richTextBox_Rep.Text))
                {
                    PrintInfo("保存成功! - " + myData.outFile);
                }
            }
            catch (Exception)
            {
                PrintError("保存失败! - " + myData.outFile);
            }
        }

        private void toolStripButton_About_Click(object sender, EventArgs e)
        {
            PrintInfo("2024测绘程序设计\n对流层改正");
        }

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

        #region 其他事件
        /// <summary>
        /// 退出二次确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 数据修改错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_Data_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            PrintError("数据格式不正确！");
        }
        #endregion


    }
}
