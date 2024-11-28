using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ENTITY;
using System.Windows.Forms;
using System.Globalization;

namespace BLL
{
    public class Validaciones
    {
        ServicioPaciente servicioPaciente = new ServicioPaciente();
        ServicioOrtodoncista servicioOrtodoncista = new ServicioOrtodoncista();
        ServicioCita servicioCita = new ServicioCita();
        ServicioConsultorio servicioConsultorio = new ServicioConsultorio();
        ServicioTratamiento servicioTratamiento = new ServicioTratamiento();
        ServicioDiagnostico servicioDiagnostico = new ServicioDiagnostico();
        ServicioPago servicioPago = new ServicioPago();

        public Validaciones() { }

        public bool validarTeclasControl(KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return true;
            }
            return false;
        }

        public bool validarLetras(KeyPressEventArgs e)
        {
            if (validarTeclasControl(e)) return true;
            string Patron = @"^[a-zA-ZñÑ\s]$";
            bool esValido = Regex.IsMatch(e.KeyChar.ToString(), Patron);
            return esValido;
        }

        public bool EsTextoValido(string texto)
        {
            return texto.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }

        public bool validarNumeros(KeyPressEventArgs e)
        {
            if (validarTeclasControl(e)) return true;
            string Patron = @"^[0-9]$";
            bool esValido = Regex.IsMatch(e.KeyChar.ToString(), Patron);
            return esValido;
        }

        public bool EsNumeroTelefonoValido(string numero)
        {
            return numero.All(c => char.IsDigit(c) || c == '+' || c == '-' || c == ' ');
        }

        public bool EsIdentificacionValida(string ID)
        {
            return !string.IsNullOrEmpty(ID) && ID.All(char.IsDigit) && ID.Length >= 5;
        }

        public bool EsNumeroTarjetaValido(string numeroTarjeta)
        {
            if (numeroTarjeta.Length != 16 || !numeroTarjeta.All(char.IsDigit))
            {
                return false;
            }

            int sum = 0;
            bool alternate = false;
            for (int i = numeroTarjeta.Length - 1; i >= 0; i--)
            {
                int n = int.Parse(numeroTarjeta[i].ToString());
                if (alternate)
                {
                    n *= 2;
                    if (n > 9)
                    {
                        n -= 9;
                    }
                }
                sum += n;
                alternate = !alternate;
            }
            return (sum % 10 == 0);
        }

        public bool ValidarNumeroTarjeta(string numeroTarjeta)
        {
            return EsNumeroTarjetaValido(numeroTarjeta);
        }

        public bool validarExistentePaciente(string Texto)
        {
            Paciente paciente = new Paciente();
            paciente = servicioPaciente.obtenerPorCodigo(Texto);

            if (paciente == null)
            {
                return true;
            }
            return false;
        }

        public bool EsFechaExpiracionValida(string fecha)
        {
            if (DateTime.TryParseExact(fecha, "MM/yy", null, System.Globalization.DateTimeStyles.None, out DateTime expiracion))
            {
                expiracion = expiracion.AddMonths(1).AddDays(-1);
                return expiracion >= DateTime.Now;
            }
            return false;
        }

        public bool EsCVVValido(string cvv)
        {
            return cvv.All(char.IsDigit) && (cvv.Length == 3 || cvv.Length == 4);
        }

        public bool validarExistenteOrtodoncista(string Texto)
        {
            Ortodoncista ortodoncista = new Ortodoncista();
            ortodoncista = servicioOrtodoncista.obtenerPorCodigo(Texto);

            if (ortodoncista == null)
            {
                return true;
            }
            return false;
        }

        public bool validarExistenteFactura(string Texto)
        {
            List<Tratamiento> lista = new List<Tratamiento>();
            lista = servicioTratamiento.cargarPorDiagnostico(Texto);
            if (lista.Count > 0)
            {
                return true;
            }
            return false;
        }

