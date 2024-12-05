using ENTITY;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBInformes : ConexionOracle
    {
        public DBInformes() { }

        private List<Dictionary<string, object>> Map(OracleDataReader reader)
        {
            var resultado = new List<Dictionary<string, object>>();

            while (reader.Read())
            {
                var fila = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    fila[reader.GetName(i)] = reader.GetValue(i);
                }
                resultado.Add(fila);
            }

            return resultado;
        }

        public List<Dictionary<string, object>> ObtenerVistasCitasPorMes()
        {
            string query = "SELECT * FROM VistaCitasPorMes";
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        resultado = Map(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar citas por mes: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public List<Dictionary<string, object>> ObtenerVistasCitasPorAnio()
        {
            string query = "SELECT * FROM VistaCitasPorAnio";
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        resultado = Map(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar citas por anio: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public List<Dictionary<string, object>> ObtenerVistasFacturasPorMes()
        {
            string query = "SELECT * FROM VistaFacturasPorMes";
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        resultado = Map(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar citas por mes: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public List<Dictionary<string, object>> ObtenerVistasFacturasPorAnio()
        {
            string query = "SELECT * FROM VistaFacturasPorAnio";
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        resultado = Map(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar citas por anio: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public List<Dictionary<string, object>> ObtenerVistasPagosPorMes()
        {
            string query = "SELECT * FROM VistaPagosPorMes";
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        resultado = Map(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar citas por mes: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public List<Dictionary<string, object>> ObtenerVistasPagosPorAnio()
        {
            string query = "SELECT * FROM VistaPagosPorAnio";
            var resultado = new List<Dictionary<string, object>>();

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        resultado = Map(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar citas por anio: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerVistaTotalesUsuarios()
        {
            string query = "SELECT * FROM VistaTotalDeUsuarios";
            Dictionary<string, object> resultado = null;

            try
            {
                AbrirConexion();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) 
                        {
                            resultado = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                resultado[reader.GetName(i)] = reader.GetValue(i);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar los totales de usuarios: " + ex.Message);
            }
            finally
            {
                CerrarConexion();
            }

            return resultado;
        }

        public Dictionary<string, object> ObtenerVistaCitaPorMes(int anio, int mes)
        {
            return ObtenerVistasCitasPorMes().FirstOrDefault(fila => fila.ContainsKey("ANIO") && Convert.ToInt32(fila["ANIO"]) == anio &&
                                                      fila.ContainsKey("MES") && Convert.ToInt32(fila["MES"]) == mes);
        }

        public Dictionary<string, object> ObtenerVistaCitaPorAnio(int anio)
        {
            return ObtenerVistasCitasPorAnio().FirstOrDefault(fila => fila.ContainsKey("ANIO") && Convert.ToInt32(fila["ANIO"]) == anio);
        }

        public Dictionary<string, object> ObtenerVistaFacturaPorMes(int anio, int mes)
        {
            return ObtenerVistasFacturasPorMes().FirstOrDefault(fila => fila.ContainsKey("ANIO") && Convert.ToInt32(fila["ANIO"]) == anio &&
                                                      fila.ContainsKey("MES") && Convert.ToInt32(fila["MES"]) == mes);
        }

        public Dictionary<string, object> ObtenerVistaFacturaPorAnio(int anio)
        {
            return ObtenerVistasFacturasPorAnio().FirstOrDefault(fila => fila.ContainsKey("ANIO") && Convert.ToInt32(fila["ANIO"]) == anio);
        }

        public Dictionary<string, object> ObtenerVistaPagoPorMes(int anio, int mes)
        {
            return ObtenerVistasPagosPorMes().FirstOrDefault(fila => fila.ContainsKey("ANIO") && Convert.ToInt32(fila["ANIO"]) == anio &&
                                                      fila.ContainsKey("MES") && Convert.ToInt32(fila["MES"]) == mes);
        }

        public Dictionary<string, object> ObtenerVistaPagoPorAnio(int anio)
        {
            return ObtenerVistasPagosPorAnio().FirstOrDefault(fila => fila.ContainsKey("ANIO") && Convert.ToInt32(fila["ANIO"]) == anio);
        }


    }
}
