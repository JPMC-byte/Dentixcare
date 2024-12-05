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
    public partial class FrmInformes : Form
    {
        Form formularioActivo = null;
        public FrmInformes()
        {
            InitializeComponent();
        }

        private void CBEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            accionarTipoDeVista();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
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

        void cerrar()
        {
            this.Close();
        }

        void accionarTipoDeVista()
        {
            switch (CBTipoInforme.Text)
            {
                case "Citas": { abrirFormulario(new FrmInformaCitas()); break; }
                case "Usuarios": { abrirFormulario(new FrmInformeUsuarios()); break; }
                case "Facturas": { abrirFormulario(new FrmInformeFacturas()); break; }
                case "Pagos": { abrirFormulario(new FrmInformesPagos()); break; }
            }
        }
    }
}
