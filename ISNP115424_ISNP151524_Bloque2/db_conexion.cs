using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ISNP115424_ISNP151524_Bloque2
{
    internal class db_conexion
    {
        SqlConnection miConexion = new SqlConnection();
        SqlCommand miComando = new SqlCommand();
        SqlDataAdapter miAdaptador = new SqlDataAdapter();
        DataSet ds = new DataSet();//Arquictectura de la base de datos en memoria RAM

        public db_conexion()
        {
            miConexion.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\db.peliculas.mdf;Integrated Security=True";
            miConexion.Open();
        }
        public DataSet obtenerDatos()
        {
            ds.Clear();
            miComando.Connection = miConexion;
            miComando.CommandText = "SELECT * FROM Peliculas";

            miAdaptador.SelectCommand = miComando;
            miAdaptador.Fill(ds, "Peliculas");

            return ds;

        }
        public string AdministrarPeliculas(string[] pelicula)
        {
            string sql = "";
            if (pelicula[0] == "nuevo") { //accion nuevo
                sql = "INSERT INTO Peliculas(Titulo, Autor, Sinopsis, Duracion, Clasificacion) VALUES(" +
                    "'" + pelicula[2] + "'," +
                    "'" + pelicula[3] + "'," +
                    "'" + pelicula[4] + "'," +
                    "'" + pelicula[5] + "'," +
                    "'" + pelicula[6] + "'," +
                    "'" + pelicula[7] + "'," +
                    "'" + pelicula[8] + "'," +
                    "'" + pelicula[9] + "'," +
                    "'" + pelicula[10] + "',";
            }
            else if (pelicula[0] == "Modificar")
            {
                sql = "UPDATE pelicuas SET codigo= '" + pelicula[2] + "', Titulo='" + pelicula[3] + "'," +
                    "Autor='" + pelicula[4] + "', Sinopsis='" + pelicula[5] + "'," + "Duracion='" + pelicula[6] + "', Clasificacion='" + pelicula[7] + "'." + pelicula[8] + "'," + pelicula[9] + "'," +
                    pelicula[10] + "' WHERE idPeliculas=" + pelicula[1];
            }
            else if (pelicula[0] == "eliminar")
            {
                sql = "DELETE FROM peliculas WHERE idPeliculas='" + pelicula[1] + "'";
            }
            return ejecutarSQL(sql);

        }  
                    
        private string ejecutarSQL (String sql)
        {
            try
            {
                miComando.Connection = miConexion;
                miComando.CommandText = sql;
                return miComando.ExecuteNonQuery().ToString();
            } catch (Exception ex)
            {
                return ex.Message;
            }
        }       
        
    }
}
