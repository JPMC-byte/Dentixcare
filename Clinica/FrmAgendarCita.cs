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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmAgendarCita : Form
    {
        Persona UsuarioActual;
        ServicioCita servisCita = new ServicioCita();
        ServicioConsultorio servisconsulto = new ServicioConsultorio();
        Validaciones vali = new Validaciones();
        public FrmAgendarCita(Persona persona)
        {
            InitializeComponent();
            UsuarioActual = persona;
        }

        private void btnRegistrado_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarFecha() || !validarFechaFutura() || !validarHoraDisponible())
            {
                return;
            }
            registrar();
        }

        private void txtRazonCita_Enter(object sender, EventArgs e)
        {
            eventoEntrar();
        }

        private void txtRazonCita_Leave(object sender, EventArgs e)
        {
            eventoSalir();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(8);
        }

        private void registrar()
        {
            Cita cita = new Cita();
            Consultorio consultorio = new Consultorio();
            consultorio = servisconsulto.cargarConsultorio("P101");

            cita.Codigo = servisCita.generarCodigo();
            cita.CodigoConsultorio = consultorio.Codigo;
            cita.CodigoPaciente = UsuarioActual.Cedula;
            cita.CodigoOrtodoncista = null;
            cita.Fecha_Cita = DTFecha_Nacimiento.Value.Date;
            cita.Fecha_Creacion = DateTime.Today.Date;
            cita.Hora_Cita = DTHora.Value.TimeOfDay;
            cita.Razon_Cita = txtRazonCita.Text;
            cita.Estado = "Solicitada";
            cita.RecordatorioCita = false;
            servisCita.guardar(cita);
            MessageBox.Show("Proceso de registro exitoso");
            limpiar();
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

        bool validarFecha()
        {
            DateTime fechaCita = DTFecha_Nacimiento.Value.Date;
            TimeSpan horaCita = DTHora.Value.TimeOfDay;
            if (!vali.validarHorario(horaCita, fechaCita))
            {
                MessageBox.Show("Error - Ya hay una cita existente en el horario establecido.");
                return false;
            }
            return true;
        }

        bool validarFechaFutura()
        {
            DateTime fechaCita = DTFecha_Nacimiento.Value.Date;
            if (!vali.validarFechaFutura(fechaCita))
            {
                MessageBox.Show("Error - No es posible agendar una cita en días pasados.");
                return false;
            }
            return true;
        }

        bool validarHoraDisponible()
        {
            TimeSpan horaCita = DTHora.Value.TimeOfDay;
            if (!vali.validarAperturaCierre(horaCita))
            {
                MessageBox.Show("Error - No es posible agendar una cita en un horario no correspondiente.");
                return false;
            }
            return true;
        }

        private void limpiar()
        {
            txtRazonCita.Text = "RAZON DE CITA";
            txtRazonCita.ForeColor = Color.DimGray;
        }

        void eventoEntrar()
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