        public bool validarHorario(TimeSpan Hora, DateTime Dia)
        {
            TimeSpan duracionCita = new TimeSpan(1, 0, 0);
            TimeSpan finNuevaCita = Hora.Add(duracionCita);
            List<Cita> listaCitas = new List<Cita>();
            listaCitas = servicioCita.obtenerTodos();

            if (listaCitas.Count == 0 || listaCitas == null)
            {
                return true;
            }

            foreach (var cita in listaCitas)
            {
                if (cita.Fecha_Cita.Date == Dia.Date)
                {
                    TimeSpan finCitaExistente = cita.Hora_Cita.Add(duracionCita);

                    bool haySuperposicion = (Hora < finCitaExistente) && (finNuevaCita > cita.Hora_Cita);

                    if (haySuperposicion)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool validarFechaFutura(DateTime fechaCita)
        {
            return fechaCita.Date > DateTime.Now.Date;
        }

        public bool validarAperturaCierre(TimeSpan Hora)
        {
            Consultorio consul = new Consultorio();
            consul = servicioConsultorio.cargarConsultorio("P101");

            bool horaNoDisponibilidad = (Hora < consul.Hora_Apertura) || (Hora > consul.Hora_Cierre);
            if (horaNoDisponibilidad)
            {
                return false;
            }
            return true;
        }

        public bool validarSegundosNombres(string Texto)
        {
            if (Texto == "SEGUNDO NOMBRE")
            {
                return false;
            }
            if (Texto == "SEGUNDO APELLIDO")
            {
                return false;
            }
            return true;
        }

        public bool validarAtendida(string texto)
        {
            if (texto == "Pendiente" || texto == "Finalizada")
            {
                return false;
            }
            return true;
        }

        public bool validarFiltroEstado(bool activo, string texto)
        {
            if (!activo || texto == "N/A")
            {
                return false;
            }
            return true;
        }

        public bool validarFiltroPaciente(bool activo, string texto)
        {
            Cita cita = servicioCita.obtenerPorCedula(texto);
            Paciente paciente = servicioPaciente.obtenerPorCodigo(texto);
            if (!activo || texto == "CEDULA DEL PACIENTE" || cita == null || paciente == null)
            {
                return false;
            }
            return true;
        }

        public bool validarFiltroFecha(bool activo, DateTime fecha)
        {
            if (!activo || fecha.Date > DateTime.Now.Date)
            {
                return false;
            }
            return true;
        }

        public bool validarAntedecentes(Persona persona)
        {
            List<Diagnostico> listaDiagnosticos = servicioDiagnostico.cargarPorCedula(persona.Cedula);

            if (listaDiagnosticos.Count == 0 || listaDiagnosticos == null) return false;

            return true;
        }

        public bool validarTratamientosPerDiagnostico(Diagnostico diagnostico)
        {
            List<Tratamiento> listaTratamientos = servicioTratamiento.cargarPorDiagnostico(diagnostico.Codigo);

            if (listaTratamientos.Count == 0 || listaTratamientos == null) return false;

            return true;
        }

        public bool validarPagos(Factura factura)
        {
            List<Pago> listaPagos = servicioPago.cargarPorFactura(factura.ID_Factura);

            if (listaPagos.Count == 0 || listaPagos == null) return false;

            return true;
        }

        public bool validarTratamientoAsignado(Tratamiento tratamiento)
        {
            if (tratamiento.CodigoDiagnostico == "" || tratamiento.CodigoFactura == "")
            {
                return false;
            }
            return true;
        }

        public bool validarMonto(double monto)
        {
            if (monto <= 0)
            {
                return false;
            }
            return true;
        }

        public bool validarFacturaPagada(Factura factura)
        {
            if (factura.Total_Pagado >= factura.Total || factura.Estado == "Finalizada")
            {
                return true;
            }
            return false;
        }

        public bool validarMetodoPago(string texto)
        {
            if (texto == "" || texto == "N/A")
            {
                return false;
            }
            return true;
        }

        public string NormalizarCadena(string texto)
        {
            string textoFormateado = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in textoFormateado)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || c == '-' || c == '/')
                    {
                        sb.Append(char.ToLowerInvariant(c));
                    }
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC).Trim();
        }
    }
}
