using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using ENTITY;
using System.Linq;

namespace DAL
{
    public class DBFactura : ConexionOracle
    {
        public string guardarDato(Factura factura)
        {
            try
            {
                string query = "INSERT INTO FACTURA (ID_FACTURA, FECHA_EMISION, ESTADO, MONTO_TOTAL, TOTAL_PAGADO, CAMBIO, CEDULA_PACIENTE) " +
                               "VALUES (:ID_Factura, TO_DATE(:Fecha_Emision, 'DD-MM-YYYY'), :Estado, :Total, :Total_Pagado, :Cambio, :CedulaPaciente)";
                OracleTransaction transaction = null;
                AbrirConexion();

                transaction = conexion.BeginTransaction();
                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    command.Parameters.Add(new OracleParameter("ID_Factura", factura.ID_Factura));
                    command.Parameters.Add(new OracleParameter("Fecha_Emision", factura.Fecha_Emision.ToString("dd-MM-yyyy")));
                    command.Parameters.Add(new OracleParameter("Estado", factura.Estado));
                    command.Parameters.Add(new OracleParameter("Total", (float)factura.Total));
                    command.Parameters.Add(new OracleParameter("Total_Pagado", (float)factura.Total_Pagado));
                    command.Parameters.Add(new OracleParameter("Cambio", (float)factura.Cambio));
                    command.Parameters.Add(new OracleParameter("CedulaPaciente", factura.CedulaPaciente)); 

                    command.ExecuteNonQuery();
                }
                transaction.Commit();
                return "Factura guardada correctamente.";
            }
            catch (Exception ex)
            {
                return $"Error al guardar la factura: {ex.Message}";
            }
            finally
            {
                CerrarConexion();
            }
        }

        private Factura Map(OracleDataReader reader)
        {
            return new Factura
            {
                ID_Factura = Convert.ToString(reader["ID_FACTURA"]),
                Fecha_Emision = Convert.ToDateTime(reader["FECHA_EMISION"]),
                Estado = Convert.ToString(reader["ESTADO"]),
                Total = Convert.ToDouble(reader["MONTO_TOTAL"]),
                Total_Pagado = Convert.ToDouble(reader["TOTAL_PAGADO"]),
                Cambio = Convert.ToDouble(reader["CAMBIO"]),
                CedulaPaciente = Convert.ToString(reader["CEDULA_PACIENTE"]) 
            };
        }

        public List<Factura> obtenerTodos()
        {
            List<Factura> facturas = new List<Factura>();
            try
            {
                string query = "SELECT * FROM FACTURA ORDER BY TO_NUMBER(SUBSTR(ID_FACTURA, 3))";

                AbrirConexion();

                using (OracleCommand command = new OracleCommand(query, conexion))
                {
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            facturas.Add(Map(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return new List<Factura>();
            }
            finally
            {
                CerrarConexion();
            }
            return facturas;
        }
        public List<Factura> cargarPorCedulaPaciente(string codigo)
        {
            return obtenerTodos().Where(Factura => Factura.CedulaPaciente == codigo).ToList();
        }

        public Factura obtenerPorCodigo(string id)
        {
            return obtenerTodos().FirstOrDefault<Factura>(x => x.ID_Factura == id);
        }

        public string actualizarDato(Factura factura)
        {
            try
            {
                string query = "UPDATE FACTURA SET FECHA_EMISION = TO_DATE(:Fecha_Emision, 'DD-MM-YYYY'), ESTADO = :Estado, " +
                               "MONTO_TOTAL = :Total, TOTAL_PAGADO = :Total_Pagado, CAMBIO = :Cambio, CEDULA_PACIENTE = :CedulaPaciente " +
                               "WHERE ID_FACTURA = :ID_Factura";

                AbrirConexion();

                using (OracleTransaction transaction = conexion.BeginTransaction())
                {
                    using (OracleCommand command = new OracleCommand(query, conexion))
                    {
                        command.Parameters.Add(new OracleParameter("Fecha_Emision", factura.Fecha_Emision.ToString("dd-MM-yyyy")));
                        command.Parameters.Add(new OracleParameter("Estado", factura.Estado));
                        command.Parameters.Add(new OracleParameter("Total", Convert.ToDecimal(factura.Total)));
                        command.Parameters.Add(new OracleParameter("Total_Pagado", Convert.ToDecimal(factura.Total_Pagado)));
                        command.Parameters.Add(new OracleParameter("Cambio", Convert.ToDecimal(factura.Cambio)));
                        command.Parameters.Add(new OracleParameter("CedulaPaciente", factura.CedulaPaciente));
                        command.Parameters.Add(new OracleParameter("ID_Factura", factura.ID_Factura));

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine($"Advertencia: No se encontró una factura con ID {factura.ID_Factura} para actualizar.");
                            transaction.Rollback();
                            return "No se encontró la factura para actualizar.";
                        }

                        transaction.Commit();
                        return "Factura actualizada correctamente.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta SQL: " + ex.Message);
                return $"Error al actualizar la factura: {ex.Message}";
            }
            finally
            {
                CerrarConexion();
            }
        }



    }
}
