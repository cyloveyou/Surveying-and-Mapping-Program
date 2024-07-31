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

namespace 纵横断面
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region 自定义函数
        /// <summary>
        /// 打印提示
        /// </summary>
        /// <param name="str"></param>
        public void PrintInfo(string str)
        {
            this.toolStripStatusLabel_LStatus.Text = str;
            MessageBox.Show(str, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 打印错误
        /// </summary>
        /// <param name="str"></param>
        public void PrintError(string str)
        {
            this.toolStripStatusLabel_LStatus.Text = str;
            MessageBox.Show(str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="folder"></param>
        public void OpenFolder(string folder)
        {
            try
            {
                System.Diagnostics.Process.Start(folder);
            }
            catch (Exception)
            {
                PrintError("打开文件夹失败！");
            }
        }

        public void Clear()
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
        private void toolStripButton_Open_Click(object sender, EventArgs e)
        {
            try
            {
                if (myData.inFile != "" && MessageBox.Show("是否放弃当前页面?", "提示",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                {
                    return;
                }
                this.Clear();
                if (MyFile.OpenFile(myData))
                {
                    this.dataGridView_Data.DataSource = myData.myV.obsP;
                    PrintInfo("文件读取成功! - " + myData.inFile);
                };
            }
            catch (Exception)
            {
                PrintError("文件读取失败! - " + myData.outFile);
            }
        }

        private void toolStripButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                MyFile.SaveFile(myData, richTextBox_Rep.Text);
                PrintInfo("保存成功! - " + myData.outFile);
            }
            catch (Exception)
            {
                PrintError("保存失败! - " + myData.outFile);
            }
        }

        private void toolStripButton_Cal_Click(object sender, EventArgs e)
        {
            try
            {
                //计算AB之间的坐标方位角
                double AZ_AB = myData.A.AZToOther(myData.B);

                //计算纵断面的长度
                myData.myV.CalD();
                //计算内插点坐标
                myData.myV.interPoint();
                //计算纵断面面积
                myData.myV.CalS();

                //计算横断面
                myData.myV.CalMyH();

                string rep = $"序号,说明,结果\n";
                rep += $"1,参考高程点,H0的高程值,{myData.myV.H0:f3}\n";
                rep += $"2,关键点,K0的高程值,{myData.myV.Kp[0].H:f3}\n";
                rep += $"3,关键点,K1的高程值,{myData.myV.Kp[1].H:f3}\n";
                rep += $"4,关键点,K2的高程值,{myData.myV.Kp[2].H:f3}\n";
                rep += $"5,测试点,AB,的坐标方位角,{AZ_AB:f5}\n";
                rep += $"6,A,的内插高程,h,{myData.A.InterPH(myData.myV.obsP):f3}\n";
                rep += $"7,B,的内插高程,h,{myData.B.InterPH(myData.myV.obsP):f3}\n";
                rep += $"8,以,A、B,为两个端点的梯形面积,S,{myData.A.CalS(myData.B, myData.myV.H0):f3}\n";
                rep += $"9,K0,到,K1,的平面距离,D0,{myData.myV.DList[0]:f3}\n";
                rep += $"10,K1,到,K2,的平面距离,D1,{myData.myV.DList[1]:f3}\n";
                rep += $"11,纵断面的平面总距离,D,{myData.myV.D:f3}\n";
                rep += $"12,方位角ɑ01,{myData.myV.alphaList[0]:f5}\n";
                rep += $"13,方位角ɑ12,{myData.myV.alphaList[1]:f5}\n";
                rep += $"14,第一条纵断面的内插点Z3的坐标X,{myData.myV.NewPList.Find(t => t.Name == "Z3").X:f3}\n";
                rep += $"15,第一条纵断面的内插点Z3的坐标Y,{myData.myV.NewPList.Find(t => t.Name == "Z3").Y:f3}\n";
                rep += $"16,第一条纵断面的内插点Z3的高程H,{myData.myV.NewPList.Find(t => t.Name == "Z3").H:f3}\n";
                rep += $"17,第二条纵断面的内插点Y3的坐标X,{myData.myV.NewPList.Find(t => t.Name == "Y3").X:f3}\n";
                rep += $"18,第二条纵断面的内插点Y3的坐标Y,{myData.myV.NewPList.Find(t => t.Name == "Y3").Y:f3}\n";
                rep += $"19,第二条纵断面的内插点Y3的高程H,{myData.myV.NewPList.Find(t => t.Name == "Y3").H:f3}\n";
                rep += $"20,第一条纵断面面积S1,{myData.myV.SList[0]:f3}\n";
                rep += $"21,第二条纵断面面积S2,{myData.myV.SList[1]:f3}\n";
                rep += $"22,纵断面总面积S,{myData.myV.S:f3}\n";
                rep += $"23,第一条横断面内插点Q3的坐标X,{myData.myV.myH_S[0].PList[2].X:f3}\n";
                rep += $"24,第一条横断面内插点Q3的坐标Y,{myData.myV.myH_S[0].PList[2].Y:f3}\n";
                rep += $"25,第一条横断面内插点Q3的高程H,{myData.myV.myH_S[0].PList[2].H:f3}\n";
                rep += $"26,第二条横断面内插点W3的坐标X,{myData.myV.myH_S[1].PList[2].X:f3}\n";
                rep += $"27,第二条横断面内插点W3的坐标Y,{myData.myV.myH_S[1].PList[2].Y:f3}\n";
                rep += $"28,第二条横断面内插点W3的高程H,{myData.myV.myH_S[1].PList[2].H:f3}\n";
                rep += $"29,第一条横断面的面积Srow1,{myData.myV.myH_S[0].S:f3}\n";
                rep += $"30,第一条横断面的面积Srow2,{myData.myV.myH_S[1].S:f3}\n";
                this.richTextBox_Rep.Text = rep;
                PrintInfo("计算成功!");
            }
            catch (Exception)
            {
                PrintError("计算失败!");
            }
        }

        private void toolStripButton_About_Click(object sender, EventArgs e)
        {
            PrintInfo("2024测绘程序设计大赛\n纵横断面计算");
        }

        private void toolStripButton_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
            OpenFolder(Path.GetDirectoryName(myData.inFile));
        }

        private void 打开报告文件夹ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFolder(Path.GetDirectoryName(myData.inFile));
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
