
namespace BGG_PlayStats
{
    partial class FormError
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
            this.btnOk = new System.Windows.Forms.Button();
            this.txtErrorMsg = new System.Windows.Forms.TextBox();
            this.txtStackTrace = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(364, 415);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtErrorMsg
            // 
            this.txtErrorMsg.Location = new System.Drawing.Point(13, 13);
            this.txtErrorMsg.Name = "txtErrorMsg";
            this.txtErrorMsg.Size = new System.Drawing.Size(775, 20);
            this.txtErrorMsg.TabIndex = 1;
            // 
            // txtStackTrace
            // 
            this.txtStackTrace.Location = new System.Drawing.Point(13, 40);
            this.txtStackTrace.Multiline = true;
            this.txtStackTrace.Name = "txtStackTrace";
            this.txtStackTrace.Size = new System.Drawing.Size(775, 369);
            this.txtStackTrace.TabIndex = 2;
            // 
            // FormError
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOk;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtStackTrace);
            this.Controls.Add(this.txtErrorMsg);
            this.Controls.Add(this.btnOk);
            this.Name = "FormError";
            this.Text = "FormError";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TextBox txtErrorMsg;
        private System.Windows.Forms.TextBox txtStackTrace;
    }
}