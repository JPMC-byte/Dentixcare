using BLL;
using ENTITY;
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
    public partial class FrmRegistroCita : Form
    {
        ServicioCita servicioCita = new ServicioCita();
        ServicioConsultorio servisconsulto = new ServicioConsultorio();
        Validaciones vali = new Validaciones();
        Persona UsuarioActual;
        public FrmRegistroCita(Persona persona)
        {
            InitializeComponent();
            UsuarioActual = persona;
        }

        private void FrmRegistroCita_Load(object sender, EventArgs e)
        {
            cargarCitas();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            actualizar();
        }

        private void btnInformacion_Click(object sender, EventArgs e)
        {
            if (!verificar())
            {
                return;
            }
            abrirInformacion();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarEstado())
            {
                return;
            }
            abrirActualizar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarEstado())
            {
                return;
            }
            if (confirmar())
            {
                cancelarCita();
            }
        }

        private void CBFiltrarEstado_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorEstado();
        }

        private void cargarCitas(string estado = null)
        {
            var citas = servicioCita.cargarPorCodigo(UsuarioActual.Cedula);

            if (validarFiltroEstado(CBFiltrarEstado.Checked, estado))
            {
                citas = servicioCita.cargarPorFiltrosGui(estado, UsuarioActual.Cedula);
            }
            DGVCitas.DataSource = citas;
        }

        public Cita citaSeleccionada()
        {
            var codigoCita = DGVCitas.SelectedRows[0].Cells["Codigo"].Value.ToString();

            Cita citaSeleccionada = servicioCita.obtenerPorCodigo(codigoCita);
            return citaSeleccionada;
        }

        void abrirInformacion()
        {
            Cita cita = citaSeleccionada();
            FrmInformacion F = new FrmInformacion(cita);
            F.Show();
        }

        bool verificar()
        {
            if (DGVCitas.SelectedRows.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una cita de la lista para ver más información.", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        bool validarEstado()
        {
            Cita cita = citaSeleccionada();
            if (!vali.validarAtendida(cita.Estado))
            {
                MessageBox.Show("Error - No es posible alterar una cita que ya ha sido atendida.");
                return false;
            }
            return true;
        }

        public bool validarFiltroEstado(bool activo, string texto)
        {
            return vali.validarFiltroEstado(activo, texto);
        }

        void abrirActualizar()
        {
            Cita cita = citaSeleccionada();
            FrmActualizarCita F = new FrmActualizarCita(cita);
            F.Show();
        }

        bool confirmar()
        {
            return MessageBox.Show("¿Está seguro que desea eliminar dicho registro?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        void cancelarCita()
        {
            Cita cita = citaSeleccionada();
            servicioCita.eliminar(cita);
        }

        void actualizar()
        {
            string estadoSeleccionado = CBEstado.SelectedItem?.ToString();
            cargarCitas(estadoSeleccionado);
        }

        void cerrar()
        {
            this.Close();
        }

        void accionarFiltroPorEstado()
        {
            if (CBFiltrarEstado.Checked)
            {
                CBEstado.Enabled = true;
            }
            else
            {
                CBEstado.Enabled = false;
            }
        }
    }
}