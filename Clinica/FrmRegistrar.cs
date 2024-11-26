using System;
using System.Drawing;
using System.Windows.Forms;
using Logica;
using ENTITY;
using BLL;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using GUI.ClasesControl;

namespace GUI
{
    public partial class FrmRegistrar : Form
    {
        ServicioPaciente servispaciente = new ServicioPaciente();
        ServicioConsultorio servisconsulto = new ServicioConsultorio();
        Validaciones vali = new Validaciones();
        public FrmRegistrar()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmRegistrar_Load(object sender, EventArgs e)
        {
            guardarTamanoOriginal();
            aplicarEstadoVentana();
        }

        private void FrmRegistrar_MouseDown(object sender, MouseEventArgs e)
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

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            minimizar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(4);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            volver();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            if (CtrlMaximizar.EsMaximizada) tamanoOriginal();
            else maximizar();
        }

        private void registrar()
        {
            if (!verificar() || !validarExistente() || !verificarCamposOpcionales()) { return; }

            Paciente paciente = new Paciente();
            Consultorio consultorio = new Consultorio();
            consultorio = servisconsulto.cargarConsultorio("P101");
            paciente.CodigoConsultorio = consultorio.Codigo;
            paciente.PrimerNombre = txtPrimerNombre.Text;
            paciente.SegundoNombre = (txtSegundoNombre.Text == "SEGUNDO NOMBRE") ? "N/A" : txtSegundoNombre.Text;
            paciente.PrimerApellido = txtPrimerApellido.Text;
            paciente.SegundoApellido = (txtSegundoApellido.Text == "SEGUNDO APELLIDO") ? "N/A" : txtSegundoApellido.Text;
            paciente.Telefono = txtTelefono.Text;
            paciente.Cedula = txtCedula.Text;
            paciente.Fecha_De_Nacimiento = DTFecha_Nacimiento.Value.Date;
            paciente.Contrasena = txtContrasena.Text;
            servispaciente.guardar(paciente);
            MessageBox.Show("Proceso de registro exitoso");
            limpiar();
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

        bool verificar()
        {
            if (txtPrimerNombre.Text == "PRIMER NOMBRE" || txtPrimerApellido.Text == "PRIMER APELLIDO" || 
                txtTelefono.Text == "TELEFONO" || txtCedula.Text == "CEDULA" || txtContrasena.Text == "CONTRASEÑA")
            {
                MessageBox.Show("Por favor, rellene/complete los campos vacios");
                return false;
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
            if (!vali.validarExistentePaciente(txtCedula.Text))
            {
                MessageBox.Show("Error - Esté paciente ya se encuentra registrado.");
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
            baseTextbox(txtContrasena, "CONTRASEÑA");
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

        private void aplicarEstadoVentana()
        {
            if (CtrlMaximizar.EsMaximizada) { maximizar(); }
            else if (CtrlMaximizar.EscalaOriginal != Size.Empty && CtrlMaximizar.PosicionOriginal != Point.Empty) { tamanoOriginal(); }
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

        private void txtPrimerNombre_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtPrimerNombre, "PRIMER NOMBRE");
        private void txtPrimerNombre_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtPrimerNombre, "PRIMER NOMBRE");
        private void txtSegundoNombre_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtSegundoNombre, "SEGUNDO NOMBRE");
        private void txtSegundoNombre_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtSegundoNombre, "SEGUNDO NOMBRE");
        private void txtPrimerApellido_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtPrimerApellido, "PRIMER APELLIDO");
        private void txtPrimerApellido_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtPrimerApellido, "PRIMER APELLIDO");
        private void txtSegundoApellido_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtSegundoApellido, "SEGUNDO APELLIDO");
        private void txtSegundoApellido_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtSegundoApellido, "SEGUNDO APELLIDO");
        private void txtTelefono_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtTelefono, "TELEFONO");
        private void txtTelefono_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtTelefono, "TELEFONO");
        private void txtCedula_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtCedula, "CEDULA");
        private void txtCedula_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtCedula, "CEDULA");
        private void txtContrasena_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtContrasena, "CONTRASEÑA");
        private void txtContrasena_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtContrasena, "CONTRASEÑA");


        private void txtPrimerNombre_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtSegundoNombre_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtPrimerApellido_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtSegundoApellido_KeyPress(object sender, KeyPressEventArgs e) { if (!validarLetras(e)) e.Handled = true; }
        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
        private void txtCedula_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
    }
}
