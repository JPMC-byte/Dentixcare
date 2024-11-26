using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ServicioDiagnostico
    {
        private DBDiagnostico reposDiagnostico;

        public ServicioDiagnostico()
        {
            reposDiagnostico = new DBDiagnostico();
        }

        public string guardar(Diagnostico diagnostico)
        {
            return reposDiagnostico.guardarDato(diagnostico);
        }

        public List<Diagnostico> obtenerTodos()
        {
            return reposDiagnostico.obtenerTodos();
        }

        public Diagnostico obtenerPorCodigo(string codigo)
        {
            return reposDiagnostico.obtenerPorCodigo(codigo);
        }

        public string eliminar(Diagnostico diagnostico)
        {
            return reposDiagnostico.eliminarDato(diagnostico.Codigo);
        }
        public string actualizar(Diagnostico diagnostico, string diagnosticoDescripcion)
        {
            Diagnostico diagnosticoAModificar = obtenerPorCodigo(diagnostico.Codigo);
            diagnosticoAModificar.Descripcion = diagnosticoDescripcion;
            return reposDiagnostico.actualizarDato(diagnosticoAModificar);
        }
        public List<Diagnostico> cargarPorCedula(string codigo) 
        {
            return reposDiagnostico.cargarPorCedula(codigo);
        }
        public List<Diagnostico> cargarPorFecha(DateTime fecha)
        {
            return reposDiagnostico.cargarPorFecha(fecha);
        }
        public List<Diagnostico> cargarPorFiltros(DateTime fecha, string IDPaciente)
        {
            return reposDiagnostico.cargarPorFiltros(fecha, IDPaciente);
        }
        public string generarCodigo()
        {
            List<Diagnostico> diagnosticosExistentes = obtenerTodos();
            string nuevoCodigo;

            if (diagnosticosExistentes == null || diagnosticosExistentes.Count == 0)
            {
                nuevoCodigo = "D001";
            }
            else
            {
                Diagnostico ultimoDiagnostico = diagnosticosExistentes.Last();
                int ultimoCodigoNumerico = int.Parse(ultimoDiagnostico.Codigo.Substring(1));
                nuevoCodigo = 'D' + (ultimoCodigoNumerico + 1).ToString().PadLeft(3, '0');
            }

            return nuevoCodigo;
        }
    }
}
