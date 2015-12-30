namespace eCPU.Screens
{
    partial class GameWindow
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
            this.components = new System.ComponentModel.Container();
            this.pbWindow = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSpaceInvadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugSpaceInvadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbWindow)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbWindow
            // 
            this.pbWindow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbWindow.Dock = System.Windows.Forms.DockStyle.Top;
            this.pbWindow.Location = new System.Drawing.Point(0, 24);
            this.pbWindow.Name = "pbWindow";
            this.pbWindow.Size = new System.Drawing.Size(451, 397);
            this.pbWindow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbWindow.TabIndex = 0;
            this.pbWindow.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gameToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(451, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runSpaceInvadersToolStripMenuItem,
            this.debugSpaceInvadersToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // runSpaceInvadersToolStripMenuItem
            // 
            this.runSpaceInvadersToolStripMenuItem.Name = "runSpaceInvadersToolStripMenuItem";
            this.runSpaceInvadersToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.runSpaceInvadersToolStripMenuItem.Text = "Run Space Invaders";
            this.runSpaceInvadersToolStripMenuItem.Click += new System.EventHandler(this.runSpaceInvadersToolStripMenuItem_Click);
            // 
            // debugSpaceInvadersToolStripMenuItem
            // 
            this.debugSpaceInvadersToolStripMenuItem.Name = "debugSpaceInvadersToolStripMenuItem";
            this.debugSpaceInvadersToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.debugSpaceInvadersToolStripMenuItem.Text = "Debug Space Invaders";
            this.debugSpaceInvadersToolStripMenuItem.Click += new System.EventHandler(this.debugSpaceInvadersToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 419);
            this.Controls.Add(this.pbWindow);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "GameWindow";
            this.Text = "GameWindow";
            ((System.ComponentModel.ISupportInitialize)(this.pbWindow)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWindow;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSpaceInvadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugSpaceInvadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}