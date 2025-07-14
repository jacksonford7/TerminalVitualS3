namespace MonitorVacios
{
    partial class FrmMonitor
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.ckTogle = new System.Windows.Forms.CheckBox();
            this.lsMensajes = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(366, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 42);
            this.button1.TabIndex = 9;
            this.button1.Text = "Reset";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // ckTogle
            // 
            this.ckTogle.Appearance = System.Windows.Forms.Appearance.Button;
            this.ckTogle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckTogle.Location = new System.Drawing.Point(452, 9);
            this.ckTogle.Name = "ckTogle";
            this.ckTogle.Size = new System.Drawing.Size(105, 42);
            this.ckTogle.TabIndex = 8;
            this.ckTogle.Text = "Monitorear";
            this.ckTogle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ckTogle.UseVisualStyleBackColor = true;
            this.ckTogle.CheckedChanged += new System.EventHandler(this.ckTogle_CheckedChanged);
            // 
            // lsMensajes
            // 
            this.lsMensajes.BackColor = System.Drawing.Color.Black;
            this.lsMensajes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lsMensajes.ForeColor = System.Drawing.SystemColors.Window;
            this.lsMensajes.FormattingEnabled = true;
            this.lsMensajes.Location = new System.Drawing.Point(6, 57);
            this.lsMensajes.Name = "lsMensajes";
            this.lsMensajes.Size = new System.Drawing.Size(551, 392);
            this.lsMensajes.TabIndex = 7;
            // 
            // FrmMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 449);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ckTogle);
            this.Controls.Add(this.lsMensajes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmMonitor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMonitor_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox ckTogle;
        private System.Windows.Forms.ListBox lsMensajes;
    }
}

