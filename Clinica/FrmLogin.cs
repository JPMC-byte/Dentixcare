using BLL;
using ENTITY;
using GUI.ClasesControl;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmLogin : Form
    {
        ServicioPaciente servispaciente = new ServicioPaciente();
        Validaciones vali = new Validaciones();
        public FrmLogin()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            guardarTamanoOriginal();
            aplicarEstadoVentana();
        }

        private void FrmLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void btnIngresar_Click(object sender, EventArgs e) 
        {
            ingreso();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            minimizar();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            volver();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (CtrlMaximizar.EsMaximizada) tamanoOriginal();
            else maximizar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(5);
        }

        private void ingreso()
        {
            if (!verificar()) { return; }
            string Cedula = txtCedula.Text;
            string Contraseña = txtContrasena.Text;
            Paciente paciente = servispaciente.iniciarSesion(Cedula,Contraseña);
            if(validarUsuario(paciente)) abrirIngreso(paciente);
        }

        bool verificar()
        {
            if (txtCedula.Text == "CEDULA" || txtContrasena.Text == "CONTRASEÑA")
            {
                MessageBox.Show("Por favor, rellene/complete los campos vacios");
                return false;
            }
            return true;
        }

        bool validar(KeyPressEventArgs e)
        {
            if (!vali.validarNumeros(e))
            {
                return false;
            }
            return true;
        }

        bool validarUsuario(Paciente paciente)
        {
            if (paciente != null)
            {
                MessageBox.Show("Inicio de sesion exitoso");
                return true;
            }
            else
            {
                MessageBox.Show("Error - Cedula y/o contraseña incorrecta");
                return false;
            }
        }

        void abrirIngreso(Paciente paciente)
        {
            var F = new FrmPaciente(paciente);
            F.Show();
            this.Close();
        }

        private void limpiar()
        {
            baseTextbox(txtCedula, "CEDULA");
            baseTextbox(txtContrasena, "CONTRASEÑA");
        }

        void volver()
        {
            var f = new FrmInicioSesion();
            f.Show();
            this.Close();
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

        void moverVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        void baseTextbox(TextBox textBox, string nombre)
        {
            textBox.Text = nombre;
            textBox.ForeColor = Color.DimGray;
        }

        void eventoEntrarTextbox(TextBox textBox, string nombre)
        {
            if (textBox.Text == nombre)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        void eventoDejarTextbox(TextBox textBox, string nombre)
        {
            if (textBox.Text == "")
            {
                textBox.Text = nombre;
                textBox.ForeColor = Color.DimGray;
            }
        }

        private void CBVerContraseña_CheckedChanged(object sender, EventArgs e)
        {
            if (CBVerContraseña.Checked)
            {
                txtContrasena.PasswordChar = '\0'; 
            }
            else
            {
                txtContrasena.PasswordChar = '*'; 
            }
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

        private void txtCedula_Enter(object sender, EventArgs e) { eventoEntrarTextbox(txtCedula, "CEDULA"); }
        private void txtCedula_Leave(object sender, EventArgs e) { eventoDejarTextbox(txtCedula, "CEDULA"); }
        private void txtContrasena_Enter(object sender, EventArgs e) { eventoEntrarTextbox(txtContrasena, "CONTRASEÑA"); }
        private void txtContrasena_Leave(object sender, EventArgs e) { eventoDejarTextbox(txtContrasena, "CONTRASEÑA"); }

        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e) { if (!validar(e)) e.Handled = true; }
    }
}
