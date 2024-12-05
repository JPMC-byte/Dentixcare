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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmGestionPaciente : Form
    {
        ServicioPaciente servisPaciente = new ServicioPaciente();
        Validaciones vali = new Validaciones();
        public FrmGestionPaciente()
        {
            InitializeComponent();
        }

        private void FrmGestionPaciente_Load(object sender, EventArgs e)
        {
            cargarPacientes();
        }

        private void btnInformacion_Click(object sender, EventArgs e)
        {
            if (!verificar())
            {
                return;
            }
            abrirInformacion();
        }

        private void btnAntecedentes_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarAntecedentes()) { return; }
            mostrarAntecedentes();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(14);
        }

        private void CBFiltrarPorPaciente_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorPaciente();
            actualizar();
        }

        private void txtCedulaPaciente_TextChanged(object sender, EventArgs e)
        {
            actualizar();
        }

        void cargarPacientes(string cedulaPaciente = null)
        {
            var Pacientes = servisPaciente.obtenerTodos();

            if (validarFiltroPaciente(CBFiltrarPorPaciente.Checked, cedulaPaciente))
            {
                Pacientes = servisPaciente.cargarPorCodigo(cedulaPaciente);
            }
            DGVPaciente.DataSource = Pacientes;
        }

        public Paciente pacienteSeleccionado()
        {
            var cedulaPaciente = DGVPaciente.SelectedRows[0].Cells["Cedula"].Value.ToString();

            Paciente pacienteSeleccionado = servisPaciente.obtenerPorCodigo(cedulaPaciente);
            return pacienteSeleccionado;
        }

        bool verificar()
        {
            if (DGVPaciente.SelectedRows.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un paciente de la lista para realizar dicha acción", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        bool validarAntecedentes()
        {
            Paciente paciente = pacienteSeleccionado();
            if (!vali.validarAntedecentes(paciente))
            {
                MessageBox.Show("El paciente no presenta antecedentes previos registrados", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        void abrirInformacion()
        {
            Paciente paciente = pacienteSeleccionado();
            FrmPerfil F = new FrmPerfil(paciente);
            F.Show();
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

        void cerrar()
        {
            this.Close();
        }

        void actualizar()
        {
            string cedulaPaciente = txtCedulaPaciente.Text != "CEDULA DEL PACIENTE" ? txtCedulaPaciente.Text : string.Empty;
            cargarPacientes(cedulaPaciente);
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

        void mostrarAntecedentes()
        {
            Paciente paciente = pacienteSeleccionado();
            FrmGestionAntecedentes F = new FrmGestionAntecedentes(paciente);
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
