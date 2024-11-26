using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using ENTITY;
using DAL;
using System;
using System.Linq;

namespace BLL
{
    public class ServicioTwilio
    {
        private readonly string accountSid;
        private readonly string authToken;
        private readonly string twilioPhoneNumber;

        private readonly DBPaciente dbPaciente;
        private readonly DBCita dbCita;

        public ServicioTwilio(string accountSid, string authToken, string twilioPhoneNumber)
        {
            this.accountSid = accountSid;
            this.authToken = authToken;
            this.twilioPhoneNumber = twilioPhoneNumber;
            dbPaciente = new DBPaciente();
            dbCita = new DBCita();
        }

        public string enviarSMS(string numeroDestino, string mensaje)
        {
            try
            {
                numeroDestino = normalizarNumeroTelefono(numeroDestino);

                if (!esNumeroTelefonoValido(numeroDestino))
                {
                    throw new Exception("Número de teléfono no válido.");
                }

                TwilioClient.Init(accountSid, authToken);

                var message = MessageResource.Create(
                    body: mensaje,
                    from: new PhoneNumber(twilioPhoneNumber),
                    to: new PhoneNumber(numeroDestino)
                );

                return message.Sid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar SMS a {numeroDestino}: {ex.Message}");
                throw new Exception("Error al enviar el SMS: " + ex.Message);
            }
        }

        public string notificarCambioContrasena(Paciente paciente)
        {
            string citaDentUrl = "https://t.me/CitaDent_bot";
            try
            {
                if (paciente == null || string.IsNullOrEmpty(paciente.Telefono))
                {
                    throw new Exception("No se puede enviar la notificación. El paciente no tiene un número de teléfono registrado.");
                }

                string mensaje = $"Hola {paciente.PrimerNombre}, tu contraseña en CitaDent ha sido actualizada recientemente. " +
                                 $"Si no fuiste tú quien realizó este cambio, por favor contacta inmediatamente a nuestro soporte técnico en nuestro sistema:{citaDentUrl}.";

                return enviarSMS(paciente.Telefono, mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar notificación de cambio de contraseña: {ex.Message}");
                throw new Exception("No se pudo enviar la notificación de cambio de contraseña: " + ex.Message);
            }
        }

        private bool esNumeroTelefonoValido(string numero)
        {
            return !string.IsNullOrEmpty(numero) && numero.All(c => char.IsDigit(c) || c == '+') && numero.Length >= 10;
        }

        private string normalizarNumeroTelefono(string numero)
        {
            if (!numero.StartsWith("+"))
            {
                return $"+57{numero}";
            }
            return numero;
        }
    }
}
