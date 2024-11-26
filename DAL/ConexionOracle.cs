using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConexionOracle
    {
        private string cadenaConexion = "User Id=esquema_agenda;Password=1234;Data Source=localhost:1521/xepdb1;";

        protected OracleConnection conexion;

        public ConexionOracle()
        {
            conexion = new OracleConnection(cadenaConexion);
        }

        public bool AbrirConexion()
        {
            if (conexion.State != ConnectionState.Open)
            {
                conexion.Open();
                return true;
            }
            return false;
        }

        public void CerrarConexion()
        {
            if (conexion.State == ConnectionState.Open)
            {
                conexion.Close();
            }
        }
    }
}
