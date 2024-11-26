using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ServicioPago
    {
        DBPago reposPago;

        public ServicioPago()
        {
            reposPago = new DBPago();
        }
        public string guardar(Pago pago)
        {
            return reposPago.guardarDato(pago);
        }

        public List<Pago> obtenerTodos()
        {
            return reposPago.obtenerTodos();
        }
        public Pago obtenerPorCodigo(string id)
        {
            return reposPago.obtenerPorCodigo(id);
        }
        public List<Pago> cargarPorFactura(string codigoFactura) 
        {
            return reposPago.cargarPorCodigoFactura(codigoFactura);
        }

        public string generarCodigo()
        {
            List<Pago> pagosExistentes = obtenerTodos();
            string nuevoCodigo;

            if (pagosExistentes.Count == 0 || pagosExistentes == null)
            {
                nuevoCodigo = "PG001";
            }
            else
            {
                Pago ultimoPago = pagosExistentes.Last();
                int ultimoCodigoNumerico = int.Parse(ultimoPago.ID_Pago.Substring(2));
                nuevoCodigo = "PG" + (ultimoCodigoNumerico + 1).ToString().PadLeft(3, '0');
            }
            return nuevoCodigo;
        }
    }
}
