using BLL;
using ENTITY;
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
    public partial class FrmActualizarCita : Form
    {
        ServicioCita serviscita = new ServicioCita();
        Cita citaActual;
        
        public FrmActualizarCita(Cita cita)
        {
            InitializeComponent();
            citaActual = cita;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmActualizarCita_MouseDown(object sender, MouseEventArgs e)
        {
            moverMouse();
        }

        private void FrmActualizarCita_Load(object sender, EventArgs e)
        {
            cargarDatos(citaActual);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!verificar())
            {
                return;
            }
            if (confirmar())
            {
                actualizar();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(9);
        }

        private void txtRazonCita_Enter(object sender, EventArgs e)
        {
            eventoEnter();
        }

        private void txtRazonCita_Leave(object sender, EventArgs e)
        {
            eventoSalir();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        void moverMouse()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        void eventoEnter()
        {
            if (txtRazonCita.Text == "RAZON DE CITA")
            {
                txtRazonCita.Text = "";
                txtRazonCita.ForeColor = Color.Black;
            }
        }

        void eventoSalir()
        {
            if (txtRazonCita.Text == "")
            {
                txtRazonCita.Text = "RAZON DE CITA";
                txtRazonCita.ForeColor = Color.DimGray;
            }
        }

        void cerrar()
        {
            this.Close();
        }

        private void cargarDatos(Cita cita)
        {
            txtHora.Text = cita.Hora_Cita.ToString();
            txtFecha.Text = cita.Fecha_Cita.ToString("dd/MM/yyyy");
        }

        void actualizar()
        {
            string razonCita = txtRazonCita.Text;
            serviscita.actualizarRazon(citaActual, razonCita);
            MessageBox.Show("Proceso de modificación exitoso");
            limpiar();
        }

        bool confirmar()
        {
            return MessageBox.Show("¿Está seguro que desea actualizar dicha cita?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        bool verificar()
        {
            if (txtRazonCita.Text == "RAZON DE CITA")
            {
                MessageBox.Show("Por favor, rellene/complete los campos vacios");
                return false;
            }
            return true;
        }

        private void limpiar()
        {
            txtRazonCita.Text = "RAZON DE CITA";
            txtRazonCita.ForeColor = Color.DimGray;
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
    }
}
