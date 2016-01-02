namespace KDS.e8080
{
    partial class DebugWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtSP = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPC = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.gridPC = new System.Windows.Forms.DataGridView();
            this.PC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Data = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label15 = new System.Windows.Forms.Label();
            this.txtNext = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCycles = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRegA = new System.Windows.Forms.TextBox();
            this.txtBC = new System.Windows.Forms.TextBox();
            this.txtDE = new System.Windows.Forms.TextBox();
            this.txtHL = new System.Windows.Forms.TextBox();
            this.txtFlags = new System.Windows.Forms.TextBox();
            this.txtRun = new System.Windows.Forms.TextBox();
            this.btnRunN = new System.Windows.Forms.Button();
            this.btnInt1 = new System.Windows.Forms.Button();
            this.btnInt2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridPC)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(273, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "A";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSP
            // 
            this.txtSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSP.Location = new System.Drawing.Point(397, 40);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(50, 22);
            this.txtSP.TabIndex = 17;
            this.txtSP.Text = "8888";
            this.txtSP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(327, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 23);
            this.label8.TabIndex = 16;
            this.label8.Text = "SP:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtPC
            // 
            this.txtPC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPC.Location = new System.Drawing.Point(397, 12);
            this.txtPC.Name = "txtPC";
            this.txtPC.Size = new System.Drawing.Size(50, 22);
            this.txtPC.TabIndex = 15;
            this.txtPC.Text = "88";
            this.txtPC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(327, 12);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 23);
            this.label9.TabIndex = 14;
            this.label9.Text = "PC:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // gridPC
            // 
            this.gridPC.AllowUserToAddRows = false;
            this.gridPC.AllowUserToDeleteRows = false;
            this.gridPC.AllowUserToResizeColumns = false;
            this.gridPC.AllowUserToResizeRows = false;
            this.gridPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PC,
            this.Data});
            this.gridPC.Location = new System.Drawing.Point(12, 12);
            this.gridPC.MultiSelect = false;
            this.gridPC.Name = "gridPC";
            this.gridPC.ReadOnly = true;
            this.gridPC.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.gridPC.Size = new System.Drawing.Size(240, 399);
            this.gridPC.TabIndex = 28;
            // 
            // PC
            // 
            this.PC.HeaderText = "PC";
            this.PC.Name = "PC";
            this.PC.ReadOnly = true;
            this.PC.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.PC.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Data
            // 
            this.Data.HeaderText = "Data";
            this.Data.Name = "Data";
            this.Data.ReadOnly = true;
            this.Data.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Data.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // label15
            // 
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(263, 199);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 23);
            this.label15.TabIndex = 29;
            this.label15.Text = "Next:";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNext
            // 
            this.txtNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNext.Location = new System.Drawing.Point(333, 199);
            this.txtNext.Name = "txtNext";
            this.txtNext.Size = new System.Drawing.Size(238, 22);
            this.txtNext.TabIndex = 30;
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(353, 246);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(94, 28);
            this.btnNext.TabIndex = 32;
            this.btnNext.Text = "Run 1";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // txtCount
            // 
            this.txtCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCount.Location = new System.Drawing.Point(333, 142);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(238, 22);
            this.txtCount.TabIndex = 35;
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(263, 142);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 23);
            this.label16.TabIndex = 34;
            this.label16.Text = "Count:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(333, 69);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 23);
            this.label10.TabIndex = 36;
            this.label10.Text = "B-C";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(393, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 23);
            this.label11.TabIndex = 37;
            this.label11.Text = "D-E";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(453, 69);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(54, 23);
            this.label12.TabIndex = 38;
            this.label12.Text = "H-L";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(513, 69);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(54, 23);
            this.label13.TabIndex = 39;
            this.label13.Text = "flags";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCycles
            // 
            this.txtCycles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCycles.Location = new System.Drawing.Point(333, 171);
            this.txtCycles.Name = "txtCycles";
            this.txtCycles.Size = new System.Drawing.Size(238, 22);
            this.txtCycles.TabIndex = 41;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(263, 171);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 40;
            this.label2.Text = "Cycles:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRegA
            // 
            this.txtRegA.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRegA.Location = new System.Drawing.Point(273, 95);
            this.txtRegA.Name = "txtRegA";
            this.txtRegA.Size = new System.Drawing.Size(54, 22);
            this.txtRegA.TabIndex = 42;
            this.txtRegA.Text = "8888";
            this.txtRegA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtBC
            // 
            this.txtBC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBC.Location = new System.Drawing.Point(333, 95);
            this.txtBC.Name = "txtBC";
            this.txtBC.Size = new System.Drawing.Size(54, 22);
            this.txtBC.TabIndex = 43;
            this.txtBC.Text = "8888";
            this.txtBC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtDE
            // 
            this.txtDE.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDE.Location = new System.Drawing.Point(393, 95);
            this.txtDE.Name = "txtDE";
            this.txtDE.Size = new System.Drawing.Size(54, 22);
            this.txtDE.TabIndex = 44;
            this.txtDE.Text = "8888";
            this.txtDE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtHL
            // 
            this.txtHL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHL.Location = new System.Drawing.Point(453, 95);
            this.txtHL.Name = "txtHL";
            this.txtHL.Size = new System.Drawing.Size(54, 22);
            this.txtHL.TabIndex = 45;
            this.txtHL.Text = "8888";
            this.txtHL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtFlags
            // 
            this.txtFlags.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFlags.Location = new System.Drawing.Point(513, 95);
            this.txtFlags.Name = "txtFlags";
            this.txtFlags.Size = new System.Drawing.Size(54, 22);
            this.txtFlags.TabIndex = 46;
            this.txtFlags.Text = "8888";
            this.txtFlags.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRun
            // 
            this.txtRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRun.Location = new System.Drawing.Point(457, 286);
            this.txtRun.Name = "txtRun";
            this.txtRun.Size = new System.Drawing.Size(75, 22);
            this.txtRun.TabIndex = 48;
            this.txtRun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnRunN
            // 
            this.btnRunN.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunN.Location = new System.Drawing.Point(354, 280);
            this.btnRunN.Name = "btnRunN";
            this.btnRunN.Size = new System.Drawing.Size(94, 28);
            this.btnRunN.TabIndex = 49;
            this.btnRunN.Text = "Run N";
            this.btnRunN.UseVisualStyleBackColor = true;
            this.btnRunN.Click += new System.EventHandler(this.btnRunN_Click);
            // 
            // btnInt1
            // 
            this.btnInt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInt1.Location = new System.Drawing.Point(453, 347);
            this.btnInt1.Name = "btnInt1";
            this.btnInt1.Size = new System.Drawing.Size(75, 31);
            this.btnInt1.TabIndex = 51;
            this.btnInt1.Text = "Int 1";
            this.btnInt1.UseVisualStyleBackColor = true;
            // 
            // btnInt2
            // 
            this.btnInt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInt2.Location = new System.Drawing.Point(453, 384);
            this.btnInt2.Name = "btnInt2";
            this.btnInt2.Size = new System.Drawing.Size(75, 31);
            this.btnInt2.TabIndex = 52;
            this.btnInt2.Text = "Int 2";
            this.btnInt2.UseVisualStyleBackColor = true;
            // 
            // DebugWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 420);
            this.Controls.Add(this.btnRunN);
            this.Controls.Add(this.txtRun);
            this.Controls.Add(this.txtFlags);
            this.Controls.Add(this.txtHL);
            this.Controls.Add(this.txtDE);
            this.Controls.Add(this.txtBC);
            this.Controls.Add(this.txtRegA);
            this.Controls.Add(this.txtCycles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.txtNext);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.gridPC);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtPC);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Name = "DebugWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "8080 Debugger";
            ((System.ComponentModel.ISupportInitialize)(this.gridPC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPC;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView gridPC;
        private System.Windows.Forms.DataGridViewTextBoxColumn PC;
        private System.Windows.Forms.DataGridViewTextBoxColumn Data;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtNext;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtCycles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRegA;
        private System.Windows.Forms.TextBox txtBC;
        private System.Windows.Forms.TextBox txtDE;
        private System.Windows.Forms.TextBox txtHL;
        private System.Windows.Forms.TextBox txtFlags;
        private System.Windows.Forms.TextBox txtRun;
        private System.Windows.Forms.Button btnRunN;
        private System.Windows.Forms.Button btnInt1;
        private System.Windows.Forms.Button btnInt2;
    }
}

