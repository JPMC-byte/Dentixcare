using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ServicioFactura
    {
        DBFactura reposFactura;

        public ServicioFactura()
        {
            reposFactura = new DBFactura();
        }

        public string guardar(Factura factura)
        {
            return reposFactura.guardarDato(factura);
        }

        public List<Factura> obtenerTodos()
        {
            return reposFactura.obtenerTodos();
        }

        public Factura obtenerPorCodigo(string id)
        {
            return reposFactura.obtenerPorCodigo(id);
        }

        //public string actualizarEstado(Factura factura, string estado)
        //{
        //    Factura facturaAModificar = obtenerPorCodigo(factura.ID_Factura);
        //    facturaAModificar.Estado = estado;
        //    return reposFactura.actualizarDato(facturaAModificar);
        //}

        public string actualizarEstado(Factura factura, string estado)
        {
            Factura facturaAModificar = obtenerPorCodigo(factura.ID_Factura);

            if (facturaAModificar == null)
            {
                return $"Factura con ID {factura.ID_Factura} no encontrada.";
            }
            facturaAModificar.Estado = estado;

            if (factura.Total > facturaAModificar.Total)
            {
                facturaAModificar.Total = factura.Total;
            }
            return reposFactura.actualizarDato(facturaAModificar);
        }

        public string sumarMonto(Factura factura, double monto)
        {
            Factura facturaAModificar = obtenerPorCodigo(factura.ID_Factura);
            facturaAModificar.Total += monto;
            return reposFactura.actualizarDato(facturaAModificar);
        }
        public Factura sumarMontoAPagado(Factura factura, double monto)
        {
            Factura facturaAModificar = reposFactura.obtenerPorCodigo(factura.ID_Factura);
            facturaAModificar.Total_Pagado += monto;
            reposFactura.actualizarDato(facturaAModificar);

            return facturaAModificar;
        }

        public string sumarCambio(Factura factura)
        {
            Factura facturaAModificar = obtenerPorCodigo(factura.ID_Factura);
            facturaAModificar.Cambio = facturaAModificar.Total_Pagado - facturaAModificar.Total;
            return reposFactura.actualizarDato(facturaAModificar);
        }

        public string generarCodigo()
        {
            List<Factura> facturasExistentes = obtenerTodos();
            string nuevoCodigo;

            if (facturasExistentes.Count == 0 || facturasExistentes == null)
            {
                nuevoCodigo = "FC001";
            }
            else
            {
                Factura ultimaFactura = facturasExistentes.Last();
                int ultimoCodigoNumerico = int.Parse(ultimaFactura.ID_Factura.Substring(2));
                nuevoCodigo = "FC" + (ultimoCodigoNumerico + 1).ToString().PadLeft(3, '0');
            }
            return nuevoCodigo;
        }
        public List<Factura> cargarPorCedula(string Codigo)
        {
            return reposFactura.cargarPorCedulaPaciente(Codigo);
        }

    }
}