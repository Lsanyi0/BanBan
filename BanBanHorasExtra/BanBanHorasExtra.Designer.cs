namespace BanBanHorasExtra
{
    partial class BanBanHorasExtra
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
            this.dpDesde = new System.Windows.Forms.DateTimePicker();
            this.lbNombreEmpleado = new System.Windows.Forms.Label();
            this.cbEmpleados = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dpHasta = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.btAceptar = new System.Windows.Forms.Button();
            this.btCancelar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // dpDesde
            // 
            this.dpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dpDesde.Location = new System.Drawing.Point(134, 39);
            this.dpDesde.MinDate = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            this.dpDesde.Name = "dpDesde";
            this.dpDesde.Size = new System.Drawing.Size(318, 20);
            this.dpDesde.TabIndex = 2;
            // 
            // lbNombreEmpleado
            // 
            this.lbNombreEmpleado.AutoSize = true;
            this.lbNombreEmpleado.Location = new System.Drawing.Point(12, 15);
            this.lbNombreEmpleado.Name = "lbNombreEmpleado";
            this.lbNombreEmpleado.Size = new System.Drawing.Size(115, 13);
            this.lbNombreEmpleado.TabIndex = 1;
            this.lbNombreEmpleado.Text = "Seleccionar empleado:";
            // 
            // cbEmpleados
            // 
            this.cbEmpleados.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEmpleados.FormattingEnabled = true;
            this.cbEmpleados.Location = new System.Drawing.Point(134, 12);
            this.cbEmpleados.MaxDropDownItems = 20;
            this.cbEmpleados.Name = "cbEmpleados";
            this.cbEmpleados.Size = new System.Drawing.Size(318, 21);
            this.cbEmpleados.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hora extra desde:";
            // 
            // dpHasta
            // 
            this.dpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dpHasta.Location = new System.Drawing.Point(134, 65);
            this.dpHasta.Name = "dpHasta";
            this.dpHasta.Size = new System.Drawing.Size(318, 20);
            this.dpHasta.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hasta:";
            // 
            // btAceptar
            // 
            this.btAceptar.Location = new System.Drawing.Point(377, 102);
            this.btAceptar.Name = "btAceptar";
            this.btAceptar.Size = new System.Drawing.Size(75, 23);
            this.btAceptar.TabIndex = 4;
            this.btAceptar.Text = "Aceptar";
            this.btAceptar.UseVisualStyleBackColor = true;
            // 
            // btCancelar
            // 
            this.btCancelar.Location = new System.Drawing.Point(296, 102);
            this.btCancelar.Name = "btCancelar";
            this.btCancelar.Size = new System.Drawing.Size(75, 23);
            this.btCancelar.TabIndex = 5;
            this.btCancelar.Text = "Cancelar";
            this.btCancelar.UseVisualStyleBackColor = true;
            // 
            // BanBanHorasExtra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(464, 129);
            this.Controls.Add(this.btCancelar);
            this.Controls.Add(this.btAceptar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbEmpleados);
            this.Controls.Add(this.lbNombreEmpleado);
            this.Controls.Add(this.dpHasta);
            this.Controls.Add(this.dpDesde);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BanBanHorasExtra";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BabBanHorasExtra";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dpDesde;
        private System.Windows.Forms.Label lbNombreEmpleado;
        private System.Windows.Forms.ComboBox cbEmpleados;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dpHasta;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btAceptar;
        private System.Windows.Forms.Button btCancelar;
    }
}

