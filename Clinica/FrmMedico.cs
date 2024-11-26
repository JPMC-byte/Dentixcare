using BLL;
using ENTITY;
using GUI.ClasesControl;
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
    public partial class FrmMedico : Form
    {
        private bool esMaximizada = false;
        private Size escalaOriginal;
        private Point posicionOriginal;

        ServicioConsultorio servisConsul = new ServicioConsultorio();
        Persona personaActual = new Ortodoncista();
        Form formularioActivo = null;
        public FrmMedico(Persona persona)
        {
            InitializeComponent();
            personaActual = persona;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void Panellogo_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void FrmMedico_Load(object sender, EventArgs e)
        {
            guardarTamanoOriginal();
            aplicarEstadoVentana();
            mostrarDia();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            minimizar();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (CtrlMaximizar.EsMaximizada) tamanoOriginal();
            else maximizar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(11);
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            cerrarSesion();
        }

        private void btnDatosUsuario_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmPerfil(personaActual));
            ocultarSubmenu();
        }

        private void btnGestionTratamientos_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(PanelSubmenuTratamientos);
        }

        private void btnDatosConsultorio_Click(object sender, EventArgs e)
        {
            verConsultorio();
        }

        private void btnGestionCitas_Click(object sender, EventArgs e)
        {
            verGestionCita();
        }

        private void btnGestionPaciente_Click(object sender, EventArgs e)
        {
            verGestionPaciente();
        }

        private void btnGestionDiagnostico_Click(object sender, EventArgs e)
        {
            verGestionDiagnosticos();
        }

        private void btnRealizarTratamiento_Click(object sender, EventArgs e)
        {
            verAgendarTratamiento();
        }

        private void btnRegistroTratamientos_Click(object sender, EventArgs e)
        {
            verGestionTratamientos();
        }

        private void abrirFormulario(Form formularioHijo)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }
            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;
            PanelHijo.Controls.Add(formularioHijo);
            PanelHijo.Tag = formularioHijo;
            formularioHijo.BringToFront();
            formularioHijo.Show();
        }

        void moverVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void ocultarSubmenu()
        {
            if (PanelSubmenuTratamientos.Visible == true) PanelSubmenuTratamientos.Visible = false;
        }

        void cerrarSesion()
        {
            if (MessageBox.Show("¿Esta seguro que desea cerrar sesion?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var f = new FrmLoginOrtodoncista();
                f.Show();
                this.Close();
            }
        }

        void minimizar()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void maximizar()
        {
            guardarTamanoOriginal();
            CtrlMaximizar.EsMaximizada = true;
            Rectangle workingArea = Screen.FromHandle(this.Handle).WorkingArea;
            this.Size = new Size(workingArea.Width, workingArea.Height);
            this.Location = workingArea.Location;
        }

        void guardarTamanoOriginal()
        {
            CtrlMaximizar.EscalaOriginal = this.Size;
            CtrlMaximizar.PosicionOriginal = this.Location;
        }

        void tamanoOriginal()
        {
            CtrlMaximizar.EsMaximizada = false;
            this.Size = CtrlMaximizar.EscalaOriginal;
            this.Location = CtrlMaximizar.PosicionOriginal;
        }

        private void mostrarSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                ocultarSubmenu();
                submenu.Visible = true;
            }
            else
            {
                submenu.Visible = false;
            }
        }

        void verConsultorio()
        {
            Consultorio consultorio = servisConsul.cargarConsultorio("P101");
            abrirFormulario(new FrmConsultorio(consultorio));
            ocultarSubmenu();
        }

        void verGestionCita()
        {
            abrirFormulario(new FrmGestionCita(personaActual));
            ocultarSubmenu();
        }

        void verGestionPaciente()
        {
            abrirFormulario(new FrmGestionPaciente());
            ocultarSubmenu();
        }

        void verGestionDiagnosticos()
        {
            abrirFormulario(new FrmGestionAntecedentes(personaActual));
            ocultarSubmenu();
        }

        void verAgendarTratamiento()
        {
            abrirFormulario(new FrmRealizarTratamiento());
            ocultarSubmenu();
        }

        void verGestionTratamientos()
        {
            abrirFormulario(new FrmGestionTratamientos());
            ocultarSubmenu();
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

        private void aplicarEstadoVentana()
        {
            if (CtrlMaximizar.EsMaximizada) { maximizar(); }
            else if (CtrlMaximizar.EscalaOriginal != Size.Empty && CtrlMaximizar.PosicionOriginal != Point.Empty) { tamanoOriginal(); }
        }

        private void contadorDia_Tick(object sender, EventArgs e)
        {
            LBHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        void mostrarDia()
        {
            contadorDia.Tick += contadorDia_Tick;
            contadorDia.Start();
            mostrarFecha();
        }

        void mostrarFecha()
        {
            LBFecha.Text = DateTime.Now.ToString("dd 'de' MMMM 'del' yyyy");
        }
    }
}
