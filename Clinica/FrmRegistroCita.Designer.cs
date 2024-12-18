﻿namespace GUI
{
    partial class FrmRegistroCita
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
            this.LBTitulo = new System.Windows.Forms.Label();
            this.DGVCitas = new System.Windows.Forms.DataGridView();
            this.btnInformacion = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnActualizarRazon = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.PictureBox();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.CBEstado = new System.Windows.Forms.ComboBox();
            this.LBFiltro = new System.Windows.Forms.Label();
            this.CBFiltrarEstado = new System.Windows.Forms.CheckBox();
            this.btnVerInfoOrtodoncista = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.DGVCitas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).BeginInit();
            this.SuspendLayout();
            // 
            // LBTitulo
            // 
            this.LBTitulo.AutoSize = true;
            this.LBTitulo.BackColor = System.Drawing.Color.Transparent;
            this.LBTitulo.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBTitulo.Location = new System.Drawing.Point(12, 20);
            this.LBTitulo.Name = "LBTitulo";
            this.LBTitulo.Size = new System.Drawing.Size(260, 39);
            this.LBTitulo.TabIndex = 7;
            this.LBTitulo.Text = "Registro de citas";
            // 
            // DGVCitas
            // 
            this.DGVCitas.AllowUserToAddRows = false;
            this.DGVCitas.AllowUserToDeleteRows = false;
            this.DGVCitas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGVCitas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVCitas.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVCitas.BackgroundColor = System.Drawing.Color.SkyBlue;
            this.DGVCitas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGVCitas.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.HotTrack;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVCitas.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DGVCitas.ColumnHeadersHeight = 30;
            this.DGVCitas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGVCitas.EnableHeadersVisualStyles = false;
            this.DGVCitas.Location = new System.Drawing.Point(19, 75);
            this.DGVCitas.Name = "DGVCitas";
            this.DGVCitas.ReadOnly = true;
            this.DGVCitas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGVCitas.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.DeepSkyBlue;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.DGVCitas.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.DGVCitas.Size = new System.Drawing.Size(709, 535);
            this.DGVCitas.TabIndex = 8;
            // 
            // btnInformacion
            // 
            this.btnInformacion.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnInformacion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnInformacion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInformacion.FlatAppearance.BorderSize = 0;
            this.btnInformacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInformacion.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInformacion.Location = new System.Drawing.Point(23, 616);
            this.btnInformacion.Name = "btnInformacion";
            this.btnInformacion.Size = new System.Drawing.Size(169, 50);
            this.btnInformacion.TabIndex = 38;
            this.btnInformacion.Text = "Más información";
            this.btnInformacion.UseVisualStyleBackColor = false;
            this.btnInformacion.Click += new System.EventHandler(this.btnInformacion_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancelar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancelar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelar.FlatAppearance.BorderSize = 0;
            this.btnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(373, 616);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(169, 50);
            this.btnCancelar.TabIndex = 63;
            this.btnCancelar.Text = "Cancelar cita";
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnActualizarRazon
            // 
            this.btnActualizarRazon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnActualizarRazon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActualizarRazon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizarRazon.FlatAppearance.BorderSize = 0;
            this.btnActualizarRazon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarRazon.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizarRazon.Location = new System.Drawing.Point(548, 616);
            this.btnActualizarRazon.Name = "btnActualizarRazon";
            this.btnActualizarRazon.Size = new System.Drawing.Size(169, 50);
            this.btnActualizarRazon.TabIndex = 64;
            this.btnActualizarRazon.Text = "Cambiar razon de cita";
            this.btnActualizarRazon.UseVisualStyleBackColor = false;
            this.btnActualizarRazon.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnCerrar
            // 
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCerrar.Image = global::GUI.Properties.Resources.Cerrar;
            this.btnCerrar.Location = new System.Drawing.Point(745, 12);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(20, 20);
            this.btnCerrar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCerrar.TabIndex = 62;
            this.btnCerrar.TabStop = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnActualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizar.FlatAppearance.BorderSize = 0;
            this.btnActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizar.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizar.Location = new System.Drawing.Point(280, 12);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(187, 50);
            this.btnActualizar.TabIndex = 65;
            this.btnActualizar.Text = "Actualizar registro";
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click_1);
            // 
            // CBEstado
            // 
            this.CBEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBEstado.Enabled = false;
            this.CBEstado.ForeColor = System.Drawing.Color.DimGray;
            this.CBEstado.FormattingEnabled = true;
            this.CBEstado.Items.AddRange(new object[] {
            "N/A",
            "Solicitada",
            "Pendiente",
            "Finalizada",
            "Cancelada"});
            this.CBEstado.Location = new System.Drawing.Point(558, 34);
            this.CBEstado.Name = "CBEstado";
            this.CBEstado.Size = new System.Drawing.Size(121, 21);
            this.CBEstado.TabIndex = 66;
            this.CBEstado.SelectedIndexChanged += new System.EventHandler(this.CBEstado_SelectedIndexChanged);
            // 
            // LBFiltro
            // 
            this.LBFiltro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LBFiltro.AutoSize = true;
            this.LBFiltro.BackColor = System.Drawing.Color.Transparent;
            this.LBFiltro.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBFiltro.Location = new System.Drawing.Point(554, 12);
            this.LBFiltro.Name = "LBFiltro";
            this.LBFiltro.Size = new System.Drawing.Size(137, 19);
            this.LBFiltro.TabIndex = 67;
            this.LBFiltro.Text = "Filtrar por estado";
            // 
            // CBFiltrarEstado
            // 
            this.CBFiltrarEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CBFiltrarEstado.AutoSize = true;
            this.CBFiltrarEstado.Location = new System.Drawing.Point(536, 27);
            this.CBFiltrarEstado.Name = "CBFiltrarEstado";
            this.CBFiltrarEstado.Size = new System.Drawing.Size(15, 14);
            this.CBFiltrarEstado.TabIndex = 80;
            this.CBFiltrarEstado.UseVisualStyleBackColor = true;
            this.CBFiltrarEstado.CheckedChanged += new System.EventHandler(this.CBFiltrarEstado_CheckedChanged);
            // 
            // btnVerInfoOrtodoncista
            // 
            this.btnVerInfoOrtodoncista.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnVerInfoOrtodoncista.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnVerInfoOrtodoncista.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVerInfoOrtodoncista.FlatAppearance.BorderSize = 0;
            this.btnVerInfoOrtodoncista.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVerInfoOrtodoncista.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVerInfoOrtodoncista.Location = new System.Drawing.Point(198, 616);
            this.btnVerInfoOrtodoncista.Name = "btnVerInfoOrtodoncista";
            this.btnVerInfoOrtodoncista.Size = new System.Drawing.Size(169, 50);
            this.btnVerInfoOrtodoncista.TabIndex = 81;
            this.btnVerInfoOrtodoncista.Text = "Ver perfil del ortodoncista";
            this.btnVerInfoOrtodoncista.UseVisualStyleBackColor = false;
            this.btnVerInfoOrtodoncista.Click += new System.EventHandler(this.btnVerInfoOrtodoncista_Click);
            // 
            // FrmRegistroCita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(777, 699);
            this.Controls.Add(this.btnVerInfoOrtodoncista);
            this.Controls.Add(this.CBFiltrarEstado);
            this.Controls.Add(this.LBFiltro);
            this.Controls.Add(this.CBEstado);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnActualizarRazon);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnInformacion);
            this.Controls.Add(this.LBTitulo);
            this.Controls.Add(this.DGVCitas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmRegistroCita";
            this.Text = "FrmRegistroCita";
            this.Load += new System.EventHandler(this.FrmRegistroCita_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGVCitas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCerrar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LBTitulo;
        private System.Windows.Forms.DataGridView DGVCitas;
        private System.Windows.Forms.Button btnInformacion;
        private System.Windows.Forms.PictureBox btnCerrar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnActualizarRazon;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.ComboBox CBEstado;
        private System.Windows.Forms.Label LBFiltro;
        private System.Windows.Forms.CheckBox CBFiltrarEstado;
        private System.Windows.Forms.Button btnVerInfoOrtodoncista;
    }
}