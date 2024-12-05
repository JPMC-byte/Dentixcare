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
using System.Web.UI.Design;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmRegistroCita : Form
    {
        ServicioCita servicioCita = new ServicioCita();
        ServicioConsultorio servisconsulto = new ServicioConsultorio();
        ServicioOrtodoncista servicioOrtodoncista = new ServicioOrtodoncista();
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
            CBEstado.SelectedIndex = 0;
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
            if (!verificar()) { return; }
            abrirInformacion();
        }

        private void btnVerInfoOrtodoncista_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarOrtodoncistaAsignado()) { return; }
            abrirInfoOrtodoncista();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarEstado()) { return; }
            abrirActualizar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarEstado()) { return; }
            if (confirmar()) { cancelarCita(); }
        }

        private void CBFiltrarEstado_CheckedChanged(object sender, EventArgs e)
        {
            accionarFiltroPorEstado();
            actualizar();
        }

        private void CBEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            actualizar();
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

        public Ortodoncista ortodoncistaSeleccionado()
        {
            var codigoOrtodoncista = DGVCitas.SelectedRows[0].Cells["CodigoOrtodoncista"].Value.ToString();

            Ortodoncista ortodoncistaSeleccionado = servicioOrtodoncista.obtenerPorCodigo(codigoOrtodoncista);
            return ortodoncistaSeleccionado;
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

            switch (cita.Estado)
            {
                case "Pendiente":
                    {
                        return validarAtendida(cita.Estado);
                    }
                case "Finalizada":
                    {
                        return validarAtendida(cita.Estado);
                    }
                case "Cancelada":
                    {
                        return validarCancelada(cita.Estado);
                    }
            }
            return true;
        }

        bool validarAtendida(string texto)
        {
            if (!vali.validarAtendidaPaciente(texto))
            {
                MessageBox.Show("Error - No es posible alterar una cita que ya ha sido atendida.", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool validarCancelada(string texto)
        {
            if (!vali.validarCancelada(texto))
            {
                MessageBox.Show("Error - No es posible alterar una cita que ha sido cancelada", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool validarOrtodoncistaAsignado()
        {
            Ortodoncista ortodoncista = ortodoncistaSeleccionado();
            if (ortodoncista == null)
            {
                MessageBox.Show("Error - Un ortodoncista aun no ha sido asignado a esta cita", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            servicioCita.actualizarEstado(cita, "Cancelada");
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

        void abrirInfoOrtodoncista()
        {
            Ortodoncista ortodoncista = ortodoncistaSeleccionado();
            FrmPerfil F = new FrmPerfil(ortodoncista);
            F.Show();
        }
    }
}