namespace yahtzee_new
{
    partial class NewYahtzee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewYahtzee));
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            btnRoll = new Button();
            btnNextRound = new Button();
            btnHelp = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(30, 48);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(219, 219);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackgroundImageLayout = ImageLayout.None;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(255, 48);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(219, 219);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.BackgroundImageLayout = ImageLayout.None;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(480, 48);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(219, 219);
            pictureBox3.TabIndex = 2;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.BackgroundImageLayout = ImageLayout.None;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(705, 48);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(219, 219);
            pictureBox4.TabIndex = 3;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.BackgroundImageLayout = ImageLayout.None;
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(930, 48);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(219, 219);
            pictureBox5.TabIndex = 4;
            pictureBox5.TabStop = false;
            // 
            // btnRoll
            // 
            btnRoll.Location = new Point(21, 381);
            btnRoll.Name = "btnRoll";
            btnRoll.Size = new Size(145, 238);
            btnRoll.TabIndex = 5;
            btnRoll.Text = "Roll";
            btnRoll.UseVisualStyleBackColor = true;
            // 
            // btnNextRound
            // 
            btnNextRound.Location = new Point(985, 381);
            btnNextRound.Name = "btnNextRound";
            btnNextRound.Size = new Size(164, 238);
            btnNextRound.TabIndex = 6;
            btnNextRound.Text = "Next Round";
            btnNextRound.UseVisualStyleBackColor = true;
            // 
            // btnHelp
            // 
            btnHelp.Cursor = Cursors.Help;
            btnHelp.Location = new Point(1119, 12);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(30, 29);
            btnHelp.TabIndex = 7;
            btnHelp.Text = "?";
            btnHelp.UseVisualStyleBackColor = true;
            btnHelp.Click += btnHelp_Click;
            // 
            // NewYahtzee
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1171, 893);
            Controls.Add(btnHelp);
            Controls.Add(btnNextRound);
            Controls.Add(btnRoll);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "NewYahtzee";
            Text = "Yahtzee";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
        }

        #endregion

        public PictureBox pictureBox1;
        public PictureBox pictureBox2;
        public PictureBox pictureBox3;
        public PictureBox pictureBox4;
        public PictureBox pictureBox5;
        private Button btnRoll;
        private Button btnNextRound;
        private Button btnHelp;
    }
}
