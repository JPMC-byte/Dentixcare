using DAL;
using ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BLL
{
    public class ServicioCita
    {
        // RepositorioCita reposCita;
        DBCita reposCita;

        public ServicioCita()
        {
            //reposCita = new RepositorioCita(Config.FILENAME_CITA);
            reposCita = new DBCita();
        }
        public string guardar(Cita cita)
        {
            return reposCita.guardarDatos(cita);
        }
        public List<Cita> obtenerTodos()
        {
            return reposCita.obtenerTodos();
        }
        public Cita obtenerPorCodigo(string id) 
        {
            return reposCita.obtenerPorCodigo(id);
        }
        public Cita obtenerPorCedula(string id)
        {
            return reposCita.obtenerPorCedula(id);
        }
        public List<Cita> cargarPorCodigo(string id)
        {
            return reposCita.cargarPorCodigo(id);
        }
        public List<Cita> cargarPorEstado(string Estado)
        {
            return reposCita.cargarPorEstado(Estado);
        }
        public List<Cita> cargarPorFiltrosGui(string Estado, string Cedula)
        {
            return reposCita.cargarPorFiltrosGui(Estado, Cedula);
        }

        public string actualizarFecha(Cita cita)
        {
            return reposCita.modificarDato(cita);
        }

        public List<Cita> cargarPorFiltros(string estado1, string estado2, string cedulaPaciente)
        {
            var citas = obtenerTodos();

            var citasFiltradas = citas.Where(cita => cita.CodigoPaciente != null &&
                                                     cita.CodigoPaciente.Trim() == cedulaPaciente.Trim() &&
                                                     (cita.Estado.Equals(estado1, StringComparison.OrdinalIgnoreCase) ||
                                                      cita.Estado.Equals(estado2, StringComparison.OrdinalIgnoreCase)))
                                      .ToList();
            return citasFiltradas;
        }

        public string actualizarHora(string codigoCita, TimeSpan nuevaHora)
        {
            var cita = obtenerPorCodigo(codigoCita);
            if (cita != null)
            {
                cita.Hora_Cita = nuevaHora;
                cita.Estado = "Solicitada";
                return reposCita.modificarDato(cita);
            }
            return "Cita no encontrada";
        }

        public string generarCodigo()
        {
            List<Cita> citasExistentes = obtenerTodos();
            string nuevoCodigo;

            if (citasExistentes.Count == 0 || citasExistentes == null)
            {
                nuevoCodigo = "001";
            }
            else
            {
                Cita ultimaCita = citasExistentes.Last();
                int ultimoCodigoNumerico = int.Parse(ultimaCita.Codigo);
                nuevoCodigo = (ultimoCodigoNumerico + 1).ToString().PadLeft(3, '0');
            }
            return nuevoCodigo;
        }
        public string actualizarRazon(Cita cita, string RazonCita)
        {
            Cita CitaAModificar = obtenerPorCodigo(cita.Codigo);
            CitaAModificar.Razon_Cita = RazonCita;
            return reposCita.modificarDato(CitaAModificar);
        }
        public string actualizarAtendida(Cita cita, string CodigoOrtodoncista, string estado)
        {
            Cita CitaAModificar = obtenerPorCodigo(cita.Codigo);
            CitaAModificar.CodigoOrtodoncista = CodigoOrtodoncista;
            CitaAModificar.Estado = estado;
            return reposCita.modificarDato(CitaAModificar);
        }

        public string eliminar(Cita cita)
        {
            return reposCita.eliminarDato(cita.Codigo);
        }
    }
}
