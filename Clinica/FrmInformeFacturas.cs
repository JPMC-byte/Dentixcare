using BLL;
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
    public partial class FrmInformeFacturas : Form
    {
        Validaciones vali = new Validaciones();
        ServicioInformes servicioInformes = new ServicioInformes(); 
        public FrmInformeFacturas()
        {
            InitializeComponent();
            
        }

        private void FrmInformeFacturas_Load(object sender, EventArgs e)
        {
            CBVista.SelectedIndex = 0;
            CBAnual.SelectedIndex = 0;
        }

        private void CBVista_SelectedIndexChanged(object sender, EventArgs e)
        {
            accionarFiltroDeVista();
        }

        private void CBMensual_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarFiltroPorMes();
        }

        private void CBAnual_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarFiltroPorAnio();
        }

        void accionarFiltroDeVista()
        {
            switch (CBVista.Text)
            {
                case "N/A":
                    {
                        CBMensual.Enabled = false;
                        CBAnual.Enabled = false;
                        Limpiar();
                        break;
                    }
                case "Mensual":
                    {
                        CBMensual.Enabled = true;
                        CBAnual.Enabled = false;
                        break;
                    }
                case "Anual":
                    {
                        CBAnual.Enabled = true;
                        CBMensual.Enabled = true;
                        break;
                    }
            }
        }

        void CargarFiltroPorMes()
        {
            var FacturasPorMes = servicioInformes.ObtenerFacturasPorMes(AnioSeleccionado(), MesSeleccionado());

            if (!ValidarFacturaPorEspecificacion(FacturasPorMes)) { Limpiar(); return; }
            LBTotal.Text = FacturasPorMes["TOTALFACTURAS"].ToString();
            LBPendientes.Text = FacturasPorMes["FACTURASPENDIENTES"].ToString();
            LBPagadas.Text = FacturasPorMes["FACTURASFINALIZADAS"].ToString();
            LBFacturacionTotal.Text = FacturasPorMes["TOTALFACTURACION"].ToString();
            LBDineroPagado.Text = FacturasPorMes["TOTALPAGADO"].ToString();
        }

        void CargarFiltroPorAnio()
        {
            var FacturasPorAnio = servicioInformes.ObtenerFacturasPorAnio(AnioSeleccionado());

            if (!ValidarFacturaPorEspecificacion(FacturasPorAnio)) { Limpiar(); return; }
            LBTotal.Text = FacturasPorAnio["TOTALFACTURAS"].ToString();
            LBPendientes.Text = FacturasPorAnio["FACTURASPENDIENTES"].ToString();
            LBPagadas.Text = FacturasPorAnio["FACTURASFINALIZADAS"].ToString();
            LBFacturacionTotal.Text = FacturasPorAnio["TOTALFACTURACION"].ToString();
            LBDineroPagado.Text = FacturasPorAnio["TOTALPAGADO"].ToString();
        }

        int MesSeleccionado()
        {
            string mesSeleccionado = CBMensual.SelectedItem.ToString();
            int mesNumerico = ObtenerMesSeleccionado(mesSeleccionado);
            return mesNumerico;
        }

        int AnioSeleccionado()
        {
            int anioSeleccionado = Convert.ToInt32(CBAnual.SelectedItem);
            return anioSeleccionado;
        }

        bool ValidarFacturaPorEspecificacion(Dictionary<string, object> informes)
        {
            if (!vali.ValidarInforme(informes))
            {
                MessageBox.Show("Error - No se encontraron reportes de informes para esta especificación", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        public int ObtenerMesSeleccionado(string mesSeleccionado)
        {
            if (mesSeleccionado == null) return 0;

            switch (mesSeleccionado.ToLower())
            {
                case "enero": return 1;
                case "febrero": return 2;
                case "marzo": return 3;
                case "abril": return 4;
                case "mayo": return 5;
                case "junio": return 6;
                case "julio": return 7;
                case "agosto": return 8;
                case "septiembre": return 9;
                case "octubre": return 10;
                case "noviembre": return 11;
                case "diciembre": return 12;
                default: return 0;
            }
        }

        void Limpiar()
        {
            baseLabel(LBTotal);
            baseLabel(LBPendientes);
            baseLabel(LBPagadas);
            baseLabel(LBFacturacionTotal);
            baseLabel(LBDineroPagado);
        }

        void baseLabel(Label label)
        {
            label.Text = "";
        }
    }
}
