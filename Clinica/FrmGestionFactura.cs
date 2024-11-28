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
    public partial class FrmGestionFactura : Form
    {
        Persona usuarioActual = new Paciente();
        ServicioFactura servisFactu = new ServicioFactura();
        Validaciones vali = new Validaciones();

        public FrmGestionFactura(Persona persona)
        {
            InitializeComponent();
            usuarioActual = persona;
        }

        private void FrmGestionFactura_Load(object sender, EventArgs e)
        {
            cargarFacturas();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(10);
        }

        private void btnActualizarRegistro_Click(object sender, EventArgs e)
        {
            cargarFacturas();
        }

        private void btnInformacion_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            abrirMasInformacion();
        }

        private void btnTratamientosRelacionados_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            verTratamientosRelacionados();
        }

        private void btnVerPagos_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarPagos()) { return; }
            abrirGestionPagos();
        }

        private void btnRealizarPago_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarFacturaPagada())
            {
                return;
            }
            abrirRealizarPago();
        }

        void cerrar()
        {
            this.Close();
        }

        void cargarFacturas()
        {
            var Facturas = servisFactu.cargarPorCedula(usuarioActual.Cedula);
            DGVFacturas.DataSource = Facturas;
        }

        bool verificar()
        {
            if (DGVFacturas.SelectedRows.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una factura de la lista para realizar dicha acción", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        bool validarPagos()
        {
            Factura factura = facturaSeleccionada();
            if (!vali.validarPagos(factura))
            {
                MessageBox.Show("La factura seleccionada no presenta pagos previos registrados", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        public Factura facturaSeleccionada()
        {
            var IDFactura = DGVFacturas.SelectedRows[0].Cells["ID_Factura"].Value.ToString();

            Factura facturaSeleccionada = servisFactu.obtenerPorCodigo(IDFactura);
            return facturaSeleccionada;
        }

        void abrirMasInformacion()
        {
            Factura factura = facturaSeleccionada();
            FrmInfoFactura F = new FrmInfoFactura(factura);
            F.Show();
        }

        void verTratamientosRelacionados()
        {
            Factura factura = facturaSeleccionada();
            FrmGestionTratamientos F = new FrmGestionTratamientos(factura);
            F.Show();
        }

        bool validarFacturaPagada()
        {
            Factura factura = facturaSeleccionada();
            if (vali.validarFacturaPagada(factura))
            {
                MessageBox.Show("Error - Dicha factura ya ha sido pagada", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        void abrirRealizarPago()
        {
            Factura factura = facturaSeleccionada();
            FrmRealizarPago F = new FrmRealizarPago(factura, usuarioActual);
            F.Show();
        }

        void abrirGestionPagos()
        {
            Factura factura = facturaSeleccionada();
            FrmGestionPago F = new FrmGestionPago(factura);
            F.Show();
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
    }
}
