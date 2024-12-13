using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using ENTITY;
using DAL;
using Logica;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;

namespace BLL
{
    public class ServicioBot
    {
        private readonly TelegramBotClient _botClient;
        private readonly ServicioPago _servicioPago;
        private readonly ServicioFactura _servicioFactura;
        private readonly ServicioPaciente _servicioPaciente;
        private readonly ServicioCita _servicioCita;
        private readonly DBConsultorio _dbConsultorio;
        private readonly ServicioOrtodoncista _servicioOrtodoncista;
        private readonly ServicioDiagnostico _servicioDiagnostico;
        private readonly Validaciones _validaciones;
        private string cedulaPacienteIniciada;
        private string telefonoPacienteIniciado;
        private bool _esperandoEmergencia = false;
        private bool usuarioAutenticado = false;
        private bool enAgendarCita = false;
        private Cita citaEnProceso;
        private bool enInicioSesion = false;
        private string cedulaInicioSesion = "";
        private bool enRegistro = false;
        private Paciente pacienteEnProceso;
        private Dictionary<long, string> comandosPendientes = new Dictionary<long, string>();
        private readonly ServicioTwilio _servicioTwilio;
        private readonly ServicioIA _servicioIA;
        private bool esperandoSeleccionRecordatorios = false;
        private bool enModificarContrasena = false;
        private bool esperandoNuevaContrasena = false;
        private bool enGestionCita = false;
        private bool enModificarCita = false;
        private bool enEsperaNuevoMotivo = false;
        private bool enEsperaNuevaFecha = false;
        private bool enEsperaNuevaHora = false;
        private string citaSeleccionada = null;
        private bool esperandoMetodoPago = false;
        private bool esperandoIdFactura = false;
        private bool esperandoProcesoPago = false;
        private string montoEnProceso = null;
        private Factura facturaEnProceso = null;
        private bool esperandoDatosTarjeta = false;
        private bool esperandoFechaExpiracion = false;
        private bool esperandoCVV = false;
        private bool esperandoSeleccionCuotas = false;
        private string tipoTarjetaEnProceso = null;
        private string numeroTarjetaEnProceso = null;
        private string fechaExpiracionEnProceso = null;
        private string cvvEnProceso = null;
        private int cuotasEnProceso = 0;

        public ServicioBot(string token)
        {
            _servicioTwilio = new ServicioTwilio(
                "ACecaed8e25095ff1a38a7540288b4c848",
                "a71b5b1a7b1e0c1f13df5bf1643262a4",
                "+14842578999"
            );
            //Clave secreta con la cual PROBAR EL SERVICIO IA de OpenIA, debido a problemas de seguridad de OpenIA: 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 
            //Clave necesaria para no dar error pero NO FUNCIONAL EN EL SISTEMA: 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
            _servicioIA = new ServicioIA("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");
            _botClient = new TelegramBotClient(token);
            _servicioPaciente = new ServicioPaciente();
            _servicioPago = new ServicioPago();
            _servicioFactura = new ServicioFactura();
            _servicioCita = new ServicioCita();
            _dbConsultorio = new DBConsultorio();
            _servicioOrtodoncista = new ServicioOrtodoncista();
            _servicioDiagnostico = new ServicioDiagnostico();
            _validaciones = new Validaciones();
        }

        public void Start()
        {
            var cts = new CancellationTokenSource();
            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, cancellationToken: cts.Token);
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            string errorMessage;
            if (exception is ApiRequestException apiRequestException)
            {
                errorMessage = $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}";
            }
            else
            {
                errorMessage = exception.ToString();
            }

