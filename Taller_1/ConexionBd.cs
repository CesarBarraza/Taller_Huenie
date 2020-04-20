using System;
using System.Data.SqlClient;

namespace Taller_1
{
    class ConexionBd
    {
        string conexion = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=Taller;Integrated Security=true;";
        public SqlConnection conectarbd = new SqlConnection();

        public ConexionBd()
        {
            conectarbd.ConnectionString = conexion;
        }

        public void abrir()
        {
            try
            {
                conectarbd.Open();
                Console.Write("Console Abierta");
            }
            catch (Exception ex)
            {
                Console.Write("Error al abrir la coneccion" + ex.Message);
            }
        }

        public void cerrar()
        {
            conectarbd.Close();
        }
    }
}
