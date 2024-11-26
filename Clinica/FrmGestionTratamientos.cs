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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class FrmGestionTratamientos : Form
    {
        Diagnostico diagnosticoActual;
        Factura facturaActual;
        ServicioTratamiento servistrat = new ServicioTratamiento();
        ServicioFactura servisFactu = new ServicioFactura();
        Validaciones vali = new Validaciones();

        public FrmGestionTratamientos()
        {
            InitializeComponent();
        }

        public FrmGestionTratamientos(Diagnostico diagnostico = null)
        {
            InitializeComponent();
            diagnosticoActual = diagnostico;
        }

        public FrmGestionTratamientos(Factura factura = null)
        {
            InitializeComponent();
            facturaActual = factura;
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void FrmGestionTratamientos_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.Parent == null) moverVentana();
        }

        private void FrmGestionTratamientos_Load(object sender, EventArgs e)
        {
            determinarIndependencia();
            cargarTratamientos();
        }

        private void IconDudas_Click(object sender, EventArgs e)
        {
            abrirManualUsuario(13);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            cerrar();
        }

        private void btnActualizarRegistro_Click(object sender, EventArgs e)
        {
            actualizar();
        }

        private void btnInformacion_Click(object sender, EventArgs e)
        {
            if (!verificar()) { return; }
            abrirInformacion();
        }

        private void btnFacturaRelacionada_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarTratamientoAsignado())
            {
                return;
            }
            abrirInfoFactura();
        }

        private void btnActualizarTratamiento_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarExistenteRelacion()) { return; }
            abrirActualizar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (!verificar() || !validarExistenteRelacion())
            {
                return;
            }
            if (confirmar())
            {
                eliminarTratamiento();
            }
        }

        void moverVentana()
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        void determinarIndependencia()
        {
            if (this.Parent == null)
            {
                btnActualizarRegistro.Enabled = false;
                btnEliminar.Enabled = false;
                btnActualizarTratamiento.Enabled = false;
                btnFacturaRelacionada.Enabled = false;
            }
        }

        public void cargarTratamientos()
        {
            var Tratamientos = servistrat.obtenerTodos();
            if (diagnosticoActual != null)
            {
                Tratamientos = servistrat.cargarPorDiagnostico(diagnosticoActual.Codigo);
            }
            else if (facturaActual != null)
            {
                Tratamientos = servistrat.cargarPorFactura(facturaActual.ID_Factura);
            }
            DGVTratamiento.DataSource = Tratamientos;
        }

        void actualizar()
        {
            cargarTratamientos();
        }

        public Tratamiento tratamientoSeleccionado()
        {
            var IDTratamiento = DGVTratamiento.SelectedRows[0].Cells["ID_Tratamiento"].Value.ToString();

            Tratamiento tratamientoSeleccionado = servistrat.obtenerPorCodigo(IDTratamiento);
            return tratamientoSeleccionado;
        }

        public Factura facturaRelacionada()
        {
            var IDFactura = DGVTratamiento.SelectedRows[0].Cells["CodigoFactura"].Value.ToString();

            Factura facturaSeleccionada = servisFactu.obtenerPorCodigo(IDFactura);
            return facturaSeleccionada;
        }

        bool verificar()
        {
            if (DGVTratamiento.SelectedRows.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un elemento de la lista para realizar dicha acción", "Selección requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        bool validarTratamientoAsignado()
        {
            Tratamiento tratamiento = tratamientoSeleccionado();
            if (!vali.validarTratamientoAsignado(tratamiento))
            {
                MessageBox.Show("Error - Este tratamiento no presenta una factura relacionada", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        bool validarExistenteRelacion()
        {
            Tratamiento tratamiento = tratamientoSeleccionado();
            if (vali.validarTratamientoAsignado(tratamiento))
            {
                MessageBox.Show("Error - No es posible alterar un tratamiento con diagnostico/factura asignada", "Acción no realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        bool confirmar()
        {
            return MessageBox.Show("¿Está seguro que desea eliminar dicho registro?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        void abrirInformacion()
        {
            Tratamiento tratamiento = tratamientoSeleccionado();
            FrmInfoTratamiento F = new FrmInfoTratamiento(tratamiento);
            F.Show();
        }

        void abrirActualizar()
        {
            Tratamiento tratamiento = tratamientoSeleccionado();
            FrmActualizarTratamiento F = new FrmActualizarTratamiento(tratamiento);
            F.Show();
        }

        void abrirInfoFactura()
        {
            Factura factura = facturaRelacionada();
            FrmInfoFactura F = new FrmInfoFactura(factura);
            F.Show();
        }

        void eliminarTratamiento()
        {
            Tratamiento tratamiento = tratamientoSeleccionado();
            servistrat.eliminar(tratamiento);
            MessageBox.Show("Registro eliminado con exito");
        }
        void cerrar()
        {
            this.Close();
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