            Console.WriteLine(errorMessage);
            return Task.CompletedTask;
        }

        private async Task ProcesarComandoStart(long chatId)
        {
            enRegistro = false;
            enInicioSesion = false;
            enAgendarCita = false;
            enGestionCita = false;
            enModificarCita = false;
            pacienteEnProceso = null;
            cedulaInicioSesion = null;
            cedulaPacienteIniciada = null;
            telefonoPacienteIniciado = null;
            usuarioAutenticado = false;
            await MostrarMenuBienvenida(chatId);
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update.Message != null && update.Message.Text != null)
            {
                string messageText = update.Message.Text;
                long chatId = update.Message.Chat.Id;

                if (_esperandoEmergencia)
                {
                    await ProcesarEmergenciaDeOrtodoncia(chatId, messageText);
                    return;
                }

                if (esperandoDatosTarjeta || esperandoFechaExpiracion || esperandoCVV)
                {
                    await ProcesarDatosTarjeta(chatId, messageText);
                    return;
                }
                else if (esperandoSeleccionCuotas)
                {
                    await ProcesarSeleccionCuotas(chatId, messageText);
                    return;
                }

                if (messageText == "Modificar o Cancelar Citas" || enGestionCita || enModificarCita)
                {
                    await GestionarCitas(chatId, messageText);
                    return;
                }

                if (messageText == "/start")
                {
                    await ProcesarComandoStart(chatId);
                    return;
                }
                else if (messageText == "Registrar")
                {
                    await IniciarRegistro(chatId);
                    return;
                }
                else if (messageText == "Iniciar Sesión")
                {
                    await IniciarSesion(chatId);
                    return;
                }

                if (messageText == "Cambiar Contraseña" || enModificarContrasena)
                {
                    if (enModificarContrasena)
                        await ProcesarModificarContrasena(chatId, messageText);
                    else
                        await IniciarModificarContrasena(chatId);
                    return;
                }
                else if (esperandoNuevaContrasena)
                {
                    await ProcesarNuevaContrasena(chatId, messageText);
                    return;
                }

                if (enRegistro)
                {
                    await ProcesarPasoRegistro(chatId, messageText);
                    return;
                }

                if (enInicioSesion)
                {
                    await ProcesarInicioSesion(chatId, messageText);
                    return;
                }

                if (enAgendarCita)
                {
                    await ProcesarPasoAgendarCita(chatId, messageText);
                    return;
                }

                if (esperandoRespuestaPreguntas || messageText == "1. ¿Cómo agendo una cita?" || messageText == "2. ¿Cómo configuro recordatorios?" || messageText == "3. ¿Cómo cambio mi contraseña?")
                {
                    await ProcesarRespuestaPreguntas(chatId, messageText);
                    return;
                }

                if (messageText == "Emergencia de Ortodoncia")
                {
                    await ManejarEmergenciaDeOrtodoncia(chatId);
                    return;
                }

                await ProcesarComandoPrincipal(chatId, messageText);
            }
        }

        private async Task MostrarMenuBienvenida(long chatId, bool mostrarCambiarContrasena = false)
        {
            var opciones = new List<KeyboardButton[]>
            {
                new KeyboardButton[] { "Registrar", "Iniciar Sesión" }
            };

            if (mostrarCambiarContrasena)
            {
                opciones.Add(new KeyboardButton[] { "Cambiar Contraseña" });
            }

            var replyKeyboard = new ReplyKeyboardMarkup(opciones)
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "¡Bienvenido al sistema de atención al cliente de CitaDent! Por favor, selecciona una opción:", replyKeyboard);
        }

        private async Task MostrarMenuNoTengo(long chatId, string campo)
        {
            var menuNoTengo = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "No tengo" }
                })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await SendMessage(chatId, $"Por favor, selecciona la opción para tu {campo}, en caso de no contar con {campo} o digitalo:", menuNoTengo);
        }

        private async Task IniciarRegistro(long chatId)
        {
            enRegistro = true;
            pacienteEnProceso = new Paciente();
            await SendMessage(chatId, "Por favor, ingresa tu numero de identificación para comenzar el registro.");
        }

        private async Task ProcesarPasoRegistro(long chatId, string mensaje)
        {
            mensaje = mensaje.Trim();

            if (string.IsNullOrEmpty(pacienteEnProceso.Cedula))
            {
                if (!_validaciones.EsIdentificacionValida(mensaje))
                {
                    await SendMessage(chatId, "La cédula solo debe contener números y tener un mínimo de cinco dígitos, además de no ser negativa. Por favor, ingrésala nuevamente.");
                    return;
                }

                var pacienteExistente = _servicioPaciente.obtenerPorCodigo(mensaje);
                if (pacienteExistente != null)
                {
                    await SendMessage(chatId, "El paciente ya está registrado. Usa la opción de iniciar sesión o escribe 'Recuperar contraseña' si olvidaste tu contraseña.");
                    enRegistro = false;
                    pacienteEnProceso = null;
                    return;
                }

                pacienteEnProceso.Cedula = mensaje;
                pacienteEnProceso.CodigoConsultorio = "P101";
                await SendMessage(chatId, "Por favor, ingresa tu primer nombre.");
                return;
            }
            else if (string.IsNullOrEmpty(pacienteEnProceso.PrimerNombre))
            {
                if (!_validaciones.EsTextoValido(mensaje))
                {
                    await SendMessage(chatId, "El primer nombre no debe contener números ni caracteres especiales. Por favor, ingrésalo nuevamente.");
                    return;
                }

                pacienteEnProceso.PrimerNombre = mensaje;
                await MostrarMenuNoTengo(chatId, "segundo nombre");
                return;
            }
            else if (pacienteEnProceso.SegundoNombre == null)
            {
                if (mensaje.Equals("No tengo", StringComparison.OrdinalIgnoreCase))
                {
                    pacienteEnProceso.SegundoNombre = "";
                }
                else if (!_validaciones.EsTextoValido(mensaje))
                {
                    await SendMessage(chatId, "El segundo nombre no debe contener números ni caracteres especiales. Por favor, ingrésalo nuevamente o selecciona una opción.");
                    await MostrarMenuNoTengo(chatId, "segundo nombre");
                    return;
                }
                else
                {
                    pacienteEnProceso.SegundoNombre = mensaje;
                }

                await SendMessage(chatId, "Por favor, ingresa tu primer apellido.");
                return;
            }
            else if (string.IsNullOrEmpty(pacienteEnProceso.PrimerApellido))
            {
                if (!_validaciones.EsTextoValido(mensaje))
                {
                    await SendMessage(chatId, "El primer apellido no debe contener números ni caracteres especiales. Por favor, ingrésalo nuevamente.");
                    return;
                }

                pacienteEnProceso.PrimerApellido = mensaje;
                await MostrarMenuNoTengo(chatId, "segundo apellido");
                return;
            }
            else if (pacienteEnProceso.SegundoApellido == null)
            {
                if (mensaje.Equals("No tengo", StringComparison.OrdinalIgnoreCase))
                {
                    pacienteEnProceso.SegundoApellido = "";
                }
                else if (!_validaciones.EsTextoValido(mensaje))
                {
                    await SendMessage(chatId, "El segundo apellido no debe contener números ni caracteres especiales. Por favor, ingrésalo nuevamente o selecciona una opción.");
                    await MostrarMenuNoTengo(chatId, "segundo apellido");
                    return;
                }
                else
                {
                    pacienteEnProceso.SegundoApellido = mensaje;
                }

                await SendMessage(chatId, "Por favor, ingresa tu número de teléfono.");
                return;
            }
            else if (string.IsNullOrEmpty(pacienteEnProceso.Telefono))
            {
                if (!_validaciones.EsNumeroTelefonoValido(mensaje) && mensaje.Length < 10)
                {
                    await SendMessage(chatId, "El número de teléfono solo debe contener dígitos y debe ser de minimo 10 digitos. Por favor, ingrésalo nuevamente.");
                    return;
                }

                pacienteEnProceso.Telefono = mensaje;
                await SendMessage(chatId, "Por favor, ingresa tu fecha de nacimiento en formato DD/MM/YYYY.");
                return;
            }
            else if (pacienteEnProceso.Fecha_De_Nacimiento == default(DateTime))
            {
                if (DateTime.TryParseExact(mensaje, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fechaNacimiento))
                {
                    int edad = DateTime.Now.Year - fechaNacimiento.Year;
                    if (fechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;

                    if (edad < 0 || edad > 120)
                    {
                        await SendMessage(chatId, "Fecha de nacimiento inválida. Por favor, ingresa una fecha realista.");
                        return;
                    }

                    pacienteEnProceso.Fecha_De_Nacimiento = fechaNacimiento;
                    await SendMessage(chatId, "Por favor, crea una contraseña para tu cuenta.");

                }
                else
                {
                    await SendMessage(chatId, "Formato de fecha inválido. Por favor, ingresa la fecha en formato DD/MM/YYYY.");
                    return;
                }
                return;
            }
            if (string.IsNullOrEmpty(pacienteEnProceso.Contrasena))
            {
                if (mensaje.Length < 6)
                {
                    await SendMessage(chatId, "La contraseña debe tener al menos 6 caracteres. Por favor, ingrésala nuevamente.");
                    return;
                }

                pacienteEnProceso.Contrasena = mensaje;

                string resultado = _servicioPaciente.guardar(pacienteEnProceso);

                await SendMessage(chatId, resultado == "Registro exitoso"
                    ? "Registro completado exitosamente. ¡Bienvenido a CitaDent!"
                    : "Error al registrar la información. Inténtalo nuevamente.");

                if (resultado == "Registro exitoso")
                {
                    cedulaPacienteIniciada = pacienteEnProceso.Cedula;
                    usuarioAutenticado = true;
                    await MostrarMenuPrincipal(chatId);
                }

                enRegistro = false;
                pacienteEnProceso = null;
            }
        }

        private async Task IniciarModificarContrasena(long chatId)
        {
            enModificarContrasena = true;
            await SendMessage(chatId, "Por favor, ingresa tu número de identificación para verificar tu identidad.");
        }

        private async Task ProcesarModificarContrasena(long chatId, string mensaje)
        {
            if (!enModificarContrasena)
                return;

            mensaje = mensaje.Trim();

            if (!_validaciones.EsIdentificacionValida(mensaje))
            {
                await SendMessage(chatId, "El número de cédula ingresado no es válido. Asegúrate de que solo contenga números y tenga una longitud correcta.");
                return;
            }

            var paciente = _servicioPaciente.obtenerPorCodigo(mensaje);
            if (paciente == null)
            {
                await SendMessage(chatId, "No se encontró un usuario con esa cédula. Por favor, verifica e ingrésala nuevamente o registrate.");
                return;
            }

            cedulaPacienteIniciada = mensaje;
            await SendMessage(chatId, "Identidad verificada. Por favor, ingresa tu nueva contraseña.");
            enModificarContrasena = false;
            esperandoNuevaContrasena = true;
        }

        private async Task ProcesarNuevaContrasena(long chatId, string mensaje)
        {
            if (!esperandoNuevaContrasena)
                return;

            if (mensaje.Length < 6)
            {
                await SendMessage(chatId, "La nueva contraseña debe tener al menos 6 caracteres. Por favor, ingrésala nuevamente.");
                return;
            }

            var resultado = _servicioPaciente.actualizarContrasena(cedulaPacienteIniciada, mensaje);

            if (resultado == "Contraseña actualizada exitosamente")
            {
                await SendMessage(chatId, "¡Tu contraseña ha sido actualizada con éxito!");

                var paciente = _servicioPaciente.obtenerPorCodigo(cedulaPacienteIniciada);
                if (paciente != null)
                {
                    try
                    {
                        _servicioTwilio.notificarCambioContrasena(paciente);
                        await SendMessage(chatId, "🔔 Se ha enviado una notificación al número registrado informando sobre el cambio de contraseña.");
                    }
                    catch (Exception ex)
                    {
                        await SendMessage(chatId, $"⚠️ Error al enviar la notificación de cambio de contraseña: {ex.Message}");
                    }
                }
            }
            else
            {
                await SendMessage(chatId, "Hubo un problema al actualizar tu contraseña. Por favor, inténtalo nuevamente.");
            }

            esperandoNuevaContrasena = false;
        }

        private async Task IniciarSesion(long chatId)
        {
            enInicioSesion = true;
            cedulaInicioSesion = "";
            await SendMessage(chatId, "Por favor, ingresa tu numero de identificación para iniciar sesión.");
        }

        private int intentosFallidos = 0;

        public async Task ProcesarInicioSesion(long chatId, string mensaje)
        {
            if (string.IsNullOrEmpty(cedulaInicioSesion))
            {
                if (!_validaciones.EsIdentificacionValida(mensaje))
                {
                    await SendMessage(chatId, "La cédula ingresada no es válida. Asegúrate de que solo contenga números y tenga una longitud correcta, además de asegurarse de que no sea negativa.");
                    return;
                }

                cedulaInicioSesion = mensaje;
                await SendMessage(chatId, "Ahora, por favor, ingresa tu contraseña.");
            }
            else
            {
                string contrasena = mensaje;

                if (string.IsNullOrEmpty(contrasena))
                {
                    await SendMessage(chatId, "La contraseña no puede estar vacía. Por favor, ingrésala nuevamente.");
                    return;
                }

                var paciente = _servicioPaciente.iniciarSesion(cedulaInicioSesion, contrasena);

                if (paciente != null)
                {
                    usuarioAutenticado = true;
                    cedulaPacienteIniciada = paciente.Cedula;
                    telefonoPacienteIniciado = paciente.Telefono;
                    enInicioSesion = false;
                    intentosFallidos = 0;

                    await SendMessage(chatId, $"¡Bienvenido, {paciente.PrimerNombre} {paciente.PrimerApellido}!");
                    await MostrarMenuPrincipal(chatId);
                }

                else
                {
                    intentosFallidos++;

                    if (intentosFallidos >= 3)
                    {
                        await SendMessage(chatId, "Credenciales incorrectas. Ahora tienes la opción de cambiar tu contraseña.");
                        await MostrarMenuBienvenida(chatId, true);
                        intentosFallidos = 0;
                        cedulaInicioSesion = "";
                    }
                    else
                    {
                        await SendMessage(chatId, $"Credenciales incorrectas. Te quedan {3 - intentosFallidos} intentos. Por favor, vuelve a ingresar tu contraseña.");
                    }
                }
            }
        }

        private async Task AgendarCita(long chatId)
        {
            if (!usuarioAutenticado)
            {
                await SendMessage(chatId, "Debes iniciar sesión o registrarte antes de agendar una cita.");
                await MostrarMenuBienvenida(chatId);
                return;
            }

            enAgendarCita = true;
            citaEnProceso = new Cita
            {
                Codigo = _servicioCita.generarCodigo(),
                Estado = "Solicitada"
            };

            await MostrarMenuFechas(chatId);
        }

        public async Task ProcesarPasoAgendarCita(long chatId, string mensaje)
        {
            if (citaEnProceso.Fecha_Cita == default(DateTime))
            {
                if (mensaje.ToLower() == "otra fecha")
                {
                    await SendMessage(chatId, "Por favor, ingresa la fecha de la cita en formato DD-MM-YYYY.");
                    return;
                }

                if (DateTime.TryParseExact(mensaje, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
                {
                    if (fecha.Date < DateTime.Now.Date)
                    {
                        await SendMessage(chatId, "La fecha de la cita no puede ser en el pasado. Por favor, selecciona una fecha válida.");
                        await MostrarMenuFechas(chatId);
                        return;
                    }

                    citaEnProceso.Fecha_Cita = fecha;
                    await MostrarMenuHoras(chatId);
                }
                else
                {
                    await SendMessage(chatId, "Formato de fecha inválido. Por favor, ingresa la fecha en formato DD/MM/YYYY.");
                    return;
                }
                return;
            }

            if (citaEnProceso.Hora_Cita == default(TimeSpan))
            {
                mensaje = _validaciones.NormalizarCadena(mensaje);
                if (TimeSpan.TryParseExact(mensaje, "hhmm", null, out TimeSpan hora))
                {
                    TimeSpan horaInicio = new TimeSpan(8, 0, 0);
                    TimeSpan horaFin = new TimeSpan(17, 30, 0);

                    if (hora < horaInicio || hora > horaFin)
                    {
                        await SendMessage(chatId, "La hora de la cita debe estar dentro del horario laboral (08:00 - 17:30). Selecciona una opción válida.");
                        await MostrarMenuHoras(chatId);
                        return;
                    }

                    DateTime fechaHoraCita = citaEnProceso.Fecha_Cita.Add(hora);
                    if (fechaHoraCita < DateTime.Now)
                    {
                        await SendMessage(chatId, "La fecha y hora de la cita no pueden ser en el pasado. Por favor, selecciona una hora futura.");
                        await MostrarMenuHoras(chatId);
                        return;
                    }

                    citaEnProceso.Hora_Cita = hora;
                    await MostrarMenuMotivos(chatId);
                }
                else
                {
                    await SendMessage(chatId, "Formato de hora inválido. Selecciona una opción válida.");
                    await MostrarMenuHoras(chatId);
                }
                return;
            }

            if (string.IsNullOrEmpty(citaEnProceso.Razon_Cita))
            {
                if (mensaje.ToLower() == "otros")
                {
                    await SendMessage(chatId, "Por favor, escribe el motivo de la cita.");
                }
                else
                {
                    citaEnProceso.Razon_Cita = mensaje;
                    await MostrarMenuOrtodoncistas(chatId);
                }
                return;
            }

            if (string.IsNullOrEmpty(citaEnProceso.CodigoOrtodoncista))
            {
                if (mensaje.ToLower() == "ortodoncista de preferencia")
                {
                    await SendMessage(chatId, "Por favor, escribe el primer nombre del ortodoncista de tu preferencia.");
                    return;
                }

                if (mensaje.ToLower() == "cualquier ortodoncista")
                {
                    var ortodoncistass = _servicioOrtodoncista.obtenerTodos();
                    if (ortodoncistass.Count > 0)
                    {
                        var random = new Random();
                        var ortodoncistaAleatorio = ortodoncistass[random.Next(ortodoncistass.Count)];
                        citaEnProceso.CodigoOrtodoncista = ortodoncistaAleatorio.Cedula;

                        await SendMessage(chatId, $"Se asignó el ortodoncista {ortodoncistaAleatorio.PrimerNombre} {ortodoncistaAleatorio.PrimerApellido}.");
                        await SendMessage(chatId, "¿Deseas activar recordatorios para esta cita? Responde con 'Si' o 'No'.");
                    }
                    else
                    {
                        await SendMessage(chatId, "No hay ortodoncistas disponibles. Por favor, inténtalo más tarde.");
                    }
                    return;
                }

                var ortodoncistas = _servicioOrtodoncista.obtenerPorPrimerNombre(mensaje);
                if (ortodoncistas.Count > 0)
                {
                    await SendMessage(chatId, "Lista de Ortodoncistas con ese nombre:");
                    foreach (var ortodoncista in ortodoncistas)
                    {
                        string detallesOrtodoncista = $"👨‍⚕️ Ortodoncista: {ortodoncista.PrimerNombre} {ortodoncista.PrimerApellido}\n" +
                                                      $"🆔 Cédula: {ortodoncista.Cedula}\n" +
                                                      $"📞 Teléfono: {ortodoncista.Telefono}\n" +
                                                      $"🏢 Consultorio: {ortodoncista.CodigoConsultorio ?? "No asignado"}\n" +
                                                      $"-------------------------------------------";
                        await SendMessage(chatId, detallesOrtodoncista);
                    }
                    await SendMessage(chatId, "Por favor, escribe la cédula del ortodoncista que prefieres.");
                    return;
                }
                else
                {
                    var ortodoncistaSeleccionado = _servicioOrtodoncista.obtenerPorCodigo(mensaje);
                    if (ortodoncistaSeleccionado != null)
                    {
                        citaEnProceso.CodigoOrtodoncista = ortodoncistaSeleccionado.Cedula;
                        await SendMessage(chatId, $"Ortodoncista seleccionado: {ortodoncistaSeleccionado.PrimerNombre} {ortodoncistaSeleccionado.PrimerApellido}.");
                        await SendMessage(chatId, "¿Deseas activar recordatorios para esta cita? Responde con 'Si' o 'No'.");
                        return;
                    }
                    else
                    {
                        await SendMessage(chatId, "No se encontró un ortodoncista con esa cédula. Inténtalo nuevamente.");
                    }
                }
                return;
            }

            if (citaEnProceso.RecordatorioCita == null)
            {
                if (mensaje.ToLower() == "si" || mensaje.ToLower() == "sí")
                {
                    citaEnProceso.RecordatorioCita = true;
                }
                else if (mensaje.ToLower() == "no")
                {
                    citaEnProceso.RecordatorioCita = false;
                }
                else
                {
                    await SendMessage(chatId, "Respuesta no válida. Por favor, responde con 'Sí' o 'No'.");
                    return;
                }
            }

            citaEnProceso.CodigoPaciente = cedulaPacienteIniciada;
            citaEnProceso.CodigoConsultorio = "P101";
            citaEnProceso.Fecha_Creacion = DateTime.Now;

            string resultado = _servicioCita.guardar(citaEnProceso);
            if (resultado == "Cita registrada exitosamente")
            {
                await SendMessage(chatId, $"Cita agendada exitosamente.\n\nDetalles de la cita:\n- Fecha: {citaEnProceso.Fecha_Cita:dd/MM/yyyy}\n- Hora: {citaEnProceso.Hora_Cita:hh\\:mm}\n- Motivo: {citaEnProceso.Razon_Cita}\n- Consultorio: {citaEnProceso.CodigoConsultorio}\n- Recordatorios: {(citaEnProceso.RecordatorioCita == true ? "Activados" : "Desactivados")}\n\n¡Te esperamos!");
                await MostrarMenuPrincipal(chatId);
            }
            else
            {
                await SendMessage(chatId, $"Error al agendar la cita: {resultado}");
            }

            enAgendarCita = false;
            citaEnProceso = null;
        }
        private async Task MostrarMenuOrtodoncistas(long chatId)
        {
            var tecladoOrtodoncistas = new ReplyKeyboardMarkup(new[]
            {
        new KeyboardButton[] { "Ortodoncista de Preferencia", "Cualquier Ortodoncista" }
            })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona una opción para asignar un ortodoncista:", tecladoOrtodoncistas);
        }

        private async Task MostrarMenuFechas(long chatId)
        {
            var tecladoFechas = new ReplyKeyboardMarkup(new[]
            {
        new KeyboardButton[] { DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"), DateTime.Now.AddDays(2).ToString("dd/MM/yyyy") },
        new KeyboardButton[] { DateTime.Now.AddDays(3).ToString("dd/MM/yyyy"), "Otra fecha" }
            })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona una fecha para la cita:", tecladoFechas);
        }

        private async Task MostrarMenuHoras(long chatId)
        {
            var tecladoHoras = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "08:00", "09:00", "10:00" },
                    new KeyboardButton[] { "11:00", "14:00", "15:00" },
                    new KeyboardButton[] { "16:00", "17:00" }
                })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona una hora para la cita:", tecladoHoras);
        }

        private async Task MostrarMenuMotivos(long chatId)
        {
            var tecladoMotivos = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "Limpieza dental", "Ortodoncia", "Consulta general" },
                    new KeyboardButton[] { "Extracción de muela", "Blanqueamiento", "Otros" }
                })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona el motivo de tu cita:", tecladoMotivos);
        }

        private async Task VerDetallesCita(long chatId)
        {
            if (!usuarioAutenticado || string.IsNullOrEmpty(cedulaPacienteIniciada))
            {
                Console.WriteLine($"Usuario autenticado: {usuarioAutenticado}, Cédula: {cedulaPacienteIniciada}");
                await SendMessage(chatId, "Debes iniciar sesión o registrarte para ver los detalles de tus citas.");
                await MostrarMenuBienvenida(chatId);
                return;
            }

            Console.WriteLine($"Buscando citas para Cédula del paciente: {cedulaPacienteIniciada}, Estado: Solicitada");

            List<Cita> citasUsuario = _servicioCita.cargarPorFiltros("Solicitada", "Pendiente", cedulaPacienteIniciada);

            if (citasUsuario == null || citasUsuario.Count == 0)
            {
                Console.WriteLine("No se encontraron citas solicitadas o pendientes para el paciente.");
                await SendMessage(chatId, "No tienes citas agendadas.");
                return;
            }

            Console.WriteLine($"Citas encontradas: {citasUsuario.Count}");
            foreach (var cita in citasUsuario)
            {
                Console.WriteLine($"Cita: {cita.Codigo}, Fecha: {cita.Fecha_Cita}, Estado: {cita.Estado}");

                string nombreOrtodoncista = "No asignado";

                if (!string.IsNullOrEmpty(cita.CodigoOrtodoncista))
                {
                    var ortodoncista = _servicioOrtodoncista.obtenerPorCodigo(cita.CodigoOrtodoncista);
                    if (ortodoncista != null)
                    {
                        nombreOrtodoncista = $"{ortodoncista.PrimerNombre} {ortodoncista.PrimerApellido}";
                    }
                }

                string detallesCita = $"📅 Fecha: {cita.Fecha_Cita:dd/MM/yyyy}\n" +
                                      $"🕒 Hora: {cita.Hora_Cita:hh\\:mm}\n" +
                                      $"💼 Motivo: {cita.Razon_Cita}\n" +
                                      $"📍 Estado: {cita.Estado}\n" +
                                      $"👨‍⚕️ Ortodoncista: {nombreOrtodoncista}\n" +
                                      $"🔔 Recordatorio: {(cita.RecordatorioCita.HasValue && cita.RecordatorioCita.Value ? "Activado" : "Desactivado")}\n" +
                                      $"-------------------------------------------";

                await SendMessage(chatId, detallesCita);
            }
        }

        private async Task MostrarMenuPrincipal(long chatId)
        {
            var menuPrincipal = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "Agendar Cita", "Detalles de Cita" },
                    new KeyboardButton[] { "Modificar o Cancelar Citas", "Confirmar Recordatorios" },
                    new KeyboardButton[] { "Consultar Diagnóstico Reciente", "Gestionar Pagos" },
                    new KeyboardButton[] { "Emergencia de Ortodoncia", "Preguntas Frecuentes", "Soporte Técnico" },
                    new KeyboardButton[] { "Volver al Menu de Registro/Inicio Sesion" }
                })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Por favor, selecciona una opción del menú:", menuPrincipal);
        }

        private async Task ProcesarComandoPrincipal(long chatId, string mensaje)
        {
            mensaje = mensaje.Trim();

            if (esperandoSeleccionRecordatorios)
            {
                await ProcesarCantidadRecordatorios(chatId, mensaje);
                return;
            }

            switch (mensaje.ToLowerInvariant())
            {
                case "agendar cita":
                    esperandoSeleccionRecordatorios = false;
                    await AgendarCita(chatId);
                    break;

                case "detalles de cita":
                    esperandoSeleccionRecordatorios = false;
                    await VerDetallesCita(chatId);
                    break;

                case "confirmar recordatorios":
                    esperandoSeleccionRecordatorios = true;
                    await EnviarRecordatorios(chatId, mensaje);
                    break;

                case "consultar diagnóstico reciente":
                    esperandoSeleccionRecordatorios = false;
                    await ConsultarDiagnosticoReciente(chatId);
                    break;

                case "gestionar pagos":
                    esperandoSeleccionRecordatorios = false;
                    await GestionarPagos(chatId);
                    break;

                case "preguntas frecuentes":
                    esperandoSeleccionRecordatorios = false;
                    await PreguntasFrecuentes(chatId);
                    break;

                case "soporte técnico":
                    esperandoSeleccionRecordatorios = false;
                    await SoporteTecnico(chatId);
                    break;

                case "volver al menu de registro/inicio sesion":
                    esperandoSeleccionRecordatorios = false;
                    await MostrarMenuBienvenida(chatId);
                    break;

                default:
                    esperandoSeleccionRecordatorios = false;
                    await ProcesarGestionPagos(chatId, mensaje);
                    break;
            }
        }

        private async Task GestionarCitas(long chatId, string mensaje = null)
        {
            if (string.IsNullOrEmpty(cedulaPacienteIniciada))
            {
                await SendMessage(chatId, "Debes iniciar sesión para poder gestionar tus citas.");
                await MostrarMenuBienvenida(chatId);
                return;
            }

            if (mensaje == null || mensaje == "Modificar o Cancelar Citas")
            {
                string cedulaPaciente = cedulaPacienteIniciada;
                var citas = _servicioCita.cargarPorFiltros("Solicitada", "Pendiente", cedulaPaciente);

                if (citas.Count == 0)
                {
                    await SendMessage(chatId, "No tienes citas solicitadas o pendientes para modificar o cancelar.");
                    await MostrarMenuPrincipal(chatId);
                    enGestionCita = false;
                    return;
                }

                var botonesCitas = citas.Select(cita => new KeyboardButton($"{cita.Codigo} - {cita.Fecha_Cita:dd/MM/yyyy} - {cita.Hora_Cita}")).ToArray();
                var botonesCitasAgrupados = botonesCitas
                    .Select((boton, index) => new { boton, index })
                    .GroupBy(x => x.index / 2)
                    .Select(g => g.Select(x => x.boton).ToArray())
                    .ToList();

                botonesCitasAgrupados.Add(new[] { new KeyboardButton("Volver al Menú Principal") });

                var tecladoCitas = new ReplyKeyboardMarkup(botonesCitasAgrupados)
                {
                    ResizeKeyboard = true,
                    OneTimeKeyboard = true
                };

                await SendMessage(chatId, "Selecciona el código de la cita que deseas modificar o cancelar:", tecladoCitas);
                enGestionCita = true;
            }
            else if (enGestionCita)
            {
                if (mensaje == "Volver al Menú Principal")
                {
                    enGestionCita = false;
                    enModificarCita = false;
                    enEsperaNuevoMotivo = false;
                    enEsperaNuevaFecha = false;
                    enEsperaNuevaHora = false;
                    citaSeleccionada = null;
                    await MostrarMenuPrincipal(chatId);
                }
                else
                {
                    string cedulaPaciente = cedulaPacienteIniciada;

                    string[] partesMensaje = mensaje.Split('-');
                    if (partesMensaje.Length > 0)
                    {
                        string codigoCita = partesMensaje[0].Trim();

                        var citas = _servicioCita.cargarPorFiltros("Solicitada", "Pendiente", cedulaPaciente);
                        var citaSeleccionadaObj = citas.FirstOrDefault(c => c.Codigo == codigoCita);

                        if (citaSeleccionadaObj != null)
                        {
                            citaSeleccionada = citaSeleccionadaObj.Codigo;
                            await MostrarOpcionesCita(chatId, citaSeleccionadaObj);
                            enModificarCita = true;
                            enGestionCita = false;
                        }
                        else
                        {
                            await SendMessage(chatId, "Cita no encontrada. Por favor, selecciona una opción válida.");
                        }
                    }
                    else
                    {
                        await SendMessage(chatId, "Formato inválido. Por favor, selecciona una opción válida.");
                    }
                }
            }
            else if (enModificarCita && citaSeleccionada != null)
            {
                if (mensaje == "Modificar Razón")
                {
                    await SendMessage(chatId, "Por favor, ingresa el nuevo motivo para la cita:");
                    enEsperaNuevoMotivo = true;
                    await MostrarMenuMotivos(chatId);
                }
                else if (mensaje == "Modificar Fecha")
                {
                    await SendMessage(chatId, "Por favor, ingresa la nueva fecha para la cita en formato DD/MM/YYYY:");
                    enEsperaNuevaFecha = true;
                    await MostrarMenuFechas(chatId);
                }
                else if (mensaje == "Modificar Hora")
                {
                    await SendMessage(chatId, "Por favor, ingresa la nueva hora para la cita en formato HH:MM (24 horas):");
                    enEsperaNuevaHora = true;
                    await MostrarMenuHoras(chatId);
                }
                else if (mensaje == "Cancelar")
                {
                    var cita = _servicioCita.obtenerPorCodigo(citaSeleccionada);
                    if (cita != null)
                    {
                        _servicioCita.eliminar(cita);
                        await SendMessage(chatId, "Tu cita ha sido cancelada con éxito.");
                    }
                    else
                    {
                        await SendMessage(chatId, "La cita no pudo ser encontrada.");
                    }

                    enModificarCita = false;
                    citaSeleccionada = null;
                    await MostrarMenuPrincipal(chatId);
                }
                else if (enEsperaNuevoMotivo)
                {
                    var cita = _servicioCita.obtenerPorCodigo(citaSeleccionada);
                    if (cita != null)
                    {
                        cita.Razon_Cita = mensaje;
                        cita.Estado = "Solicitada";
                        string resultado = _servicioCita.actualizarRazon(cita, mensaje);

                        if (resultado == "Registro modificado")
                        {
                            await SendMessage(chatId, "La razón de la cita ha sido modificada con éxito, y su estado ha cambiado a 'Solicitada'.");
                        }
                        else
                        {
                            await SendMessage(chatId, $"Error al modificar la cita: {resultado}");
                        }
                    }
                    else
                    {
                        await SendMessage(chatId, "La cita no pudo ser encontrada.");
                    }

                    enModificarCita = false;
                    enEsperaNuevoMotivo = false;
                    citaSeleccionada = null;
                    await MostrarMenuPrincipal(chatId);
                }
                else if (enEsperaNuevaFecha)
                {
                    if (DateTime.TryParseExact(mensaje, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime nuevaFecha))
                    {
                        var cita = _servicioCita.obtenerPorCodigo(citaSeleccionada);
                        if (cita != null)
                        {
                            cita.Fecha_Cita = nuevaFecha;
                            cita.Estado = "Solicitada";
                            string resultado = _servicioCita.actualizarFecha(cita);

                            if (resultado == "Registro modificado")
                            {
                                await SendMessage(chatId, "La fecha de la cita ha sido modificada con éxito, y su estado ha cambiado a 'Solicitada'.");
                            }
                            else
                            {
                                await SendMessage(chatId, $"Error al modificar la fecha de la cita: {resultado}");
                            }
                        }
                        else
                        {
                            await SendMessage(chatId, "La cita no pudo ser encontrada.");
                        }

                        enModificarCita = false;
                        enEsperaNuevaFecha = false;
                        citaSeleccionada = null;
                        await MostrarMenuPrincipal(chatId);
                    }
                    else
                    {
                        await SendMessage(chatId, "Formato de fecha inválido. Por favor, ingresa la fecha en formato DD-MM-YYYY.");
                    }
                }
                else if (enEsperaNuevaHora)
                {
                    if (TimeSpan.TryParse(mensaje, out TimeSpan nuevaHora))
                    {
                        var cita = _servicioCita.obtenerPorCodigo(citaSeleccionada);
                        if (cita != null)
                        {
                            cita.Hora_Cita = nuevaHora;
                            cita.Estado = "Solicitada";
                            string resultado = _servicioCita.actualizarHora(cita.Codigo, nuevaHora);

                            if (resultado == "Registro modificado")
                            {
                                await SendMessage(chatId, "La hora de la cita ha sido modificada con éxito, y su estado ha cambiado a 'Solicitada'.");
                            }
                            else
                            {
                                await SendMessage(chatId, $"Error al modificar la hora de la cita: {resultado}");
                            }
                        }
                        else
                        {
                            await SendMessage(chatId, "La cita no pudo ser encontrada.");
                        }

                        enModificarCita = false;
                        enEsperaNuevaHora = false;
                        citaSeleccionada = null;
                        await MostrarMenuPrincipal(chatId);
                    }
                    else
                    {
                        await SendMessage(chatId, "Formato de hora inválido. Por favor, ingresa la hora en formato HH:MM (24 horas).");
                    }
                }
                else
                {
                    await SendMessage(chatId, "Por favor, selecciona una opción válida.");
                }
            }
            else
            {
                await SendMessage(chatId, "Por favor, selecciona una opción válida del menú.");
            }
        }

        private async Task MostrarOpcionesCita(long chatId, Cita cita)
        {
            var opciones = new ReplyKeyboardMarkup(new[]
            {
                    new[] { new KeyboardButton("Modificar Razón"), new KeyboardButton("Modificar Fecha") },
                    new[] { new KeyboardButton("Modificar Hora"), new KeyboardButton("Cancelar") },
                    new[] { new KeyboardButton("Volver al Menú Principal") }
                })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };

            await SendMessage(chatId, $"¿Qué acción deseas realizar para la cita con código {cita.Codigo}?", opciones);
        }

        public static class EstadosFactura
        {
            public const string Pendiente = "Pendiente";
            public const string Pagado = "Pagado";
        }

        private async Task GestionarPagos(long chatId)
        {
            if (string.IsNullOrEmpty(cedulaPacienteIniciada))
            {
                await SendMessage(chatId, "Debes iniciar sesión para poder gestionar tus pagos.");
                await MostrarMenuBienvenida(chatId);
                return;
            }

            await SendMessage(chatId, "Gestiona tus pagos pendientes aquí. Selecciona una opción:", new ReplyKeyboardMarkup(new[]
            {
            new KeyboardButton[] { "Consultar Facturas Pendientes", "Realizar Pago" },
            new KeyboardButton[] { "Ver Historial de Facturas Pagadas", "Volver al Menú Principal" }
        })
            {
                ResizeKeyboard = true
            });
        }

        private async Task ProcesarGestionPagos(long chatId, string mensaje)
        {
            if (esperandoIdFactura)
            {
                await ProcesarIdFacturaSeleccionada(chatId, mensaje);
                return;
            }

            if (esperandoProcesoPago)
            {
                esperandoProcesoPago = false;
                await ProcesarMontoSeleccionado(chatId, mensaje);
                return;
            }

            if (esperandoMetodoPago)
            {
                await ProcesarMetodoPago(chatId, mensaje);
                return;
            }

            switch (mensaje.ToLowerInvariant())
            {
                case "consultar facturas pendientes":
                    await ConsultarFacturasPendientes(chatId);
                    break;

                case "realizar pago":
                    await MostrarMenuFacturasPendientes(chatId);
                    break;

                case "ver historial de facturas pagadas":
                    await VerHistorialFacturasPagadas(chatId);
                    break;

                case "volver al menú principal":
                    await MostrarMenuPrincipal(chatId);
                    break;

                default:
                    await SendMessage(chatId, "Por favor, selecciona una opción válida del menú.");
                    break;
            }
        }
        private async Task MostrarMenuFacturasPendientes(long chatId)
        {
            var facturasPendientes = _servicioFactura.cargarPorCedula(cedulaPacienteIniciada)
                .Where(f => f.Estado == EstadosFactura.Pendiente)
                .ToList();

            if (!facturasPendientes.Any())
            {
                await SendMessage(chatId, "No tienes facturas pendientes en este momento.");
                return;
            }

            var botones = facturasPendientes.Select(f => new KeyboardButton[] { new KeyboardButton(f.ID_Factura) }).ToArray();
            var tecladoFacturas = new ReplyKeyboardMarkup(botones)
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona el ID de la factura que deseas pagar:", tecladoFacturas);
            esperandoIdFactura = true;
        }

        private async Task ProcesarIdFacturaSeleccionada(long chatId, string idFactura)
        {
            var factura = _servicioFactura.obtenerPorCodigo(idFactura);

            if (factura == null || factura.Estado != EstadosFactura.Pendiente)
            {
                await SendMessage(chatId, "El ID de factura ingresado no es válido o la factura ya ha sido pagada.");
                return;
            }

            facturaEnProceso = factura;
            esperandoIdFactura = false;

            string detalleFactura = $"🧾 Factura seleccionada: {facturaEnProceso.ID_Factura}\n" +
                                    $"💵 Monto total: {facturaEnProceso.Total.ToString("F0")}\n" +
                                    $"💰 Saldo pendiente: {(facturaEnProceso.Total - facturaEnProceso.Total_Pagado).ToString("F0")} \n" +
                                    $"-------------------------------------------";

            await SendMessage(chatId, detalleFactura);


            await MostrarMontosPago(chatId);
        }

        private async Task MostrarMontosPago(long chatId)
        {
            if (facturaEnProceso == null || Math.Round(facturaEnProceso.Total, 2) <= Math.Round(facturaEnProceso.Total_Pagado, 2))
            {
                await SendMessage(chatId, "No se encontró una factura válida con saldo pendiente. Por favor, reinicia el proceso.");
                ResetearEstadoPago();
                return;
            }

            double saldoPendiente = Math.Round(facturaEnProceso.Total - facturaEnProceso.Total_Pagado, 2);

            var botones = new List<List<KeyboardButton>>();
            double incremento = Math.Ceiling(saldoPendiente / 4);
            double montoActual = incremento;

            while (montoActual < saldoPendiente)
            {
                botones.Add(new List<KeyboardButton> { new KeyboardButton($"{Math.Round(montoActual, 2):F0}") });
                montoActual += incremento;
            }


            botones.Add(new List<KeyboardButton> { new KeyboardButton($"Pagar Total: {Math.Round(saldoPendiente, 2):F0}") });


            botones.Add(new List<KeyboardButton> { new KeyboardButton("Volver al Menú Principal") });

            var tecladoMontos = new ReplyKeyboardMarkup(botones.ToArray())
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona el monto que deseas pagar:", tecladoMontos);
            esperandoProcesoPago = true;
        }

        private async Task ProcesarMontoSeleccionado(long chatId, string montoSeleccionado)
        {
            if (facturaEnProceso == null)
            {
                await SendMessage(chatId, "No se encontró ninguna factura en proceso. Por favor, reinicia el proceso de pago.");
                ResetearEstadoPago();
                return;
            }

            if (montoSeleccionado.ToLower() == "volver al menú principal")
            {
                await MostrarMenuPrincipal(chatId);
                ResetearEstadoPago();
                return;
            }

            montoSeleccionado = montoSeleccionado.Replace("Pagar Total: ", "").Replace("$", "").Replace(",", "").Trim();

            if (!double.TryParse(montoSeleccionado, out double monto) || monto <= 0)
            {
                await SendMessage(chatId, "El monto seleccionado no es válido. Por favor, selecciona una opción nuevamente.");
                return;
            }

            montoEnProceso = monto.ToString();
            await MostrarMetodosPago(chatId);

        }

        private async Task MostrarMetodosPago(long chatId)
        {
            var metodosPago = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "Efectivo", "PSE" },
                    new KeyboardButton[] { "Tarjeta de crédito", "Tarjeta de débito" },
                    new KeyboardButton[] { "Volver al Menú Principal" }
                })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, "Selecciona el método de pago:", metodosPago);
            esperandoMetodoPago = true;
        }

        private void ResetearEstadoPago()
        {
            esperandoMetodoPago = false;
            esperandoDatosTarjeta = false;
            esperandoFechaExpiracion = false;
            esperandoCVV = false;
            esperandoSeleccionCuotas = false;
            tipoTarjetaEnProceso = null;
            numeroTarjetaEnProceso = null;
            fechaExpiracionEnProceso = null;
            cvvEnProceso = null;
            cuotasEnProceso = 0;
            montoEnProceso = null;
        }

        private async Task ProcesarMetodoPago(long chatId, string metodoPago)
        {
            if (!esperandoMetodoPago)
            {
                await SendMessage(chatId, "Por favor, selecciona un método de pago.");
                return;
            }

            esperandoMetodoPago = false;

            if (facturaEnProceso == null)
            {
                await SendMessage(chatId, "Hubo un problema con la factura seleccionada. Por favor, inténtalo nuevamente.");
                ResetearEstadoPago();
                return;
            }

            switch (metodoPago.ToLowerInvariant())
            {
                case "efectivo":
                    await RegistrarPagoEfectivo(chatId);
                    break;

                case "pse":
                    await RegistrarPagoPSE(chatId);
                    break;

                case "tarjeta de crédito":
                    tipoTarjetaEnProceso = "crédito";
                    esperandoDatosTarjeta = true;
                    await SolicitarDatosTarjeta(chatId);
                    break;

                case "tarjeta de débito":
                    tipoTarjetaEnProceso = "débito";
                    esperandoDatosTarjeta = true;
                    await SolicitarDatosTarjeta(chatId);
                    break;

                case "volver al menú principal":
                    await MostrarMenuPrincipal(chatId);
                    ResetearEstadoPago();
                    break;

                default:
                    await SendMessage(chatId, "Método de pago inválido. Por favor, selecciona una opción válida.");
                    esperandoMetodoPago = true;
                    break;
            }
        }

        private async Task SolicitarDatosTarjeta(long chatId)
        {
            await SendMessage(chatId, $"Por favor, ingresa el número de la tarjeta de {tipoTarjetaEnProceso} (16 dígitos).");
        }

        //Este es un número de tarjeta ejemplo valido: 5288928163769320
        private async Task ProcesarDatosTarjeta(long chatId, string mensaje)
        {
            if (esperandoDatosTarjeta)
            {
                if (!_validaciones.EsNumeroTarjetaValido(mensaje))
                {
                    await SendMessage(chatId, "El número de tarjeta no es válido. Por favor, ingresa un número de tarjeta de 16 dígitos.");
                    return;
                }

                numeroTarjetaEnProceso = mensaje;
                esperandoDatosTarjeta = false;
                esperandoFechaExpiracion = true;
                await SendMessage(chatId, "Ahora, ingresa la fecha de expiración de la tarjeta en formato MM/AA.");
                return;
            }

            if (esperandoFechaExpiracion)
            {
                if (!_validaciones.EsFechaExpiracionValida(mensaje))
                {
                    await SendMessage(chatId, "La fecha de expiración no es válida. Por favor, ingresa una fecha en formato MM/AA.");
                    return;
                }

                fechaExpiracionEnProceso = mensaje;
                esperandoFechaExpiracion = false;
                esperandoCVV = true;
                await SendMessage(chatId, "Por último, ingresa el código CVV de 3 dígitos.");
                return;
            }

            if (esperandoCVV)
            {
                if (!_validaciones.EsCVVValido(mensaje))
                {
                    await SendMessage(chatId, "El código CVV no es válido. Por favor, ingresa un código de 3 dígitos.");
                    return;
                }

                cvvEnProceso = mensaje;
                esperandoCVV = false;

                if (tipoTarjetaEnProceso == "crédito")
                {
                    await MostrarMenuCuotas(chatId);
                }
                else
                {
                    await RegistrarPagoTarjeta(chatId);
                }
                return;
            }

            await SendMessage(chatId, "No se esperaba esta información. Por favor, sigue las instrucciones.");
        }

        private async Task MostrarMenuCuotas(long chatId)
        {
            var tecladoCuotas = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "1", "3", "6" },
                    new KeyboardButton[] { "12", "24", "36" },
                    new KeyboardButton[] { "Cancelar" }
                })
            {
                ResizeKeyboard = true
            };

            esperandoSeleccionCuotas = true;
            await SendMessage(chatId, "Selecciona el número de cuotas para tu pago con tarjeta de crédito:", tecladoCuotas);
        }

        private async Task ProcesarSeleccionCuotas(long chatId, string mensaje)
        {
            if (!esperandoSeleccionCuotas)
            {
                await SendMessage(chatId, "Por favor, selecciona el número de cuotas.");
                return;
            }

            if (mensaje.Equals("Cancelar", StringComparison.OrdinalIgnoreCase))
            {
                await SendMessage(chatId, "El proceso de pago con tarjeta ha sido cancelado.");
                ResetearEstadoPago();
                return;
            }

            if (!int.TryParse(mensaje, out int cuotas) || cuotas <= 0)
            {
                await SendMessage(chatId, "Número de cuotas no válido. Por favor, selecciona una opción válida.");
                return;
            }

            cuotasEnProceso = cuotas;
            esperandoSeleccionCuotas = false;
            await RegistrarPagoTarjeta(chatId);
        }

        private async Task RegistrarPagoTarjeta(long chatId)
        {
            if (!_validaciones.ValidarNumeroTarjeta(numeroTarjetaEnProceso))
            {
                await SendMessage(chatId, "El número de tarjeta no pasó la validación. Por favor, verifica los datos ingresados.");
                ResetearEstadoPago();
                return;
            }

            await SendMessage(chatId, "🔄 Procesando el pago con tarjeta...");
            await Task.Delay(2000);

            if (!double.TryParse(montoEnProceso, out double monto) || monto <= 0)
            {
                await SendMessage(chatId, "El monto almacenado no es válido. Por favor, selecciona un monto nuevamente.");
                ResetearEstadoPago();
                return;
            }

            facturaEnProceso = _servicioFactura.sumarMontoAPagado(facturaEnProceso, monto);

            bool facturaCompletamentePagada = Math.Round(facturaEnProceso.Total_Pagado, 2) >= Math.Round(facturaEnProceso.Total, 2);

            if (facturaCompletamentePagada)
            {
                _servicioFactura.actualizarEstado(facturaEnProceso, EstadosFactura.Pagado);
                await SendMessage(chatId, $"✅ Pago registrado exitosamente.\n\nFactura {facturaEnProceso.ID_Factura} ahora está completamente pagada.");

            }
            else
            {
                double saldoRestante = Math.Round(facturaEnProceso.Total - facturaEnProceso.Total_Pagado, 2);
                await SendMessage(chatId, $"✅ Pago parcial registrado.\n\nFactura: {facturaEnProceso.ID_Factura}\n💰 *Saldo restante:* {saldoRestante:C}");
            }

            try
            {
                Pago nuevoPago = new Pago
                {
                    ID_Pago = _servicioPago.generarCodigo(),
                    Fecha_Pago = DateTime.Now,
                    Metodo_Pago = tipoTarjetaEnProceso == "crédito" ? "Tarjeta de Crédito" : "Tarjeta de Débito",
                    Monto = monto,
                    CodigoFactura = facturaEnProceso.ID_Factura,
                    CodigoPaciente = facturaEnProceso.CedulaPaciente
                };

                _servicioPago.guardar(nuevoPago);

                await SendMessage(chatId, $"✅ Pago registrado exitosamente con tarjeta de {tipoTarjetaEnProceso}. " +
                                          $"Número de cuotas: {cuotasEnProceso}.\n" +
                                          $"💰 Saldo restante: {Math.Round(facturaEnProceso.Total - facturaEnProceso.Total_Pagado, 2):C}");
            }
            catch (Exception ex)
            {
                await SendMessage(chatId, $"⚠️ Ocurrió un error al registrar el pago: {ex.Message}");
            }
            finally
            {
                ResetearEstadoPago();
            }
            await ProcesarComandoPrincipal(chatId, "volver al menu principal");
        }

        private async Task RegistrarPagoEfectivo(long chatId)
        {
            if (!double.TryParse(montoEnProceso, out double monto) || monto <= 0)
            {
                await SendMessage(chatId, "El monto almacenado no es válido. Por favor, selecciona un monto nuevamente.");
                ResetearEstadoPago();
                return;
            }

            _servicioFactura.sumarMontoAPagado(facturaEnProceso, monto);

            Factura facturaActualizada = _servicioFactura.obtenerPorCodigo(facturaEnProceso.ID_Factura);
            bool facturaCompletamentePagada = Math.Round(facturaActualizada.Total_Pagado, 2) >= Math.Round(facturaActualizada.Total, 2);

            if (facturaCompletamentePagada)
            {
                _servicioFactura.actualizarEstado(facturaActualizada, EstadosFactura.Pagado);
                await SendMessage(chatId, $"✅ Pago en efectivo registrado exitosamente.\n\nFactura {facturaActualizada.ID_Factura} ahora está completamente pagada.");
            }
            else
            {
                double saldoRestante = Math.Round(facturaActualizada.Total - facturaActualizada.Total_Pagado, 2);
                await SendMessage(chatId, $"✅ Pago parcial registrado en efectivo.\n\nFactura: {facturaActualizada.ID_Factura}\n💰 *Saldo restante:* {saldoRestante:C}");
            }

            string resultadoGuardarPago = _servicioPago.guardar(new Pago
            {
                ID_Pago = _servicioPago.generarCodigo(),
                Fecha_Pago = DateTime.Now,
                Metodo_Pago = "Efectivo",
                Monto = monto,
                CodigoFactura = facturaEnProceso.ID_Factura,
                CodigoPaciente = facturaEnProceso.CedulaPaciente
            });

            if (!resultadoGuardarPago.StartsWith("Error"))
            {
                await SendMessage(chatId, "✅ Pago en efectivo registrado exitosamente.");
            }
            else
            {
                await SendMessage(chatId, $"⚠️ Ocurrió un error al registrar el pago: {resultadoGuardarPago}");
            }

            ResetearEstadoPago();
            await ProcesarComandoPrincipal(chatId, "volver al menu principal");
        }

        private async Task RegistrarPagoPSE(long chatId)
        {
            if (!double.TryParse(montoEnProceso, out double monto) || monto <= 0)
            {
                await SendMessage(chatId, "El monto almacenado no es válido. Por favor, selecciona un monto nuevamente.");
                ResetearEstadoPago();
                return;
            }

            await SendMessage(chatId, "🔄 Procesando el pago a través de PSE...");
            await Task.Delay(2000);
            await SendMessage(chatId, "✅ Pago a través de PSE completado exitosamente.");

            _servicioFactura.sumarMontoAPagado(facturaEnProceso, monto);

            Factura facturaActualizada = _servicioFactura.obtenerPorCodigo(facturaEnProceso.ID_Factura);
            bool facturaCompletamentePagada = Math.Round(facturaActualizada.Total_Pagado, 2) >= Math.Round(facturaActualizada.Total, 2);

            if (facturaCompletamentePagada)
            {
                _servicioFactura.actualizarEstado(facturaActualizada, EstadosFactura.Pagado);
                await SendMessage(chatId, $"✅ Pago a través de PSE registrado exitosamente.\n\nFactura {facturaActualizada.ID_Factura} ahora está completamente pagada.");

            }
            else
            {
                double saldoRestante = Math.Round(facturaActualizada.Total - facturaActualizada.Total_Pagado, 2);
                await SendMessage(chatId, $"✅ Pago parcial registrado a través de PSE.\n\nFactura: {facturaActualizada.ID_Factura}\n💰 *Saldo restante:* {saldoRestante:C}");
            }

            string resultadoGuardarPago = _servicioPago.guardar(new Pago
            {
                ID_Pago = _servicioPago.generarCodigo(),
                Fecha_Pago = DateTime.Now,
                Metodo_Pago = "PSE",
                Monto = monto,
                CodigoFactura = facturaEnProceso.ID_Factura,
                CodigoPaciente = facturaEnProceso.CedulaPaciente
            });

            if (!resultadoGuardarPago.StartsWith("Error"))
            {
                await SendMessage(chatId, "✅ Pago a través de PSE registrado exitosamente.");
            }
            else
            {
                await SendMessage(chatId, $"⚠️ Ocurrió un error al registrar el pago: {resultadoGuardarPago}");
            }

            ResetearEstadoPago();
            await ProcesarComandoPrincipal(chatId, "volver al menu principal");
        }
        private async Task ConsultarFacturasPendientes(long chatId)
        {
            var facturasPendientes = _servicioFactura.cargarPorCedula(cedulaPacienteIniciada)
                .Where(f => f.Estado == EstadosFactura.Pendiente)
                .ToList();

            if (!facturasPendientes.Any())
            {
                await SendMessage(chatId, "No tienes facturas pendientes en este momento.");
                return;
            }

            foreach (var factura in facturasPendientes)
            {
                int diasDeAtraso = (DateTime.Now.Date - factura.Fecha_Emision.Date).Days;

                if (diasDeAtraso < 0) diasDeAtraso = 0;

                const double tasaInteresDiaria = 0.01;
                double interesesPorDemora = diasDeAtraso * tasaInteresDiaria * (factura.Total - factura.Total_Pagado);
                if (interesesPorDemora < 0) interesesPorDemora = 0;

                double valoractualFactura = factura.Total + interesesPorDemora;

                factura.Total = valoractualFactura;
                string resultadoActualizacion = _servicioFactura.actualizarEstado(factura, factura.Estado);

                string detalleFactura = $"🧾 ID Factura: {factura.ID_Factura}\n" +
                                        $"💵 Cobro Original: {factura.Total - interesesPorDemora:C}\n" +
                                        $"💵 Total Actualizado: {valoractualFactura:C}\n" +
                                        $"💰 Total Pagado: {factura.Total_Pagado:C}\n" +
                                        $"📅 Fecha de Emisión: {factura.Fecha_Emision:dd/MM/yyyy}\n" +
                                        $"📍 Estado: {factura.Estado}\n" +
                                        $"⏳ Días de atraso: {diasDeAtraso}\n" +
                                        $"💸 Intereses por demora: {interesesPorDemora:C}\n" +
                                        $"-------------------------------------------";

                await SendMessage(chatId, detalleFactura);

                if (resultadoActualizacion != "Factura actualizada correctamente.")
                {
                    Console.WriteLine($"Error al actualizar la factura {factura.ID_Factura}: {resultadoActualizacion}");
                }
            }
        }

        private async Task VerHistorialFacturasPagadas(long chatId)
        {
            var historialFacturas = _servicioFactura.cargarPorCedula(cedulaPacienteIniciada)
                .Where(f => f.Estado.Equals(EstadosFactura.Pagado, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!historialFacturas.Any())
            {
                await SendMessage(chatId, "No tienes historial de facturas pagadas.");
                return;
            }

            foreach (var factura in historialFacturas)
            {
                string detalleFactura = $"🧾 ID Factura: {factura.ID_Factura}\n" +
                                        $"💵 Total: {factura.Total:C}\n" +
                                        $"💰 Total Pagado: {factura.Total_Pagado:C}\n" +
                                        $"📅 Fecha Emisión: {factura.Fecha_Emision:dd/MM/yyyy}\n" +
                                        $"📍 Estado: {factura.Estado}\n" +
                                        $"💵 Cambio: {factura.Cambio:C}\n" +
                                        $"-------------------------------------------";
                await SendMessage(chatId, detalleFactura);
            }
        }

        private async Task ConsultarDiagnosticoReciente(long chatId)
        {
            if (!usuarioAutenticado || string.IsNullOrEmpty(cedulaPacienteIniciada))
            {
                await SendMessage(chatId, "Debes iniciar sesión para consultar tu diagnóstico.");
                return;
            }

            try
            {

                var diagnosticos = _servicioDiagnostico.cargarPorCedula(cedulaPacienteIniciada)
                                                       .OrderByDescending(d => d.Fecha_Diagnostico)
                                                       .ToList();

                if (diagnosticos == null || diagnosticos.Count == 0)
                {
                    await SendMessage(chatId, "No se encontraron diagnósticos recientes.");
                    return;
                }

                var diagnosticoReciente = diagnosticos.FirstOrDefault();

                if (diagnosticoReciente == null || string.IsNullOrEmpty(diagnosticoReciente.Codigo))
                {
                    await SendMessage(chatId, "No se encontró información del diagnóstico más reciente.");
                    return;
                }


                var diagnostico = _servicioDiagnostico.obtenerPorCodigo(diagnosticoReciente.Codigo);

                if (diagnostico == null)
                {
                    await SendMessage(chatId, "No se pudo recuperar información adicional del diagnóstico.");
                    return;
                }


                string mensaje = $"📄 Diagnóstico más reciente:\n" +
                                 $"🗓 Fecha: {diagnostico.Fecha_Diagnostico:dd-MM-yyyy}\n" +
                                 $"🔍 Descripción: {diagnostico.Descripcion}\n" +
                                 $"📋 ID de la Cita: {diagnostico.CodigoCita}\n" +
                                 $"👨‍⚕️ Paciente: {diagnostico.CedulaPaciente}\n" +
                                 $"-------------------------------------------";

                await SendMessage(chatId, mensaje);
            }
            catch (Exception ex)
            {
                await SendMessage(chatId, $"Ocurrió un error al consultar el diagnóstico: {ex.Message}");
            }
        }

        private async Task ManejarEmergenciaDeOrtodoncia(long chatId)
        {
            _esperandoEmergencia = true;
            await SendMessage(chatId, "Has reportado una emergencia de ortodoncia. Por favor, describe brevemente el problema para ofrecerte una orientación inicial.");
        }
        private async Task ProcesarEmergenciaDeOrtodoncia(long chatId, string descripcion)
        {
            if (!_esperandoEmergencia)
                return;

            _esperandoEmergencia = false;

            string respuestaIA;
            try
            {
                respuestaIA = await _servicioIA.GenerarRespuesta(
                    $"Un paciente reportó una emergencia de ortodoncia con la siguiente descripción: {descripcion}. " +
                    "Proporciona una guía breve sobre qué puede hacer de inmediato y aclara que debe agendar una cita con un ortodoncista para atención profesional."
                );
            }
            catch (Exception ex)
            {
                respuestaIA = $"Lo siento, ocurrió un error al procesar tu solicitud. Por favor intenta nuevamente o contacta a un ortodoncista lo antes posible. Error: {ex.Message}";
            }

            await SendMessage(chatId, respuestaIA);

            await SendMessage(chatId, "Recuerda que esta es solo una orientación inicial. Es fundamental que programes una cita con un ortodoncista para atención profesional.");

            await MostrarMenuPrincipal(chatId);
        }

        bool esperandoRespuestaPreguntas;
        private async Task PreguntasFrecuentes(long chatId)
        {

            var tecladoPreguntas = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { "1. ¿Cómo agendo una cita?" },
                new KeyboardButton[] { "2. ¿Cómo configuro recordatorios?" },
                new KeyboardButton[] { "3. ¿Cómo cambio mi contraseña?" },
                new KeyboardButton[] { "Volver al Menú Principal" }
            })
            {
                ResizeKeyboard = true
            };


            await SendMessage(chatId, "Preguntas frecuentes: selecciona una opción para obtener más información.", tecladoPreguntas);


            esperandoRespuestaPreguntas = true;
        }

        private async Task ProcesarRespuestaPreguntas(long chatId, string mensaje)
        {
            esperandoRespuestaPreguntas = true;

            if (!esperandoRespuestaPreguntas)
                return;

            switch (mensaje)
            {
                case "1. ¿Cómo agendo una cita?":
                    await SendMessage(chatId, "Para agendar una cita, selecciona la opción 'Agendar Cita' en el menú principal. Luego, sigue los pasos para elegir una fecha, hora y motivo de la cita.");
                    break;

                case "2. ¿Cómo configuro recordatorios?":
                    await SendMessage(chatId, "Puedes configurar recordatorios desde el menú principal seleccionando la opción 'Confirmar Recordatorios'. Elige la cantidad de recordatorios y el sistema los enviará automáticamente.");
                    break;

                case "3. ¿Cómo cambio mi contraseña?":
                    await SendMessage(chatId, "Para cambiar tu contraseña, escribe 'Cambiar Contraseña' desde cualquier parte del sistema. Sigue las instrucciones para actualizar tu contraseña.");
                    break;

                case "Volver al Menú Principal":
                    await MostrarMenuPrincipal(chatId);
                    break;

                default:
                    await SendMessage(chatId, "La opción seleccionada no es válida. Por favor, elige una de las opciones del menú.");
                    break;
            }

            esperandoRespuestaPreguntas = false;
        }

        private async Task SoporteTecnico(long chatId)
        {
            string whatsappUrl = "https://wa.me/573332756400";

            string mensaje = $"🛠️ Para soporte técnico:\n\n" +
                             $"💬 Contáctanos directamente en [WhatsApp]({whatsappUrl}).\n" +
                             $"-------------------------------------------";

            await SendMessage(chatId, mensaje, new ReplyKeyboardRemove());
            await MostrarMenuPrincipal(chatId);
        }

        private async Task MostrarMenuCantidadRecordatorios(long chatId, Cita cita)
        {
            var tecladoCantidad = new ReplyKeyboardMarkup(new[]
            {
                    new KeyboardButton[] { "1", "2", "3" },
                    new KeyboardButton[] { "4", "5" },
                    new KeyboardButton[] { "Cancelar" }
                })
            {
                ResizeKeyboard = true
            };

            await SendMessage(chatId, $"¿Cuántos recordatorios deseas configurar antes de la cita del {cita.Fecha_Cita:dd/MM/yyyy}? Selecciona una opción del menú o elige 'Cancelar' para volver.", tecladoCantidad);
        }

        //Debido a problemas con la facturación con Twilio no es posible (a menos que se haga un pago considerable) el envio de sms a cualquier numero de telefono, esta limitado a mi número de telefono: 3216100436
        public async Task ProcesarCantidadRecordatorios(long chatId, string mensaje)
        {
            mensaje = mensaje.Trim();

            if (mensaje.Equals("Cancelar", StringComparison.OrdinalIgnoreCase))
            {
                await SendMessage(chatId, "La configuración de recordatorios ha sido cancelada. Volviendo al menú principal...");
                await MostrarMenuPrincipal(chatId);
                return;
            }

            if (!int.TryParse(mensaje, out int numeroRecordatorios) || numeroRecordatorios < 1 || numeroRecordatorios > 5)
            {
                await SendMessage(chatId, "La opción ingresada no es válida. Por favor selecciona un número entre 1 y 5 o elige 'Cancelar'.");
                return;
            }

            if (string.IsNullOrEmpty(cedulaPacienteIniciada))
            {
                await SendMessage(chatId, "No se pudo procesar tu solicitud. Por favor, inicia sesión de nuevo.");
                return;
            }

            var citasPendientes = _servicioCita.cargarPorFiltros("Solicitada", "Pendiente", cedulaPacienteIniciada)
                                               .OrderBy(c => c.Fecha_Cita)
                                               .ToList();

            if (citasPendientes == null || citasPendientes.Count == 0)
            {
                esperandoSeleccionRecordatorios = false;

                await SendMessage(chatId, "No tienes citas pendientes.");
                return;
            }

            var citaProxima = citasPendientes.First();
            var paciente = _servicioPaciente.obtenerPorCodigo(cedulaPacienteIniciada);
            if (paciente == null || string.IsNullOrEmpty(paciente.Telefono))
            {
                await SendMessage(chatId, "No se encontró un número de teléfono válido para este paciente.");
                return;
            }

            for (int i = 1; i <= numeroRecordatorios; i++)
            {
                string mensajeRecordatorio = $"Hola {paciente.PrimerNombre}, este es el recordatorio {i} de {numeroRecordatorios} para tu cita:" +
                                             $"\n📅 Fecha: {citaProxima.Fecha_Cita:dd/MM/yyyy}" +
                                             $"\n🕒 Hora: {citaProxima.Hora_Cita:hh\\:mm}" +
                                             $"\n💼 Motivo: {citaProxima.Razon_Cita}." +
                                             "\n¡Te esperamos!\n" +
                                             $"-------------------------------------------";
                try
                {
                    _servicioTwilio.enviarSMS(paciente.Telefono, mensajeRecordatorio);
                    await SendMessage(chatId, $"✅ Recordatorio {i}/{numeroRecordatorios} enviado para la cita del {citaProxima.Fecha_Cita:dd/MM/yyyy}.");
                }
                catch (Exception ex)
                {
                    await SendMessage(chatId, $"⚠️ Error al enviar el recordatorio {i}/{numeroRecordatorios}: {ex.Message}");
                }
            }

            esperandoSeleccionRecordatorios = false;
            await MostrarMenuPrincipal(chatId);
        }

        private async Task EnviarRecordatorios(long chatId, string mensaje)
        {
            if (!usuarioAutenticado || string.IsNullOrEmpty(cedulaPacienteIniciada))
            {
                await SendMessage(chatId, "Debes iniciar sesión para programar recordatorios.");
                return;
            }

            var citasPendientes = _servicioCita.cargarPorFiltros("Solicitada", "Pendiente", cedulaPacienteIniciada)
                                               .OrderBy(c => c.Fecha_Cita)
                                               .ToList();

            if (citasPendientes == null || citasPendientes.Count == 0)
            {
                esperandoSeleccionRecordatorios = false;
                await SendMessage(chatId, "No tienes citas pendientes.");
                return;
            }

            var citaProxima = citasPendientes.First();
            if (citaProxima.RecordatorioCita == false)
            {
                esperandoSeleccionRecordatorios = false;
                await SendMessage(chatId, $"Los recordatorios para la cita del {citaProxima.Fecha_Cita:dd/MM/yyyy} están desactivados.");
                return;
            }

            await MostrarMenuCantidadRecordatorios(chatId, citaProxima);
        }

        private async Task SendMessage(long chatId, string text, IReplyMarkup replyMarkup = null)
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: text,
                replyMarkup: replyMarkup
            );
        }
    }
}