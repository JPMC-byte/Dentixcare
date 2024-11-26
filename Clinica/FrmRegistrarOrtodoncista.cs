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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmRegistrarOrtodoncista : Form
    {
        ServicioOrtodoncista servispaciente = new ServicioOrtodoncista();
        ServicioConsultorio servisconsulto = new ServicioConsultorio();
        Validaciones vali = new Validaciones();
        public FrmRegistrarOrtodoncista()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmRegistrarOrtodoncista_Load(object sender, EventArgs e)
        {
            guardarTamanoOriginal();
            aplicarEstadoVentana();
        }

        private void FrmRegistrarOrtodoncista_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!CtrlMaximizar.EsMaximizada) moverVentana();
        }

        private void btnRegistrado_Click(object sender, EventArgs e)
        {
            registrar();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            volver();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            minimizar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(4);
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (CtrlMaximizar.EsMaximizada) tamanoOriginal();
            else maximizar();
        }

        public void registrar()
        {
            if (!verificar() || !validarExistente() || !verificarCamposOpcionales())
            {
                return;
            }

            Ortodoncista ortodoncista = new Ortodoncista();
            Consultorio consultorio = servisconsulto.cargarConsultorio("P101");

            ortodoncista.CodigoConsultorio = consultorio.Codigo;
            ortodoncista.PrimerNombre = txtPrimerNombre.Text;
            ortodoncista.SegundoNombre = (txtSegundoNombre.Text == "SEGUNDO NOMBRE") ? "N/A" : txtSegundoNombre.Text;
            ortodoncista.PrimerApellido = txtPrimerApellido.Text;
            ortodoncista.SegundoApellido = (txtSegundoApellido.Text == "SEGUNDO APELLIDO") ? "N/A" : txtSegundoApellido.Text;
            ortodoncista.Telefono = txtTelefono.Text;
            ortodoncista.Cedula = txtCedula.Text;
            ortodoncista.Fecha_De_Nacimiento = DTFecha_Nacimiento.Value;
            ortodoncista.Contrasena = txtContraseña.Text;

            servispaciente.guardar(ortodoncista);
            MessageBox.Show("Proceso de registro exitoso");
            limpiar();
        }

        bool verificar()
        {
            if (txtPrimerNombre.Text == "PRIMER NOMBRE" || txtPrimerApellido.Text == "PRIMER APELLIDO" ||
                txtTelefono.Text == "TELEFONO" || txtCedula.Text == "CEDULA" || txtContraseña.Text == "CONTRASEÑA")
            {
                MessageBox.Show("Por favor, rellene/complete los campos vacios");
                return false;
            }
            return true;
        }

        private bool verificarCamposOpcionales()
        {
            if (!vali.validarSegundosNombres(txtSegundoNombre.Text))
            {
                DialogResult result = MessageBox.Show("El campo 'Segundo Nombre' está vacío. ¿Desea continuar y dejarlo como 'N/A'?", "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result == DialogResult.No)
                {
                    txtSegundoNombre.Focus();
                    return false;
                }
            }
            if (!vali.validarSegundosNombres(txtSegundoApellido.Text))
            {
                DialogResult result = MessageBox.Show("El campo 'Segundo Apellido' está vacío. ¿Desea continuar y dejarlo como 'N/A'?", "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );
                if (result == DialogResult.No)
                {
                    txtSegundoApellido.Focus();
                    return false;
                }
            }
            return true;
        }

        bool validarLetras(KeyPressEventArgs e)
        {
            if (!vali.validarLetras(e))
            {
                return false;
            }
            return true;
        }

        bool validarNumeros(KeyPressEventArgs e)
        {
            if (!vali.validarNumeros(e))
            {
                return false;
            }
            return true;
        }

        bool validarExistente()
        {
            if (!vali.validarExistenteOrtodoncista(txtCedula.Text))
            {
                MessageBox.Show("Error - Esté usuario ya se encuentra registrado.");
                return false;
            }
            return true;
        }

        private void limpiar()
        {
            baseTextbox(txtPrimerNombre, "PRIMER NOMBRE");
            baseTextbox(txtSegundoNombre, "SEGUNDO NOMBRE");
            baseTextbox(txtPrimerApellido, "PRIMER APELLIDO");
            baseTextbox(txtSegundoApellido, "SEGUNDO APELLIDO");
            baseTextbox(txtTelefono, "TELEFONO");
            baseTextbox(txtCedula, "CEDULA");
            baseTextbox(txtContraseña, "CONTRASEÑA");
        }

        void baseTextbox(TextBox textBox, string nombre)
        {
            textBox.Text = nombre;
            textBox.ForeColor = Color.DimGray;
        }

        void volver()
        {
            var f = new FrmAcceso();
            f.Show();
            this.Close();
        }

        void moverVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
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

        void EventoEntrarTextbox(TextBox textBox, string nombre)
        {
            if (textBox.Text == nombre)
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        void EventoDejarTextbox(TextBox textBox, string nombre)
        {
            if (textBox.Text == "")
            {
                textBox.Text = nombre;
                textBox.ForeColor = Color.DimGray;
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

        private void txtPrimerNombre_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtPrimerNombre, "PRIMER NOMBRE");
        private void txtPrimerNombre_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtPrimerNombre, "PRIMER NOMBRE");
        private void txtSegundoNombre_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtSegundoNombre, "SEGUNDO NOMBRE");
        private void txtSegundoNombre_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtSegundoNombre, "SEGUNDO NOMBRE");
        private void txtPrimerApellido_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtPrimerApellido, "PRIMER APELLIDO");
        private void txtPrimerApellido_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtPrimerApellido, "PRIMER APELLIDO");
        private void txtSegundoApellido_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtSegundoApellido, "SEGUNDO APELLIDO");
        private void txtSegundoApellido_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtSegundoApellido, "SEGUNDO APELLIDO");
        private void txtTelefono_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtTelefono, "TELEFONO");
        private void txtTelefono_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtTelefono, "TELEFONO");
        private void txtCedula_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtCedula, "CEDULA");
        private void txtCedula_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtCedula, "CEDULA");
        private void txtContraseña_Enter(object sender, EventArgs e) => EventoEntrarTextbox(txtContraseña, "CONTRASEÑA");
        private void txtContraseña_Leave(object sender, EventArgs e) => EventoDejarTextbox(txtContraseña, "CONTRASEÑA");

        private void txtPrimerNombre_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtSegundoNombre_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtPrimerApellido_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtSegundoApellido_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
    }
}
