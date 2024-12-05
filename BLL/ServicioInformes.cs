using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicioInformes
    {
        DBInformes repositorioInformes;

        public ServicioInformes()
        {
            repositorioInformes = new DBInformes();
        }

        public Dictionary<string, object> ObtenerCitasPorMes(int anio, int mes)
        {
            return repositorioInformes.ObtenerVistaCitaPorMes(anio, mes);
        }

        public Dictionary<string, object> ObtenerCitasPorAnio(int anio)
        {
            return repositorioInformes.ObtenerVistaCitaPorAnio(anio);
        }

        public Dictionary<string, object> ObtenerFacturasPorMes(int anio, int mes)
        {
            return repositorioInformes.ObtenerVistaFacturaPorMes(anio, mes);
        }

        public Dictionary<string, object> ObtenerFacturasPorAnio(int anio)
        {
            return repositorioInformes.ObtenerVistaFacturaPorAnio(anio);
        }

        public Dictionary<string, object> ObtenerPagosPorMes(int anio, int mes)
        {
            return repositorioInformes.ObtenerVistaPagoPorMes(anio, mes);
        }

        public Dictionary<string, object> ObtenerPagosPorAnio(int anio)
        {
            return repositorioInformes.ObtenerVistaPagoPorAnio(anio);
        }

        public Dictionary<string, object> ObtenerTotalUsuarios()
        {
            return repositorioInformes.ObtenerVistaTotalesUsuarios();
        }
    }
}
