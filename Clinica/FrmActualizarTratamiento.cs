using BLL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmActualizarTratamiento : Form
    {
        Tratamiento tratamientoActual;
        ServicioTratamiento servisTrat = new ServicioTratamiento();
        Validaciones vali = new Validaciones();

        public FrmActualizarTratamiento(Tratamiento tratamiento)
        {
            InitializeComponent();
            tratamientoActual = tratamiento;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmActualizarTratamiento_Load(object sender, EventArgs e)
        {
            cargarDatos(tratamientoActual);
        }

        private void FrmActualizarTratamiento_MouseDown(object sender, MouseEventArgs e)
        {
            moverPestaña();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            limpiar();
        }

        private void BtnMinimizar_Click(object sender, EventArgs e)
        {
            minimizar();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarMonto())
            {
                return;
            }
            if (confirmar())
            {
                actualizar();
            }
        }

        void moverPestaña()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void cargarDatos(Tratamiento tratamiento)
        {
            txtCodigo.Text = tratamiento.ID_Tratamiento;
            txtCodigoDiagnostico.Text = tratamiento.CodigoDiagnostico;
            txtCodigoFactura.Text = tratamiento.CodigoFactura;
        }

        void actualizar()
        {
            string nuevaDescripcion = txtDescripcion.Text;
            string nuevaDuracion = txtDuracion.Text;
            double nuevoCosto = Convert.ToDouble(txtCosto.Text);
            servisTrat.actualizarTratamiento(tratamientoActual, nuevaDescripcion, nuevaDuracion, nuevoCosto);
            MessageBox.Show("Proceso de modificación exitoso");
            limpiar();
        }

        bool verificar()
        {
            if (txtDuracion.Text == "DURACION" || txtCosto.Text == "COSTO" || txtDescripcion.Text == "DESCRIPCION")
            {
                MessageBox.Show("Por favor, rellene/complete los campos vacios");
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

        bool validarNumeros(KeyPressEventArgs e)
        {
            if (!vali.validarNumeros(e))
            {
                return false;
            }
            return true;
        }

        bool confirmar()
        {
            return MessageBox.Show("¿Está seguro que desea actualizar dicho registro?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        void cerrar()
        {
            this.Close();
        }

        void minimizar()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void limpiar()
        {
            baseTextbox(txtDuracion, "DURACION");
            baseTextbox(txtCosto, "COSTO");
            baseTextbox(txtDescripcion, "DESCRIPCION");
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

        void eventoSalirTextbox(TextBox textBox, string nombre)
        {
            if (textBox.Text == "")
            {
                textBox.Text = nombre;
                textBox.ForeColor = Color.DimGray;
            }
        }

        private void txtCosto_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtCosto, "COSTO");
        private void txtCosto_Leave(object sender, EventArgs e) => eventoSalirTextbox(txtCosto, "COSTO");
        private void txtDuracion_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtDuracion, "DURACION");
        private void txtDuracion_Leave(object sender, EventArgs e) => eventoSalirTextbox(txtDuracion, "DURACION");
        private void txtDescripcion_Enter(object sender, EventArgs e) => eventoEntrarTextbox(txtDescripcion, "DESCRIPCION");
        private void txtDescripcion_Leave(object sender, EventArgs e) => eventoSalirTextbox(txtDescripcion, "DESCRIPCION");

        private void txtCosto_KeyPress(object sender, KeyPressEventArgs e) { if (!validarNumeros(e)) e.Handled = true; }
    }
}
