using BLL;
using ENTITY;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmInformeUsuarios : Form
    {
        Validaciones vali = new Validaciones();
        ServicioPaciente servicioPaciente = new ServicioPaciente();
        ServicioOrtodoncista servicioOrtodoncista = new ServicioOrtodoncista();
        ServicioInformes servicioInformes = new ServicioInformes();
        public FrmInformeUsuarios()
        {
            InitializeComponent();
        }

        private void FrmInformeUsuarios_Load(object sender, EventArgs e)
        {
            CargarUsuariosTotales();
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (!ValidarExistente()) { return; }
            AbrirPerfil();
        }

        bool ValidarNumeros(KeyPressEventArgs e)
        {
            if (!vali.validarNumeros(e))
            {
                return false;
            }
            return true;
        }

        public Persona UsuarioSeleccionado()
        {
            Persona persona = servicioOrtodoncista.obtenerPorCodigo(txtCedula.Text);
            if (persona == null) { persona = servicioPaciente.obtenerPorCodigo(txtCedula.Text); }
            return persona;
        }

        void AbrirPerfil()
        {
            Persona persona = UsuarioSeleccionado();
            FrmPerfil F = new FrmPerfil(persona);
            F.Show();
        }

        void EventoEntrar(TextBox textbox, string nombre)
        {
            if (textbox.Text == nombre)
            {
                textbox.Text = "";
                textbox.ForeColor = Color.Black;
            }
        }

        void EventoSalir(TextBox textbox, string nombre)
        {
            if (textbox.Text == "")
            {
                textbox.Text = nombre;
                textbox.ForeColor = Color.DimGray;
            }
        }

        void CargarUsuariosTotales()
        {
            var UsuariosActuales = servicioInformes.ObtenerTotalUsuarios();

            LBTotalOrto.Text = UsuariosActuales["TOTAL_ORTODONCISTAS"].ToString();
            LBTotalPacientes.Text = UsuariosActuales["TOTAL_PACIENTES"].ToString();
        }

        bool ValidarExistente()
        {
            if (vali.validarExistentePaciente(txtCedula.Text) && vali.validarExistenteOrtodoncista(txtCedula.Text))
            {
                MessageBox.Show("Error - Dicho usuario no existe en los registros", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void txtCedulaPaciente_Enter(object sender, EventArgs e) { EventoEntrar(txtCedula, "CEDULA"); }
        private void txtCedulaPaciente_Leave(object sender, EventArgs e) { EventoSalir(txtCedula, "CEDULA"); }

        private void txtCedulaPaciente_KeyPress(object sender, KeyPressEventArgs e) { if (!ValidarNumeros(e)) e.Handled = true; }
    }
}
