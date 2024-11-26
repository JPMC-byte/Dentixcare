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
    public partial class FrmContraseña : Form
    {
        FrmRegistrarOrtodoncista frmRegistrarOrtodoncista = new FrmRegistrarOrtodoncista();
        Form formularioActual;
        
        public FrmContraseña()
        {
            InitializeComponent();
            //formularioActual = formulario;
        }

        //public FrmContraseña()
        //{
        //}

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void EntrarSistema_Click(object sender, EventArgs e)
        {
            if (!verificar()) return;
            abrirRegistroOrtodoncista();
        }
        private void btnVolver_Click(object sender, EventArgs e)
        {
            volver();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(5);
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            minimizar();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            moverPestaña();
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

        bool verificar()
        {
            if (txtContrasena.Text == "Sistema123")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Contraseña Invalida");
                return false;
            }
        }

        void abrirRegistroOrtodoncista()
        {
            frmRegistrarOrtodoncista.Show();
            this.Close();
        }

        void minimizar()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void volver()
        {
            var f = new FrmAcceso();
            f.Show();
            this.Close();
        }

        void moverPestaña()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
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

        private void txtContrasena_Enter(object sender, EventArgs e) { eventoEntrarTextbox(txtContrasena, "CONTRASEÑA"); }
        private void txtContrasena_Leave(object sender, EventArgs e) { eventoDejarTextbox(txtContrasena, "CONTRASEÑA"); }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
