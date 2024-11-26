using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class DBPaciente : ConexionOracle
    {
        private List<Paciente> listaPacientes;

        public DBPaciente() { }
        public string guardarDato(Paciente paciente)
        {
            string query = "INSERT INTO PACIENTE (CEDULA_P, PRIMER_NOMBRE, SEGUNDO_NOMBRE, PRIMER_APELLIDO, " +
                           "SEGUNDO_APELLIDO, TELEFONO, FECHA_NACIMIENTO, CONTRASENA, ID_CONSULTORIO) " +
                           "VALUES (:Cedula, :PrimerNombre, :SegundoNombre, :PrimerApellido, :SegundoApellido, " +
                           ":Telefono, TO_DATE(:Fecha_De_Nacimiento, 'DD-MM-YYYY'), :Contrasena, :CodigoConsultorio)";

            OracleTransaction transaction = null;

            try
            {
                AbrirConexion(); 
                transaction = conexion.BeginTransaction(); 

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("Cedula", paciente.Cedula));
                    command.Parameters.Add(new OracleParameter("PrimerNombre", paciente.PrimerNombre));
                    command.Parameters.Add(new OracleParameter("SegundoNombre", paciente.SegundoNombre ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("PrimerApellido", paciente.PrimerApellido));
                    command.Parameters.Add(new OracleParameter("SegundoApellido", paciente.SegundoApellido ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Telefono", paciente.Telefono ?? (object)DBNull.Value));
                    command.Parameters.Add(new OracleParameter("Fecha_De_Nacimiento", paciente.Fecha_De_Nacimiento.ToString("dd-MM-yyyy")));
                    command.Parameters.Add(new OracleParameter("Contrasena", paciente.Contrasena));
                    command.Parameters.Add(new OracleParameter("CodigoConsultorio", paciente.CodigoConsultorio));

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
                return "Registro exitoso";
            }
            catch (OracleException ex) when (ex.Number == 1) 
            {
                transaction?.Rollback(); 
                return "Error: La cédula ya está registrada.";
            }
            catch (Exception ex)
            {
                transaction?.Rollback(); 
                return "Error: " + ex.Message;
            }
            finally
            {
                CerrarConexion(); 
            }
        }


        private Paciente Map(OracleDataReader reader)
        {
            return new Paciente
            {
                Cedula = Convert.ToString(reader["CEDULA_P"]),
                PrimerNombre = Convert.ToString(reader["PRIMER_NOMBRE"]),
                SegundoNombre = Convert.ToString(reader["SEGUNDO_NOMBRE"]),
                PrimerApellido = Convert.ToString(reader["PRIMER_APELLIDO"]),
                SegundoApellido = Convert.ToString(reader["SEGUNDO_APELLIDO"]),
                Telefono = Convert.ToString(reader["TELEFONO"]),
                Fecha_De_Nacimiento = Convert.ToDateTime(reader["FECHA_NACIMIENTO"]),
                Contrasena = Convert.ToString(reader["CONTRASENA"]),
                CodigoConsultorio = Convert.ToString(reader["ID_CONSULTORIO"])
            };
        }
        public List<Paciente> obtenerTodos()
        {
            listaPacientes = new List<Paciente>();
            string query = "SELECT * FROM PACIENTE";

            try
            {
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    AbrirConexion();
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaPacientes.Add(Map(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<Paciente>();
            }
            finally
            {
                CerrarConexion();
            }
            return listaPacientes;
        }

        public string actualizarContrasena(string cedula, string nuevaContrasena)
        {
            string query = "UPDATE PACIENTE SET CONTRASENA = :NuevaContrasena WHERE CEDULA_P = :Cedula";

            try
            {
                AbrirConexion(); // Abre la conexión a la base de datos.

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("NuevaContrasena", nuevaContrasena));
                    command.Parameters.Add(new OracleParameter("Cedula", cedula));

                    int filasAfectadas = command.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return "Contraseña actualizada exitosamente";
                    }
                    else
                    {
                        return "Error: No se encontró un paciente con esa cédula.";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
            finally
            {
                CerrarConexion(); // Cierra la conexión a la base de datos.
            }
        }


        public Paciente obtenerPorCodigo(string ID)
        {
            listaPacientes = obtenerTodos();
            return listaPacientes.Find(x => x.Cedula == ID);
        }
        public Paciente obtenerPorUsuario(string ID, string contrasena)
        {
            listaPacientes = obtenerTodos();
            return listaPacientes.FirstOrDefault(x => x.Cedula == ID && x.Contrasena == contrasena);
        }
        public List<Paciente> cargarPorCodigo(string codigo)
        {
            return obtenerTodos().Where(paciente => paciente.Cedula == codigo).ToList();
        }

    }
}
