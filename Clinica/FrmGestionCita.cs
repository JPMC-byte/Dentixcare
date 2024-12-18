﻿using BLL;
using ENTITY;
using Logica;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmGestionCita : Form
    {
        ServicioPaciente servicioPaciente = new ServicioPaciente();
        ServicioCita servicioCita = new ServicioCita();
        Persona usuarioActual;
        Validaciones vali = new Validaciones();

        public FrmGestionCita(Persona persona)
        {
            InitializeComponent();
            usuarioActual = persona;
        }

        private void FrmGestionCita_Load(object sender, EventArgs e)
        {
            cargarCitas();
            CBEstado.SelectedIndex = 0;
        }

        private void btnAtender_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarEstado("Atender")) { return; }
            if (confirmarAsignacion()) { atenderCita(); actualizar(); }
        }

        private void btnDiagnostico_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarEstado("Diagnosticar")) { return; }
            abrirRealizarDiagnostico();
        }

        private void btnInformacion_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            abrirInformacion();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(13);
        }

        private void CBFiltrarEstado_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorEstado();
            actualizar();
        }

        private void CBFiltrarPorPaciente_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorPaciente();
            actualizar();
        }

        private void CBEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizar();
        }

        private void txtCedulaPaciente_TextChanged(object sender, EventArgs e)
        {
            actualizar();
        }

        private void cargarCitas(string estado = null, string cedulaPaciente = null)
        {
            var citas = servicioCita.obtenerTodos();

            if (validarFiltroEstado(CBFiltrarEstado.Checked, estado))
            {
                citas = servicioCita.cargarPorEstado(estado);
            }
            if (validarFiltroPaciente(CBFiltrarPorPaciente.Checked, cedulaPaciente))
            {
                citas = servicioCita.cargarPorCodigo(cedulaPaciente);
            }
            if (validarFiltroEstado(CBFiltrarEstado.Checked, estado) && validarFiltroPaciente(CBFiltrarPorPaciente.Checked, cedulaPaciente))
            {
                citas = servicioCita.cargarPorFiltrosGui(estado,cedulaPaciente);
            }
            DGVCitas.DataSource = citas; 
        }

        void actualizar()
        {
            string estadoSeleccionado = CBEstado.SelectedItem?.ToString();
            string cedulaPaciente = txtCedulaPaciente.Text != "CEDULA DEL PACIENTE" ? txtCedulaPaciente.Text : string.Empty;
            cargarCitas(estadoSeleccionado, cedulaPaciente);
        }

        public Paciente pacienteSeleccionado()
        {
            var cedulaPaciente = DGVCitas.SelectedRows[0].Cells["CodigoPaciente"].Value.ToString();

            Paciente pacienteSeleccionado = servicioPaciente.obtenerPorCodigo(cedulaPaciente);
            return pacienteSeleccionado;
        }

        public Cita citaSeleccionada()
        {
            var codigoCita = DGVCitas.SelectedRows[0].Cells["Codigo"].Value.ToString();

            Cita citaSeleccionada = servicioCita.obtenerPorCodigo(codigoCita);
            return citaSeleccionada;
        }

        bool verificar()
        {
            if (DGVCitas.SelectedRows.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una cita de la lista para ver más información.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        bool validarEstado(string Contexto)
        {
            Cita cita = citaSeleccionada();

            switch (cita.Estado)
            {
                case "Solicitada":
                    {
                        if(Contexto == "Diagnosticar") { return validarSolicitada(cita.Estado); }
                        else { return true; }
                    }
                case "Pendiente":
                    {
                        return validarAtendida(cita.Estado);
                    }
                case "Finalizada": 
                    {
                        return validarAtendida(cita.Estado);
                    }
                case "Cancelada":
                    {
                        return validarCancelada(cita.Estado);
                    }
            }
            return true;
        }

        bool validarSolicitada(string texto)
        {
            if (vali.validarAtendidaPaciente(texto))
            {
                MessageBox.Show("Error - No es posible realizar un diagnóstico si la cita no ha sido atendida.", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool validarAtendida(string texto)
        {
            if (!vali.validarAtendidaOrtodoncista(texto))
            {
                MessageBox.Show("Error - No es posible alterar una cita que ya ha finalizado", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool validarCancelada(string texto)
        {
            if (!vali.validarCancelada(texto))
            {
                MessageBox.Show("Error - No es posible alterar una cita que ha sido cancelada", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public bool validarFiltroEstado(bool activo, string texto)
        {
            return vali.validarFiltroEstado(activo,texto);
        }

        public bool validarFiltroPaciente(bool activo, string texto)
        {
            return vali.validarFiltroPaciente(activo,texto);
        }

        bool validarNumeros(KeyPressEventArgs e)
        {
            if (!vali.validarNumeros(e))
            {
                return false;
            }
            return true;
        }

        void atenderCita()
        {
            Cita cita = citaSeleccionada();
            servicioCita.actualizarAtendida(cita, usuarioActual.Cedula, "Pendiente");
            MessageBox.Show("Proceso de asignación exitoso");
        }

        bool confirmarAsignacion()
        {
            if (MessageBox.Show("¿Esta seguro que desea atender dicha cita?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                return true;
            return false;
        }

        void accionarFiltroPorEstado()
        {
            if (CBFiltrarEstado.Checked)
            {
                CBEstado.Enabled = true;
            }
            else 
            {
                CBEstado.Enabled = false;
            }
        }

        void accionarFiltroPorPaciente()
        {
            if (CBFiltrarPorPaciente.Checked)
            {
               txtCedulaPaciente.Enabled = true;
            }
            else
            {
                txtCedulaPaciente.Enabled = false;
            }
        }

        void cerrar()
        {
            this.Close();
        }

        void eventoEntrar(TextBox textbox, string nombre)
        {
            if (textbox.Text == nombre)
            {
                textbox.Text = "";
                textbox.ForeColor = Color.Black;
            }
        }

        void eventoSalir(TextBox textbox, string nombre)
        {
            if (textbox.Text == "")
            {
                textbox.Text = nombre;
                textbox.ForeColor = Color.DimGray;
            }
        }

        void abrirInformacion()
        {
            Cita cita = citaSeleccionada();
            FrmInformacion F = new FrmInformacion(cita);
            F.Show();
        }

        void abrirRealizarDiagnostico()
        {
            Cita cita = citaSeleccionada();
            Paciente paciente = pacienteSeleccionado();
            FrmRealizarDiagnostico F = new FrmRealizarDiagnostico(cita, paciente);
            F.FormClosed += (s, args) => actualizar();
            F.Show();
        }

        void abrirManualUsuario(int pagina)
        {
            string tempPath = Path.GetTempPath();
            string pdfPath = Path.Combine(tempPath, "ManualDeUsuario.pdf");
            using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GUI.Recursos.ManualDeUsuario.pdf"))
            {
                if (resourceStream != null)
                {
                    using (var fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
                    {
                        resourceStream.CopyTo(fileStream);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontró el archivo PDF incrustado.");
                    return;
                }
            }
            Process pdfProcess = new Process();
            pdfProcess.StartInfo.FileName = "Acrobat.exe";
            pdfProcess.StartInfo.Arguments = $"/A \"page={pagina}\" \"{pdfPath}\"";
            pdfProcess.Start();

            pdfProcess.Exited += (s, e) =>
            {
                if (File.Exists(pdfPath))
                {
                    File.Delete(pdfPath);
                }
            };
            pdfProcess.EnableRaisingEvents = true;
        }

        private void txtCedulaPaciente_Enter(object sender, EventArgs e) { eventoEntrar(txtCedulaPaciente, "CEDULA DEL PACIENTE"); }
        private void txtCedulaPaciente_Leave(object sender, EventArgs e) { eventoSalir(txtCedulaPaciente, "CEDULA DEL PACIENTE"); }

        private void txtCedulaPaciente_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
    }

}
