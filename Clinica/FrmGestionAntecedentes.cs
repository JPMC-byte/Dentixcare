using BLL;
using ENTITY;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmGestionAntecedentes : Form
    {
        Persona usuarioActual;
        ServicioDiagnostico servisDiag = new ServicioDiagnostico();
        ServicioCita servisCita = new ServicioCita();
        ServicioPaciente servisPaciente = new ServicioPaciente();
        Validaciones vali = new Validaciones();
        public FrmGestionAntecedentes(Persona persona)
        {
            InitializeComponent();
            usuarioActual = persona;
            determinarUsuario(usuarioActual);
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmGestionAntecedentes_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Parent == null)
            {
                moverVentana();
            }
        }

        private void FrmGestionAntecedentes_Load(object sender, EventArgs e)
        {
            cargarDiagnosticos();
        }

        private void btnInformacion_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            abrirInformacion();
        }

        private void btnAsignarTratamiento_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            asignarTratamiento();
        }

        private void btnTratamientosRelacion_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            verTratamientosRelacionados();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarExistenteRelacion())
            {
                return;
            }
            if (confirmar())
            {
                cancelarDiagnostico();
            }
        }

        private void btnActualizarDiagnostico_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarExistenteRelacion())
            {
                return;
            }
            actualizarDiagnostico();
        }

        private void btnActualizarRegistro_Click(object sender, EventArgs e)
        {
            actualizar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(11);
        }

        private void CBFiltrarFecha_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorFecha();
        }

        private void CBFiltrarPorPaciente_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorPaciente();
        }

        private void cargarDiagnosticos(DateTime? Fecha = null, string cedulaPaciente = null)
        {
            List<Diagnostico> diagnosticos = new List<Diagnostico>();

            if (usuarioActual is Paciente)
            {
                cedulaPaciente = usuarioActual.Cedula;
                diagnosticos = servisDiag.cargarPorCedula(cedulaPaciente);

                if (Fecha.HasValue && validarFiltroFecha(CBFiltrarFecha.Checked, Fecha.Value))
                {
                    diagnosticos = servisDiag.cargarPorFecha(Fecha.Value);
                }
            }
            else 
            {
                diagnosticos = servisDiag.obtenerTodos();

                if (Fecha.HasValue && validarFiltroFecha(CBFiltrarFecha.Checked, Fecha.Value))
                {
                    diagnosticos = servisDiag.cargarPorFecha(Fecha.Value);
                }
                if (validarFiltroPaciente(CBFiltrarPorPaciente.Checked, cedulaPaciente))
                {
                    diagnosticos = servisDiag.cargarPorCedula(cedulaPaciente);
                }
                if(Fecha.HasValue && validarFiltroPaciente(CBFiltrarPorPaciente.Checked, cedulaPaciente) && validarFiltroFecha(CBFiltrarFecha.Checked, Fecha.Value))
                {
                    diagnosticos = servisDiag.cargarPorFiltros(Fecha.Value, cedulaPaciente);
                }
            }
            DGVDiagnostico.DataSource = diagnosticos;
        }

        void moverVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        public void determinarUsuario(Persona persona)
        {
            if (usuarioActual is Paciente)
            {
                CBFiltrarPorPaciente.Enabled = false;
                txtCedulaPaciente.Enabled = false;
                btnEliminar.Enabled = false;
                btnActualizarDiagnostico.Enabled = false;
                btnAsignarTratamiento.Enabled = false;
                LBFiltrarPorPaciente.Enabled = false;
            }
        }

        bool verificar()
        {
            if (DGVDiagnostico.SelectedRows.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un diagnostico de la lista para realizar dicha acción", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        public bool validarFiltroFecha(bool activo, DateTime fecha)
        {
            return vali.validarFiltroFecha(activo, fecha);
        }

        public bool validarFiltroPaciente(bool activo, string texto)
        {
            return vali.validarFiltroPaciente(activo, texto);
        }

        bool validarNumeros(KeyPressEventArgs e)
        {
            if (!vali.validarNumeros(e))
            {
                return false;
            }
            return true;
        }

        void actualizar()
        {
            DateTime? fechaSeleccionada = CBFiltrarFecha.Checked ? (DateTime?)DTFiltroFecha.Value.Date : null;
            string cedulaPaciente = txtCedulaPaciente.Text != "CEDULA DEL PACIENTE" ? txtCedulaPaciente.Text : null;

            cargarDiagnosticos(fechaSeleccionada, cedulaPaciente);

            if (CBFiltrarPorPaciente.Checked && !validarFiltroPaciente(CBFiltrarPorPaciente.Checked, cedulaPaciente))
            {
                MessageBox.Show("La cédula del paciente no existe en el registro", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (CBFiltrarFecha.Checked && !validarFiltroFecha(CBFiltrarFecha.Checked, DTFiltroFecha.Value.Date))
            {
                MessageBox.Show("No es posible filtrar por una fecha futura", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void accionarFiltroPorFecha()
        {
            if (CBFiltrarFecha.Checked)
            {
                DTFiltroFecha.Enabled = true;
            }
            else
            {
                DTFiltroFecha.Enabled = false;
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

        public Paciente pacienteSeleccionado()
        {
            var cedulaPaciente = DGVDiagnostico.SelectedRows[0].Cells["CedulaPaciente"].Value.ToString();

            Paciente pacienteSeleccionado = servisPaciente.obtenerPorCodigo(cedulaPaciente);
            return pacienteSeleccionado;
        }

        public Cita citaSeleccionada()
        {
            var codigoCita = DGVDiagnostico.SelectedRows[0].Cells["CodigoCita"].Value.ToString();

            Cita citaSeleccionada = servisCita.obtenerPorCodigo(codigoCita);
            return citaSeleccionada;
        }

        public Diagnostico diagnosticoSeleccionado()
        {
            var codigoDiag = DGVDiagnostico.SelectedRows[0].Cells["Codigo"].Value.ToString();
            Diagnostico diagnosticoSeleccionado = servisDiag.obtenerPorCodigo(codigoDiag);
            return diagnosticoSeleccionado;
        }

        void abrirInformacion()
        {
            Diagnostico diagnostico = diagnosticoSeleccionado();
            FrmInfoDiagnostico F = new FrmInfoDiagnostico(diagnostico);
            F.Show();
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

        void cerrar()
        {
            this.Close();
        }

        void asignarTratamiento()
        {
            Diagnostico diagnostico = diagnosticoSeleccionado();
            Paciente paciente = pacienteSeleccionado();
            FrmSeleccionTratamiento F = new FrmSeleccionTratamiento(diagnostico, paciente);
            F.Show();
        }

        void verTratamientosRelacionados()
        {
            Diagnostico diagnostico = diagnosticoSeleccionado();
            FrmGestionTratamientos F = new FrmGestionTratamientos(diagnostico);
            F.Show();
        }

        void cancelarDiagnostico()
        {
            Diagnostico diagnostico = diagnosticoSeleccionado();
            servisDiag.eliminar(diagnostico);
            MessageBox.Show("Registro eliminado con exito");
        }

        bool confirmar()
        {
            return MessageBox.Show("¿Está seguro que desea eliminar dicho registro?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        bool validarExistenteRelacion()
        {
            Diagnostico diagnostico = diagnosticoSeleccionado();
            if (vali.validarExistenteFactura(diagnostico.Codigo))
            {
                MessageBox.Show("Error - No es posible alterar un diagnostico con tratamientos asignados", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        void actualizarDiagnostico()
        {
            Paciente paciente = pacienteSeleccionado();
            Cita cita = citaSeleccionada();
            Diagnostico diagnostico = diagnosticoSeleccionado();
            FrmActualizarDiagnostico F = new FrmActualizarDiagnostico(cita, paciente, diagnostico);
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
