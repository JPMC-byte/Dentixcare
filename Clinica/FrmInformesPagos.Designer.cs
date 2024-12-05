namespace GUI
{
    partial class FrmInformesPagos
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
            this.CBAnual = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CBMensual = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.CBVista = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.LBPerfil = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel11 = new System.Windows.Forms.Panel();
            this.LBEfectivo = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.LBMontoPagado = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.LBPSE = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.LBTotal = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel12 = new System.Windows.Forms.Panel();
            this.LBTarjetaDebito = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.panel14 = new System.Windows.Forms.Panel();
            this.LBTarjetaCredito = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel5.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.panel14.SuspendLayout();
            this.SuspendLayout();
            // 
            // CBAnual
            // 
            this.CBAnual.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CBAnual.BackColor = System.Drawing.Color.PowderBlue;
            this.CBAnual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBAnual.Enabled = false;
            this.CBAnual.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBAnual.ForeColor = System.Drawing.Color.DimGray;
            this.CBAnual.FormattingEnabled = true;
            this.CBAnual.Items.AddRange(new object[] {
            "2024",
            "2025",
            "2026"});
            this.CBAnual.Location = new System.Drawing.Point(573, 20);
            this.CBAnual.Name = "CBAnual";
            this.CBAnual.Size = new System.Drawing.Size(148, 24);
            this.CBAnual.TabIndex = 148;
            this.CBAnual.SelectedIndexChanged += new System.EventHandler(this.CBAnual_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label9.Location = new System.Drawing.Point(523, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 19);
            this.label9.TabIndex = 147;
            this.label9.Text = "Año:";
            // 
            // CBMensual
            // 
            this.CBMensual.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CBMensual.BackColor = System.Drawing.Color.PowderBlue;
            this.CBMensual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBMensual.Enabled = false;
            this.CBMensual.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBMensual.ForeColor = System.Drawing.Color.DimGray;
            this.CBMensual.FormattingEnabled = true;
            this.CBMensual.Items.AddRange(new object[] {
            "Enero",
            "Febrero",
            "Marzo",
            "Abril",
            "Mayo",
            "Junio",
            "Julio",
            "Agosto",
            "Septiembre",
            "Octubre",
            "Noviembre",
            "Diciembre"});
            this.CBMensual.Location = new System.Drawing.Point(357, 20);
            this.CBMensual.Name = "CBMensual";
            this.CBMensual.Size = new System.Drawing.Size(148, 24);
            this.CBMensual.TabIndex = 145;
            this.CBMensual.SelectedIndexChanged += new System.EventHandler(this.CBMensual_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label8.Location = new System.Drawing.Point(307, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 19);
            this.label8.TabIndex = 146;
            this.label8.Text = "Mes:";
            // 
            // CBVista
            // 
            this.CBVista.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.CBVista.BackColor = System.Drawing.Color.PowderBlue;
            this.CBVista.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBVista.Font = new System.Drawing.Font("Microsoft YaHei", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CBVista.ForeColor = System.Drawing.Color.DimGray;
            this.CBVista.FormattingEnabled = true;
            this.CBVista.Items.AddRange(new object[] {
            "N/A",
            "Mensual",
            "Anual"});
            this.CBVista.Location = new System.Drawing.Point(135, 20);
            this.CBVista.Name = "CBVista";
            this.CBVista.Size = new System.Drawing.Size(148, 24);
            this.CBVista.TabIndex = 143;
            this.CBVista.SelectedIndexChanged += new System.EventHandler(this.CBVista_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label7.Location = new System.Drawing.Point(21, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 19);
            this.label7.TabIndex = 144;
            this.label7.Text = "Tipo de vista:";
            // 
            // LBPerfil
            // 
            this.LBPerfil.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.LBPerfil.AutoSize = true;
            this.LBPerfil.BackColor = System.Drawing.Color.Transparent;
            this.LBPerfil.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBPerfil.ForeColor = System.Drawing.Color.DodgerBlue;
            this.LBPerfil.Location = new System.Drawing.Point(124, 107);
            this.LBPerfil.Name = "LBPerfil";
            this.LBPerfil.Size = new System.Drawing.Size(235, 26);
            this.LBPerfil.TabIndex = 136;
            this.LBPerfil.Text = "Número total de pagos";
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel5.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel5.Controls.Add(this.panel11);
            this.panel5.Location = new System.Drawing.Point(196, 393);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(189, 176);
            this.panel5.TabIndex = 135;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.PowderBlue;
            this.panel11.Controls.Add(this.LBEfectivo);
            this.panel11.Location = new System.Drawing.Point(29, 30);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(134, 120);
            this.panel11.TabIndex = 1;
            // 
            // LBEfectivo
            // 
            this.LBEfectivo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBEfectivo.AutoSize = true;
            this.LBEfectivo.BackColor = System.Drawing.Color.Transparent;
            this.LBEfectivo.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBEfectivo.ForeColor = System.Drawing.Color.Navy;
            this.LBEfectivo.Location = new System.Drawing.Point(58, 45);
            this.LBEfectivo.Name = "LBEfectivo";
            this.LBEfectivo.Size = new System.Drawing.Size(0, 26);
            this.LBEfectivo.TabIndex = 108;
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel2.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel2.Controls.Add(this.panel8);
            this.panel2.Location = new System.Drawing.Point(405, 136);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(189, 176);
            this.panel2.TabIndex = 132;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.PowderBlue;
            this.panel8.Controls.Add(this.LBMontoPagado);
            this.panel8.Location = new System.Drawing.Point(29, 32);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(134, 120);
            this.panel8.TabIndex = 1;
            // 
            // LBMontoPagado
            // 
            this.LBMontoPagado.AutoSize = true;
            this.LBMontoPagado.BackColor = System.Drawing.Color.Transparent;
            this.LBMontoPagado.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBMontoPagado.ForeColor = System.Drawing.Color.Navy;
            this.LBMontoPagado.Location = new System.Drawing.Point(34, 46);
            this.LBMontoPagado.Name = "LBMontoPagado";
            this.LBMontoPagado.Size = new System.Drawing.Size(0, 26);
            this.LBMontoPagado.TabIndex = 107;
            // 
            // panel6
            // 
            this.panel6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel6.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel6.Controls.Add(this.panel10);
            this.panel6.Location = new System.Drawing.Point(1, 393);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(189, 176);
            this.panel6.TabIndex = 134;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.PowderBlue;
            this.panel10.Controls.Add(this.LBPSE);
            this.panel10.Location = new System.Drawing.Point(29, 30);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(134, 120);
            this.panel10.TabIndex = 106;
            // 
            // LBPSE
            // 
            this.LBPSE.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBPSE.AutoSize = true;
            this.LBPSE.BackColor = System.Drawing.Color.Transparent;
            this.LBPSE.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBPSE.ForeColor = System.Drawing.Color.Navy;
            this.LBPSE.Location = new System.Drawing.Point(58, 45);
            this.LBPSE.Name = "LBPSE";
            this.LBPSE.Size = new System.Drawing.Size(0, 26);
            this.LBPSE.TabIndex = 107;
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Location = new System.Drawing.Point(153, 136);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 176);
            this.panel1.TabIndex = 131;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.PowderBlue;
            this.panel7.Controls.Add(this.LBTotal);
            this.panel7.Location = new System.Drawing.Point(29, 32);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(134, 120);
            this.panel7.TabIndex = 0;
            // 
            // LBTotal
            // 
            this.LBTotal.AutoSize = true;
            this.LBTotal.BackColor = System.Drawing.Color.Transparent;
            this.LBTotal.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBTotal.ForeColor = System.Drawing.Color.Navy;
            this.LBTotal.Location = new System.Drawing.Point(54, 49);
            this.LBTotal.Name = "LBTotal";
            this.LBTotal.Size = new System.Drawing.Size(0, 26);
            this.LBTotal.TabIndex = 106;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel4.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel4.Controls.Add(this.panel12);
            this.panel4.Location = new System.Drawing.Point(586, 393);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(189, 176);
            this.panel4.TabIndex = 137;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.PowderBlue;
            this.panel12.Controls.Add(this.LBTarjetaDebito);
            this.panel12.Location = new System.Drawing.Point(29, 30);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(134, 120);
            this.panel12.TabIndex = 1;
            // 
            // LBTarjetaDebito
            // 
            this.LBTarjetaDebito.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBTarjetaDebito.AutoSize = true;
            this.LBTarjetaDebito.BackColor = System.Drawing.Color.Transparent;
            this.LBTarjetaDebito.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBTarjetaDebito.ForeColor = System.Drawing.Color.Navy;
            this.LBTarjetaDebito.Location = new System.Drawing.Point(60, 45);
            this.LBTarjetaDebito.Name = "LBTarjetaDebito";
            this.LBTarjetaDebito.Size = new System.Drawing.Size(0, 26);
            this.LBTarjetaDebito.TabIndex = 108;
            // 
            // panel13
            // 
            this.panel13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel13.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel13.Controls.Add(this.panel14);
            this.panel13.Location = new System.Drawing.Point(391, 393);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(189, 176);
            this.panel13.TabIndex = 136;
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.PowderBlue;
            this.panel14.Controls.Add(this.LBTarjetaCredito);
            this.panel14.Location = new System.Drawing.Point(29, 30);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(134, 120);
            this.panel14.TabIndex = 106;
            // 
            // LBTarjetaCredito
            // 
            this.LBTarjetaCredito.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBTarjetaCredito.AutoSize = true;
            this.LBTarjetaCredito.BackColor = System.Drawing.Color.Transparent;
            this.LBTarjetaCredito.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBTarjetaCredito.ForeColor = System.Drawing.Color.Navy;
            this.LBTarjetaCredito.Location = new System.Drawing.Point(58, 45);
            this.LBTarjetaCredito.Name = "LBTarjetaCredito";
            this.LBTarjetaCredito.Size = new System.Drawing.Size(0, 26);
            this.LBTarjetaCredito.TabIndex = 107;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(397, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 26);
            this.label1.TabIndex = 149;
            this.label1.Text = "Monto total pagado";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(12, 328);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(431, 26);
            this.label2.TabIndex = 150;
            this.label2.Text = "- Conteo de los metodos de pago utilizados";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label3.Location = new System.Drawing.Point(75, 364);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 26);
            this.label3.TabIndex = 151;
            this.label3.Text = "PSE";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label4.Location = new System.Drawing.Point(250, 364);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 26);
            this.label4.TabIndex = 152;
            this.label4.Text = "Efectivo";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label6.Location = new System.Drawing.Point(397, 364);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(183, 26);
            this.label6.TabIndex = 153;
            this.label6.Text = "Tarjeta de credito";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label11.Location = new System.Drawing.Point(592, 364);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(178, 26);
            this.label11.TabIndex = 154;
            this.label11.Text = "Tarjeta de debito";
            // 
            // FrmInformesPagos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(777, 648);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.CBAnual);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.CBMensual);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.CBVista);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.LBPerfil);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmInformesPagos";
            this.Text = "FrmInformesPagos";
            this.Load += new System.EventHandler(this.FrmInformesPagos_Load);
            this.panel5.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel12.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CBAnual;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox CBMensual;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox CBVista;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LBPerfil;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Label LBEfectivo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label LBMontoPagado;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label LBPSE;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label LBTotal;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Label LBTarjetaDebito;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Label LBTarjetaCredito;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
    }
}