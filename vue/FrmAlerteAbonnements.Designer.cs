
namespace Mediatek86.vue
{
    partial class FrmAlerteAbonnements
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
            this.dgvFinAbonnements = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinAbonnements)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvFinAbonnements
            // 
            this.dgvFinAbonnements.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFinAbonnements.Location = new System.Drawing.Point(60, 50);
            this.dgvFinAbonnements.Name = "dgvFinAbonnements";
            this.dgvFinAbonnements.RowHeadersWidth = 51;
            this.dgvFinAbonnements.RowTemplate.Height = 24;
            this.dgvFinAbonnements.Size = new System.Drawing.Size(650, 346);
            this.dgvFinAbonnements.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(131, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Récapitulatif des abonnement se finissant dans moins de 30 jours";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrmAlerteAbonnements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvFinAbonnements);
            this.Name = "FrmAlerteAbonnements";
            this.Text = "FrmAlerteAbonnements";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFinAbonnements)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvFinAbonnements;
        private System.Windows.Forms.Label label1;
    }
}