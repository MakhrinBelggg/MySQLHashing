namespace MySQLOnline
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            toolStrip1 = new ToolStrip();
            toolStripLabel3 = new ToolStripLabel();
            toolStripTextBox3 = new ToolStripTextBox();
            toolStripLabel1 = new ToolStripLabel();
            toolStripTextBox1 = new ToolStripTextBox();
            toolStripLabel2 = new ToolStripLabel();
            toolStripTextBox2 = new ToolStripTextBox();
            toolStripButton1 = new ToolStripButton();
            panel1 = new Panel();
            label2 = new Label();
            button1 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            dataGridView1 = new DataGridView();
            toolStripButton2 = new ToolStripButton();
            toolStrip1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.AutoSize = false;
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new Size(17, 17);
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel3, toolStripTextBox3, toolStripLabel1, toolStripTextBox1, toolStripLabel2, toolStripTextBox2, toolStripButton2, toolStripButton1 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(914, 46);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel3
            // 
            toolStripLabel3.Margin = new Padding(9, 1, 0, 2);
            toolStripLabel3.Name = "toolStripLabel3";
            toolStripLabel3.Size = new Size(114, 43);
            toolStripLabel3.Text = "database name:";
            // 
            // toolStripTextBox3
            // 
            toolStripTextBox3.AutoSize = false;
            toolStripTextBox3.Name = "toolStripTextBox3";
            toolStripTextBox3.Size = new Size(148, 27);
            toolStripTextBox3.Click += toolStripTextBox3_Click;
            toolStripTextBox3.TextChanged += toolStripTextBox3_TextChanged;
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Margin = new Padding(10, 1, 0, 2);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(46, 43);
            toolStripLabel1.Text = "login:";
            // 
            // toolStripTextBox1
            // 
            toolStripTextBox1.AutoSize = false;
            toolStripTextBox1.Name = "toolStripTextBox1";
            toolStripTextBox1.Size = new Size(148, 27);
            toolStripTextBox1.Click += toolStripTextBox1_Click;
            toolStripTextBox1.TextChanged += toolStripTextBox1_TextChanged;
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Margin = new Padding(10, 1, 0, 2);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(75, 43);
            toolStripLabel2.Text = "password:";
            // 
            // toolStripTextBox2
            // 
            toolStripTextBox2.Name = "toolStripTextBox2";
            toolStripTextBox2.Size = new Size(148, 46);
            toolStripTextBox2.Click += toolStripTextBox2_Click;
            toolStripTextBox2.TextChanged += toolStripTextBox2_TextChanged;
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(74, 43);
            toolStripButton1.Text = "Start Test";
            toolStripButton1.Click += toolStripButton1_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(dataGridView1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 46);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(914, 483);
            panel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.FlatStyle = FlatStyle.Flat;
            label2.ForeColor = Color.Firebrick;
            label2.Location = new Point(344, 15);
            label2.Name = "label2";
            label2.Size = new Size(265, 20);
            label2.TabIndex = 4;
            label2.Text = "Error! Data integrity has been violated ";
            label2.Visible = false;
            // 
            // button1
            // 
            button1.Location = new Point(810, 8);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(90, 28);
            button1.TabIndex = 3;
            button1.Text = "Perform";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 16);
            label1.Name = "label1";
            label1.Size = new Size(111, 20);
            label1.TabIndex = 2;
            label1.Text = "MySQL запрос:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(14, 40);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(886, 70);
            textBox1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(14, 118);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 43;
            dataGridView1.RowTemplate.Height = 27;
            dataGridView1.Size = new Size(887, 352);
            dataGridView1.TabIndex = 0;
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(62, 43);
            toolStripButton2.Text = "Refresh";
            toolStripButton2.Click += toolStripButton2_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 529);
            Controls.Add(panel1);
            Controls.Add(toolStrip1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripLabel toolStripLabel2;
        private ToolStripTextBox toolStripTextBox2;
        private Panel panel1;
        private Label label1;
        private TextBox textBox1;
        private DataGridView dataGridView1;
        private ToolStripLabel toolStripLabel3;
        private ToolStripTextBox toolStripTextBox3;
        private Button button1;
        private ToolStripButton toolStripButton1;
        public Label label2;
        private ToolStripButton toolStripButton2;
    }
}