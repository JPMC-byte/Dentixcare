using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ServicioTratamiento
    {
        private DBTratamiento reposTratamiento;

        public ServicioTratamiento()
        {
            reposTratamiento = new DBTratamiento();
        }

        public string guardar(Tratamiento tratamiento)
        {
            return reposTratamiento.guardarDato(tratamiento);
        }

        public List<Tratamiento> obtenerTodos()
        {
            return reposTratamiento.obtenerTodos();
        }

        public Tratamiento obtenerPorCodigo(string id)
        {
            return reposTratamiento.obtenerPorCodigo(id);
        }

        public string obtenerFacturaRelacionada(string codigoDiagnostico)
        {
            List<Tratamiento> tratamientos = cargarPorDiagnostico(codigoDiagnostico);
            if (tratamientos.Count > 0)
            {
                return tratamientos[0].CodigoFactura;
            }
            return "No se encontró factura relacionada";
        }

        public string eliminar(Tratamiento tratamiento)
        {
            return reposTratamiento.eliminarDato(tratamiento.ID_Tratamiento);
        }

        public string actualizarTratamiento(Tratamiento tratamiento, string nuevaDescripcion, string nuevaDuracion, double nuevoCosto)
        {
            Tratamiento tratamientoAModificar = obtenerPorCodigo(tratamiento.ID_Tratamiento);
            tratamientoAModificar.Descripcion = nuevaDescripcion;
            tratamientoAModificar.Duracion = nuevaDuracion;
            tratamientoAModificar.Costo = nuevoCosto;
            return reposTratamiento.actualizarDato(tratamientoAModificar);
        }

        public string actualizarForaneaDiagnostico(Tratamiento tratamiento, string Codigo)
        {
            Tratamiento tratamientoAModificar = obtenerPorCodigo(tratamiento.ID_Tratamiento);
            tratamientoAModificar.CodigoDiagnostico = Codigo;
            return reposTratamiento.actualizarDato(tratamientoAModificar);
        }

        public string actualizarForaneaFactura(Tratamiento tratamiento, string Codigo)
        {
            Tratamiento tratamientoAModificar = obtenerPorCodigo(tratamiento.ID_Tratamiento);
            tratamientoAModificar.CodigoFactura = Codigo;
            return reposTratamiento.actualizarDato(tratamientoAModificar);
        }

        public List<Tratamiento> cargarPorFactura(string codigo)
        {
            return reposTratamiento.cargarPorCodigoFactura(codigo);
        }

        public List<Tratamiento> cargarPorDiagnostico(string codigo)
        {
            return reposTratamiento.cargarPorCodigoDiagnostico(codigo);
        }
        public string generarCodigo()
        {
            List<Tratamiento> tratamientosExistentes = obtenerTodos();
            string nuevoCodigo;

            if (tratamientosExistentes == null || tratamientosExistentes.Count == 0)
            {
                nuevoCodigo = "T001";
            }
            else
            {
                Tratamiento ultimoTratamiento = tratamientosExistentes.Last();
                int ultimoCodigoNumerico = int.Parse(ultimoTratamiento.ID_Tratamiento.Substring(1));
                nuevoCodigo = 'T' + (ultimoCodigoNumerico + 1).ToString().PadLeft(3, '0');
            }
            return nuevoCodigo;
        }
    }
}
