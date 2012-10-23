namespace Centipede
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ActionsVarsTabControl = new System.Windows.Forms.TabControl();
            this.ActionsTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ActionsList = new System.Windows.Forms.ListBox();
            this.AddActionTabs = new System.Windows.Forms.TabControl();
            this.UIActTab = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.FlowContActTab = new System.Windows.Forms.TabPage();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.ExcelActTab = new System.Windows.Forms.TabPage();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.MathCadActTab = new System.Windows.Forms.TabPage();
            this.listBox4 = new System.Windows.Forms.ListBox();
            this.SolidWorksActTab = new System.Windows.Forms.TabPage();
            this.listBox5 = new System.Windows.Forms.ListBox();
            this.OtherActTab = new System.Windows.Forms.TabPage();
            this.listBox6 = new System.Windows.Forms.ListBox();
            this.VarsTab = new System.Windows.Forms.TabPage();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox7 = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.ActionsVarsTabControl.SuspendLayout();
            this.ActionsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.AddActionTabs.SuspendLayout();
            this.UIActTab.SuspendLayout();
            this.FlowContActTab.SuspendLayout();
            this.ExcelActTab.SuspendLayout();
            this.MathCadActTab.SuspendLayout();
            this.SolidWorksActTab.SuspendLayout();
            this.OtherActTab.SuspendLayout();
            this.VarsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // ActionsVarsTabControl
            // 
            this.ActionsVarsTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsVarsTabControl.Controls.Add(this.ActionsTab);
            this.ActionsVarsTabControl.Controls.Add(this.VarsTab);
            this.ActionsVarsTabControl.Location = new System.Drawing.Point(12, 12);
            this.ActionsVarsTabControl.Name = "ActionsVarsTabControl";
            this.ActionsVarsTabControl.SelectedIndex = 0;
            this.ActionsVarsTabControl.Size = new System.Drawing.Size(455, 471);
            this.ActionsVarsTabControl.TabIndex = 0;
            // 
            // ActionsTab
            // 
            this.ActionsTab.Controls.Add(this.splitContainer1);
            this.ActionsTab.Location = new System.Drawing.Point(4, 22);
            this.ActionsTab.Name = "ActionsTab";
            this.ActionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ActionsTab.Size = new System.Drawing.Size(447, 445);
            this.ActionsTab.TabIndex = 0;
            this.ActionsTab.Text = "Actions";
            this.ActionsTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ActionsList);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.AddActionTabs);
            this.splitContainer1.Size = new System.Drawing.Size(447, 445);
            this.splitContainer1.SplitterDistance = 298;
            this.splitContainer1.TabIndex = 0;
            // 
            // ActionsList
            // 
            this.ActionsList.AllowDrop = true;
            this.ActionsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsList.FormattingEnabled = true;
            this.ActionsList.Location = new System.Drawing.Point(6, 3);
            this.ActionsList.Name = "ActionsList";
            this.ActionsList.Size = new System.Drawing.Size(434, 290);
            this.ActionsList.TabIndex = 2;
            // 
            // AddActionTabs
            // 
            this.AddActionTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.AddActionTabs.Controls.Add(this.UIActTab);
            this.AddActionTabs.Controls.Add(this.FlowContActTab);
            this.AddActionTabs.Controls.Add(this.ExcelActTab);
            this.AddActionTabs.Controls.Add(this.MathCadActTab);
            this.AddActionTabs.Controls.Add(this.SolidWorksActTab);
            this.AddActionTabs.Controls.Add(this.OtherActTab);
            this.AddActionTabs.Location = new System.Drawing.Point(3, 3);
            this.AddActionTabs.Name = "AddActionTabs";
            this.AddActionTabs.SelectedIndex = 0;
            this.AddActionTabs.Size = new System.Drawing.Size(444, 137);
            this.AddActionTabs.TabIndex = 3;
            // 
            // UIActTab
            // 
            this.UIActTab.Controls.Add(this.listBox1);
            this.UIActTab.Location = new System.Drawing.Point(4, 22);
            this.UIActTab.Name = "UIActTab";
            this.UIActTab.Padding = new System.Windows.Forms.Padding(3);
            this.UIActTab.Size = new System.Drawing.Size(436, 111);
            this.UIActTab.TabIndex = 0;
            this.UIActTab.Text = "UI";
            this.UIActTab.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(7, 7);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(413, 95);
            this.listBox1.TabIndex = 0;
            // 
            // FlowContActTab
            // 
            this.FlowContActTab.Controls.Add(this.listBox2);
            this.FlowContActTab.Location = new System.Drawing.Point(4, 22);
            this.FlowContActTab.Name = "FlowContActTab";
            this.FlowContActTab.Padding = new System.Windows.Forms.Padding(3);
            this.FlowContActTab.Size = new System.Drawing.Size(436, 111);
            this.FlowContActTab.TabIndex = 1;
            this.FlowContActTab.Text = "Flow Control";
            this.FlowContActTab.UseVisualStyleBackColor = true;
            // 
            // listBox2
            // 
            this.listBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(12, 8);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(413, 95);
            this.listBox2.TabIndex = 1;
            // 
            // ExcelActTab
            // 
            this.ExcelActTab.Controls.Add(this.listBox3);
            this.ExcelActTab.Location = new System.Drawing.Point(4, 22);
            this.ExcelActTab.Name = "ExcelActTab";
            this.ExcelActTab.Padding = new System.Windows.Forms.Padding(3);
            this.ExcelActTab.Size = new System.Drawing.Size(436, 111);
            this.ExcelActTab.TabIndex = 2;
            this.ExcelActTab.Text = "Excel";
            this.ExcelActTab.UseVisualStyleBackColor = true;
            // 
            // listBox3
            // 
            this.listBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(12, 8);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(413, 95);
            this.listBox3.TabIndex = 1;
            // 
            // MathCadActTab
            // 
            this.MathCadActTab.Controls.Add(this.listBox4);
            this.MathCadActTab.Location = new System.Drawing.Point(4, 22);
            this.MathCadActTab.Name = "MathCadActTab";
            this.MathCadActTab.Size = new System.Drawing.Size(436, 111);
            this.MathCadActTab.TabIndex = 3;
            this.MathCadActTab.Text = "MathCAD";
            this.MathCadActTab.UseVisualStyleBackColor = true;
            // 
            // listBox4
            // 
            this.listBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox4.FormattingEnabled = true;
            this.listBox4.Location = new System.Drawing.Point(12, 8);
            this.listBox4.Name = "listBox4";
            this.listBox4.Size = new System.Drawing.Size(413, 95);
            this.listBox4.TabIndex = 1;
            // 
            // SolidWorksActTab
            // 
            this.SolidWorksActTab.Controls.Add(this.listBox5);
            this.SolidWorksActTab.Location = new System.Drawing.Point(4, 22);
            this.SolidWorksActTab.Name = "SolidWorksActTab";
            this.SolidWorksActTab.Size = new System.Drawing.Size(436, 111);
            this.SolidWorksActTab.TabIndex = 4;
            this.SolidWorksActTab.Text = "SolidWorks";
            this.SolidWorksActTab.UseVisualStyleBackColor = true;
            // 
            // listBox5
            // 
            this.listBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox5.FormattingEnabled = true;
            this.listBox5.Location = new System.Drawing.Point(12, 8);
            this.listBox5.Name = "listBox5";
            this.listBox5.Size = new System.Drawing.Size(413, 95);
            this.listBox5.TabIndex = 1;
            // 
            // OtherActTab
            // 
            this.OtherActTab.Controls.Add(this.listBox6);
            this.OtherActTab.Location = new System.Drawing.Point(4, 22);
            this.OtherActTab.Name = "OtherActTab";
            this.OtherActTab.Size = new System.Drawing.Size(436, 111);
            this.OtherActTab.TabIndex = 5;
            this.OtherActTab.Text = "Other Actions";
            this.OtherActTab.UseVisualStyleBackColor = true;
            // 
            // listBox6
            // 
            this.listBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox6.FormattingEnabled = true;
            this.listBox6.Location = new System.Drawing.Point(12, 8);
            this.listBox6.Name = "listBox6";
            this.listBox6.Size = new System.Drawing.Size(413, 95);
            this.listBox6.TabIndex = 1;
            // 
            // VarsTab
            // 
            this.VarsTab.Controls.Add(this.textBox2);
            this.VarsTab.Controls.Add(this.textBox1);
            this.VarsTab.Controls.Add(this.button3);
            this.VarsTab.Controls.Add(this.comboBox2);
            this.VarsTab.Controls.Add(this.comboBox1);
            this.VarsTab.Controls.Add(this.listBox7);
            this.VarsTab.Location = new System.Drawing.Point(4, 22);
            this.VarsTab.Name = "VarsTab";
            this.VarsTab.Padding = new System.Windows.Forms.Padding(3);
            this.VarsTab.Size = new System.Drawing.Size(447, 445);
            this.VarsTab.TabIndex = 1;
            this.VarsTab.Text = "Variables";
            this.VarsTab.UseVisualStyleBackColor = true;
            // 
            // LoadBtn
            // 
            this.LoadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadBtn.Location = new System.Drawing.Point(473, 12);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(75, 23);
            this.LoadBtn.TabIndex = 1;
            this.LoadBtn.Text = "&Load";
            this.LoadBtn.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 489);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(455, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(473, 489);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "&Run";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(473, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "&Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // listBox7
            // 
            this.listBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox7.FormattingEnabled = true;
            this.listBox7.Location = new System.Drawing.Point(7, 4);
            this.listBox7.Name = "listBox7";
            this.listBox7.Size = new System.Drawing.Size(434, 407);
            this.listBox7.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(103, 419);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(89, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.Text = "Type";
            // 
            // comboBox2
            // 
            this.comboBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(297, 419);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(89, 21);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.Text = "Unit";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(400, 419);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "&Add";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 419);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(90, 20);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Name";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(198, 419);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(90, 20);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "Value";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 524);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.LoadBtn);
            this.Controls.Add(this.ActionsVarsTabControl);
            this.Name = "MainWindow";
            this.Text = "Centipede";
            this.ActionsVarsTabControl.ResumeLayout(false);
            this.ActionsTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.AddActionTabs.ResumeLayout(false);
            this.UIActTab.ResumeLayout(false);
            this.FlowContActTab.ResumeLayout(false);
            this.ExcelActTab.ResumeLayout(false);
            this.MathCadActTab.ResumeLayout(false);
            this.SolidWorksActTab.ResumeLayout(false);
            this.OtherActTab.ResumeLayout(false);
            this.VarsTab.ResumeLayout(false);
            this.VarsTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ActionsVarsTabControl;
        private System.Windows.Forms.TabPage ActionsTab;
        private System.Windows.Forms.TabPage VarsTab;
        private System.Windows.Forms.Button LoadBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox ActionsList;
        private System.Windows.Forms.TabControl AddActionTabs;
        private System.Windows.Forms.TabPage UIActTab;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TabPage FlowContActTab;
        private System.Windows.Forms.TabPage ExcelActTab;
        private System.Windows.Forms.TabPage MathCadActTab;
        private System.Windows.Forms.TabPage SolidWorksActTab;
        private System.Windows.Forms.TabPage OtherActTab;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.ListBox listBox4;
        private System.Windows.Forms.ListBox listBox5;
        private System.Windows.Forms.ListBox listBox6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListBox listBox7;
    }
}

