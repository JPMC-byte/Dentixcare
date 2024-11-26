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
    public partial class FrmRealizarTratamiento : Form
    {
        ServicioTratamiento servisTrat = new ServicioTratamiento();
        Validaciones vali = new Validaciones();

        public FrmRealizarTratamiento()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(13);
        }

        private void btnRegistrado_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarMonto())
            {
                return;
            }
            registrar();
        }

        void registrar()
        {
            Tratamiento tratamiento = new Tratamiento();
            tratamiento.ID_Tratamiento = servisTrat.generarCodigo();
            tratamiento.Costo = Convert.ToDouble(txtCosto.Text);
            tratamiento.Duracion = txtDuracion.Text;
            tratamiento.Descripcion = txtDescripcion.Text;
            tratamiento.CodigoDiagnostico = null;
            tratamiento.CodigoFactura = null;
            servisTrat.guardar(tratamiento);
            MessageBox.Show("Proceso de registro exitoso");
            limpiar();
        }

        bool verificar()
        {
            if (txtCosto.Text == "COSTO" || txtDuracion.Text == "DURACION" || txtDescripcion.Text == "DESCRIPCION")
            {
                MessageBox.Show("Por favor, rellene/complete los campos vacios");
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

        bool validarMonto()
        {
            double monto = Convert.ToDouble(txtCosto.Text);
            if (!vali.validarMonto(monto))
            {
                MessageBox.Show("Error - El monto asignado no es valido", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        void cerrar()
        {
            this.Close();
        }

        void baseTextbox(TextBox textBox, string nombre)
        {
            textBox.Text = nombre;
            textBox.ForeColor = Color.DimGray;
        }

        private void limpiar()
        {
            baseTextbox(txtCosto, "COSTO");
            baseTextbox(txtDuracion, "DURACION");
            baseTextbox(txtDescripcion, "DESCRIPCION");
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

        private void txtCosto_Enter(object sender, EventArgs e) { eventoEntrarTextbox(txtCosto, "COSTO"); }
        private void txtCosto_Leave(object sender, EventArgs e) { eventoDejarTextbox(txtCosto, "COSTO"); }
        private void txtDuracion_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtDuracion, "DURACION");
        private void txtDuracion_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtDuracion, "DURACION");
        private void txtDescripcion_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtDescripcion, "DESCRIPCION");
        private void txtDescripcion_Leave(object sender, EventArgs e) => eventoDejarTextbox(txtDescripcion, "DESCRIPCION");

        private void txtCosto_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
    }
}
