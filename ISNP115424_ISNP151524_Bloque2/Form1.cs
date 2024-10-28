using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ISNP115424_ISNP151524_Bloque2
{
    public partial class Form1 : Form
    {
        db_conexion objConexion = new db_conexion();
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        public int posicion = 0;
        String accion = "nuevo";
        private string ConnectionString;

        public Form1()
        {
            InitializeComponent();
            CargarPeliculas();
        }
        private void CargarPeliculas()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Peliculas", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridViewPeliculas.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar películas: " + ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            obtenerDatos();
        }
        private void obtenerDatos()
        {
            ds = objConexion.obtenerDatos();
            dt = ds.Tables["peliculas"];
            dt.PrimaryKey = new DataColumn[] { dt.Columns["idPeliculas"] };
            mostrarDatos();
        }
        private void habDesControles(Boolean estado)
        {
            btnEliminar.Enabled = !estado;
        }
        private void mostrarDatos()
        {
            txtTitulo.Text = dt.Rows[posicion].ItemArray[1].ToString();
            txtAutor.Text = dt.Rows[posicion].ItemArray[2].ToString();
            txtSinopsis.Text = dt.Rows[posicion].ItemArray[3].ToString();
            txtDuracion.Text = dt.Rows[posicion].ItemArray[4].ToString();
            txtClasificacion.Text = dt.Rows[posicion].ItemArray[5].ToString();

        }
        private void limpiarCajas()
        {
            txtTitulo.Text = "";
            txtAutor.Text = "";
            txtSinopsis.Text = "";
            txtDuracion.Text = "";
            txtClasificacion.Text = "";

        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (btnGuardar.Text == "Guardar")
            {
                btnGuardar.Text = "Guardar";
                btnModificar.Text = "Cancelar";
                limpiarCajas();
                accion = "nuevo";
                habDesControles(true);
            }
            else
            {//guardar
                String[] datos = {
                    accion,
                    dt.Rows[posicion].ItemArray[0].ToString(), //idPeliculas
                    txtTitulo.Text,
                    txtAutor.Text,
                    txtSinopsis.Text,
                    txtDuracion.Text,
                    txtClasificacion.Text
                };
                String response = objConexion.AdministrarPeliculas(datos);
                if (response != "1")
                {
                    MessageBox.Show("Error: " + response, "Registrando datos de Peliculas", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    obtenerDatos();
                    habDesControles(false);
                    btnGuardar.Text = "Guardar";
                    btnModificar.Text = "Modificar";
                }
            }
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (btnModificar.Text == "Modificar")
            {
                btnModificar.Text = "Cancelar";
                btnGuardar.Text = "Guardar";
                accion = "modificar";
                habDesControles(true);
            }
            else
            {//cancelar

                mostrarDatos();
                habDesControles(false);
                btnModificar.Text = "Modificar";
                btnGuardar.Text = "Guardar";
            }

        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Esta seguro de eliminar a: " + txtTitulo.Text, "Eliminando Peliculas",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                String[] datos =
                {
                    "eliminar", dt.Rows[posicion].ToString(),//idPeliculas
                };
                String response = objConexion.AdministrarPeliculas(datos);
                if (response != "1")
                {
                    MessageBox.Show("error: " + response, "Eliminando datos de peliculas", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    obtenerDatos();
                }
            }

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla '_db_peliculasDataSet.Peliculas' Puede moverla o quitarla según sea necesario.
            this.peliculasTableAdapter.Fill(this._db_peliculasDataSet.Peliculas);
            CargarPeliculas();
        }
    }
}