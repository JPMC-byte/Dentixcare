namespace GUI
{
    partial class FrmGestionFactura
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnActualizarRegistro = new System.Windows.Forms.Button();
            this.btnInformacion = new System.Windows.Forms.Button();
            this.btnRealizarPago = new System.Windows.Forms.Button();
            this.btnTratamientosRelacionados = new System.Windows.Forms.Button();
            this.LBTitulo = new System.Windows.Forms.Label();
            this.DGVFacturas = new System.Windows.Forms.DataGridView();
            this.btnVerPagos = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.PictureBox();
            this.IconDudas = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.DGVFacturas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconDudas)).BeginInit();
            this.SuspendLayout();
            // 
            // btnActualizarRegistro
            // 
            this.btnActualizarRegistro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizarRegistro.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActualizarRegistro.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizarRegistro.FlatAppearance.BorderSize = 0;
            this.btnActualizarRegistro.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarRegistro.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarRegistro.ForeColor = System.Drawing.Color.Black;
            this.btnActualizarRegistro.Location = new System.Drawing.Point(445, 22);
            this.btnActualizarRegistro.Name = "btnActualizarRegistro";
            this.btnActualizarRegistro.Size = new System.Drawing.Size(198, 50);
            this.btnActualizarRegistro.TabIndex = 120;
            this.btnActualizarRegistro.Text = "Actualizar registro";
            this.btnActualizarRegistro.UseVisualStyleBackColor = false;
            this.btnActualizarRegistro.Click += new System.EventHandler(this.btnActualizarRegistro_Click);
            // 
            // btnInformacion
            // 
            this.btnInformacion.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnInformacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnInformacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInformacion.FlatAppearance.BorderSize = 0;
            this.btnInformacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformacion.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformacion.ForeColor = System.Drawing.Color.Black;
            this.btnInformacion.Location = new System.Drawing.Point(19, 626);
            this.btnInformacion.Name = "btnInformacion";
            this.btnInformacion.Size = new System.Drawing.Size(169, 50);
            this.btnInformacion.TabIndex = 119;
            this.btnInformacion.Text = "Más información";
            this.btnInformacion.UseVisualStyleBackColor = false;
            this.btnInformacion.Click += new System.EventHandler(this.btnInformacion_Click);
            // 
            // btnRealizarPago
            // 
            this.btnRealizarPago.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnRealizarPago.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRealizarPago.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRealizarPago.FlatAppearance.BorderSize = 0;
            this.btnRealizarPago.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRealizarPago.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRealizarPago.ForeColor = System.Drawing.Color.Black;
            this.btnRealizarPago.Location = new System.Drawing.Point(369, 626);
            this.btnRealizarPago.Name = "btnRealizarPago";
            this.btnRealizarPago.Size = new System.Drawing.Size(169, 50);
            this.btnRealizarPago.TabIndex = 118;
            this.btnRealizarPago.Text = "Realizar pago";
            this.btnRealizarPago.UseVisualStyleBackColor = false;
            this.btnRealizarPago.Click += new System.EventHandler(this.btnRealizarPago_Click);
            // 
            // btnTratamientosRelacionados
            // 
            this.btnTratamientosRelacionados.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnTratamientosRelacionados.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTratamientosRelacionados.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTratamientosRelacionados.FlatAppearance.BorderSize = 0;
            this.btnTratamientosRelacionados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTratamientosRelacionados.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTratamientosRelacionados.ForeColor = System.Drawing.Color.Black;
            this.btnTratamientosRelacionados.Location = new System.Drawing.Point(194, 626);
            this.btnTratamientosRelacionados.Name = "btnTratamientosRelacionados";
            this.btnTratamientosRelacionados.Size = new System.Drawing.Size(169, 50);
            this.btnTratamientosRelacionados.TabIndex = 116;
            this.btnTratamientosRelacionados.Text = "Ver tratamientos relacionados";
            this.btnTratamientosRelacionados.UseVisualStyleBackColor = false;
            this.btnTratamientosRelacionados.Click += new System.EventHandler(this.btnTratamientosRelacionados_Click);
            // 
            // LBTitulo
            // 
            this.LBTitulo.AutoSize = true;
            this.LBTitulo.BackColor = System.Drawing.Color.Transparent;
            this.LBTitulo.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBTitulo.ForeColor = System.Drawing.Color.Black;
            this.LBTitulo.Location = new System.Drawing.Point(12, 30);
            this.LBTitulo.Name = "LBTitulo";
            this.LBTitulo.Size = new System.Drawing.Size(301, 39);
            this.LBTitulo.TabIndex = 114;
            this.LBTitulo.Text = "Gestion de facturas";
            // 
            // DGVFacturas
            // 
            this.DGVFacturas.AllowUserToAddRows = false;
            this.DGVFacturas.AllowUserToDeleteRows = false;
            this.DGVFacturas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVFacturas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVFacturas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVFacturas.BackgroundColor = System.Drawing.Color.SkyBlue;
            this.DGVFacturas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVFacturas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVFacturas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DGVFacturas.ColumnHeadersHeight = 30;
            this.DGVFacturas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGVFacturas.EnableHeadersVisualStyles = false;
            this.DGVFacturas.Location = new System.Drawing.Point(19, 85);
            this.DGVFacturas.Name = "DGVFacturas";
            this.DGVFacturas.ReadOnly = true;
            this.DGVFacturas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVFacturas.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.DGVFacturas.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.DGVFacturas.Size = new System.Drawing.Size(709, 535);
            this.DGVFacturas.TabIndex = 115;
            // 
            // btnVerPagos
            // 
            this.btnVerPagos.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnVerPagos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnVerPagos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerPagos.FlatAppearance.BorderSize = 0;
            this.btnVerPagos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerPagos.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerPagos.ForeColor = System.Drawing.Color.Black;
            this.btnVerPagos.Location = new System.Drawing.Point(544, 626);
            this.btnVerPagos.Name = "btnVerPagos";
            this.btnVerPagos.Size = new System.Drawing.Size(169, 50);
            this.btnVerPagos.TabIndex = 122;
            this.btnVerPagos.Text = "Ver pagos relacionados";
            this.btnVerPagos.UseVisualStyleBackColor = false;
            this.btnVerPagos.Click += new System.EventHandler(this.btnVerPagos_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.Image = global::GUI.Properties.Resources.Cerrar;
            this.btnCerrar.Location = new System.Drawing.Point(745, 22);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(20, 20);
            this.btnCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCerrar.TabIndex = 117;
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // IconDudas
            // 
            this.IconDudas.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IconDudas.Cursor = System.Windows.Forms.Cursors.Hand;
            this.IconDudas.Image = global::GUI.Properties.Resources.IconQuestion;
            this.IconDudas.Location = new System.Drawing.Point(719, 22);
            this.IconDudas.Name = "IconDudas";
            this.IconDudas.Size = new System.Drawing.Size(20, 20);
            this.IconDudas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.IconDudas.TabIndex = 123;
            this.IconDudas.TabStop = false;
            this.IconDudas.Click += new System.EventHandler(this.IconDudas_Click);
            // 
            // FrmGestionFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(777, 699);
            this.Controls.Add(this.IconDudas);
            this.Controls.Add(this.btnVerPagos);
            this.Controls.Add(this.btnActualizarRegistro);
            this.Controls.Add(this.btnInformacion);
            this.Controls.Add(this.btnRealizarPago);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnTratamientosRelacionados);
            this.Controls.Add(this.LBTitulo);
            this.Controls.Add(this.DGVFacturas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmGestionFactura";
            this.Text = "FrmGestionFactura";
            this.Load += new System.EventHandler(this.FrmGestionFactura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVFacturas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IconDudas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnActualizarRegistro;
        private System.Windows.Forms.Button btnInformacion;
        private System.Windows.Forms.Button btnRealizarPago;
        private System.Windows.Forms.PictureBox btnCerrar;
        private System.Windows.Forms.Button btnTratamientosRelacionados;
        private System.Windows.Forms.Label LBTitulo;
        private System.Windows.Forms.DataGridView DGVFacturas;
        private System.Windows.Forms.Button btnVerPagos;
        private System.Windows.Forms.PictureBox IconDudas;
    }
}