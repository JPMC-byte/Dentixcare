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
    public partial class FrmPaciente : Form
    {
        ServicioConsultorio servisConsul = new ServicioConsultorio();
        Persona usuarioActual = new Paciente();
        Form formularioActivo = null;
        public FrmPaciente(Persona persona)
        {
            InitializeComponent();
            usuarioActual = persona;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmPaciente_Load(object sender, EventArgs e)
        {
            guardarTamanoOriginal();
            aplicarEstadoVentana();
            mostrarDia();
        }

        private void PCFondo_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void Panellogo_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            cerrarSesion();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(6);
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

        private void btnDatosUsuario_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(PanelSubmenuDatos);
        }

        private void btnGestionCitas_Click(object sender, EventArgs e)
        {
            mostrarSubmenu(PanelSubmenuCitas);
        }

        private void btnGestionPagos_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmGestionFactura(usuarioActual));
            ocultarSubmenu();
        }

        private void btnPerfil_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmPerfil(usuarioActual));
            ocultarSubmenu();
        }

        private void btnAntecedentes_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmGestionAntecedentes(usuarioActual));
            ocultarSubmenu();
        }

        private void btnAgendarCita_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmAgendarCita(usuarioActual));
            ocultarSubmenu();
        }

        private void btnRegistroCitas_Click(object sender, EventArgs e)
        {
            abrirFormulario(new FrmRegistroCita(usuarioActual));
            ocultarSubmenu();
        }

        private void btnDatosConsultorio_Click(object sender, EventArgs e)
        {
            Consultorio consultorio = servisConsul.cargarConsultorio("P101");
            abrirFormulario(new FrmConsultorio(consultorio));
            ocultarSubmenu();
        }

        void moverVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        void cerrarSesion()
        {
            if (MessageBox.Show("¿Esta seguro que desea cerrar sesion?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var F = new FrmLogin();
                F.Show();
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

        private void ocultarSubmenu()
        {
            if (PanelSubmenuDatos.Visible == true)
                PanelSubmenuDatos.Visible = false;
            if (PanelSubmenuCitas.Visible == true)
                PanelSubmenuCitas.Visible = false;
        }

        private void mostrarSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                ocultarSubmenu();
                submenu.Visible = true;
            }else
            {
                submenu.Visible = false;
            }
        }

        private void abrirFormulario(Form FormularioHijo)
        {
            if (formularioActivo != null)
            {
                formularioActivo.Close();
            }
            formularioActivo = FormularioHijo;
            FormularioHijo.TopLevel = false;
            FormularioHijo.FormBorderStyle = FormBorderStyle.None;
            FormularioHijo.Dock = DockStyle.Fill;
            PanelHijo.Controls.Add(FormularioHijo);
            PanelHijo.Tag = FormularioHijo;
            FormularioHijo.BringToFront();
            FormularioHijo.Show();
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

        void mostrarDia()
        {
            timer1.Tick += timer1_Tick;
            timer1.Start();
            mostrarFecha();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LBHora.Text = DateTime.Now.ToString("HH:mm:ss"); 
        }

        void mostrarFecha()
        {
            LBFecha.Text = DateTime.Now.ToString("dd 'de' MMMM 'del' yyyy");
        }

        private void aplicarEstadoVentana()
        {
            if (CtrlMaximizar.EsMaximizada) { maximizar(); }
            else if (CtrlMaximizar.EscalaOriginal != Size.Empty && CtrlMaximizar.PosicionOriginal != Point.Empty) { tamanoOriginal(); }
        }
    }
}
