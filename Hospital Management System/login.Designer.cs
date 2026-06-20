namespace Hospital_Management_System
{
    partial class login
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
            label1 = new Label();
            panel1 = new Panel();
            Clear = new Button();
            button1 = new Button();
            pictureBox2 = new PictureBox();
            Password = new TextBox();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            UserName = new TextBox();
            label3 = new Label();
            label2 = new Label();
            button2 = new Button();
            label5 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Sylfaen", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(20, 19);
            label1.Name = "label1";
            label1.Size = new Size(254, 44);
            label1.TabIndex = 0;
            label1.Text = "Welcome Back!";
            label1.TextAlign = ContentAlignment.TopCenter;
            label1.Click += label1_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.AliceBlue;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label5);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(Clear);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(pictureBox2);
            panel1.Controls.Add(Password);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(UserName);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(472, 65);
            panel1.Name = "panel1";
            panel1.Size = new Size(285, 337);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // Clear
            // 
            Clear.BackColor = Color.DeepSkyBlue;
            Clear.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Clear.ForeColor = SystemColors.ControlLightLight;
            Clear.Location = new Point(145, 231);
            Clear.Name = "Clear";
            Clear.Size = new Size(80, 33);
            Clear.TabIndex = 10;
            Clear.Text = "⟳Clear";
            Clear.TextImageRelation = TextImageRelation.ImageAboveText;
            Clear.UseVisualStyleBackColor = false;
            Clear.Click += Clear_Click_1;
            // 
            // button1
            // 
            button1.BackColor = Color.DeepSkyBlue;
            button1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = SystemColors.ControlLightLight;
            button1.Location = new Point(52, 231);
            button1.Name = "button1";
            button1.Size = new Size(80, 32);
            button1.TabIndex = 9;
            button1.Text = "🡢Login";
            button1.TextImageRelation = TextImageRelation.ImageAboveText;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.password;
            pictureBox2.Location = new Point(54, 165);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(29, 20);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // Password
            // 
            Password.Location = new Point(52, 187);
            Password.Name = "Password";
            Password.Size = new Size(173, 27);
            Password.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(86, 165);
            label4.Name = "label4";
            label4.Size = new Size(73, 20);
            label4.TabIndex = 6;
            label4.Text = "Password";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.download;
            pictureBox1.Location = new Point(52, 95);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(29, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // UserName
            // 
            UserName.Location = new Point(52, 120);
            UserName.Name = "UserName";
            UserName.Size = new Size(173, 27);
            UserName.TabIndex = 4;
            UserName.TextChanged += textBox1_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(80, 99);
            label3.Name = "label3";
            label3.Size = new Size(81, 20);
            label3.TabIndex = 2;
            label3.Text = "UserName";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Sitka Banner Semibold", 7.79999971F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(68, 53);
            label2.Name = "label2";
            label2.Size = new Size(140, 19);
            label2.TabIndex = 1;
            label2.Text = "Please sign in to continue";
            label2.Click += label2_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 128, 0);
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = SystemColors.ControlLightLight;
            button2.Location = new Point(186, 277);
            button2.Name = "button2";
            button2.Size = new Size(83, 29);
            button2.TabIndex = 11;
            button2.Text = "Sign Up";
            button2.TextImageRelation = TextImageRelation.ImageAboveText;
            button2.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(20, 281);
            label5.Name = "label5";
            label5.Size = new Size(166, 19);
            label5.TabIndex = 12;
            label5.Text = "Don't have an account ?";
            // 
            // login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.login;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Name = "login";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private Panel panel1;
        private Label label3;
        private Label label2;
        private PictureBox pictureBox1;
        private TextBox UserName;
        private PictureBox pictureBox2;
        private TextBox Password;
        private Label label4;
        private Button button1;
        private Button Clear;
        private Button button2;
        private Label label5;
    }
}
