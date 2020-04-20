using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Taller_1
{
    public partial class FmrCliente : Form
    {
        
        public int IdCliente;
        public FmrCliente()
        {
            InitializeComponent();
        }

        //Boton guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Cliente c = new Cliente
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Fecha_Nac = dtpFecha.Value.Year.ToString() + '/' + dtpFecha.Value.Month.ToString() + '/' + dtpFecha.Value.Day.ToString(),
                Direccion = txtDireccion.Text
            };
            ConexionBd bd = new ConexionBd();
            string agregar = string.Format("Insert into clientes (Nombre, Apellido, Fecha_Nacimiento, Direccion) values ('{0}','{1}','{2}','{3}')",
            c.Nombre, c.Apellido, c.Fecha_Nac, c.Direccion);

            SqlCommand comando = new SqlCommand(agregar, bd.conectarbd);
                bd.abrir();
                if (IdCliente == 0)
                {
                    comando.ExecuteNonQuery();
                    this.cargarGrid();
                    MessageBox.Show("Se agregaron los datos correctamente");
                    this.limpiar(this);
                }
                else
                {
                    actualizar();
                    this.limpiar(this);
                    MessageBox.Show("Los cambios se realizaron correcatamente");
                    IdCliente = 0;
                }           
            bd.cerrar();
        }

        //Selecciona del dataDridView
        private void dataGridView_Click(object sender, EventArgs e)
        {
            IdCliente = Convert.ToInt32(dataGridView.CurrentRow.Cells[0].Value);
            txtNombre.Text = Convert.ToString(dataGridView.CurrentRow.Cells[1].Value);
            txtApellido.Text = Convert.ToString(dataGridView.CurrentRow.Cells[2].Value);

            dtpFecha.Value = Convert.ToDateTime(dataGridView.CurrentRow.Cells[3].Value);
            txtDireccion.Text = Convert.ToString(dataGridView.CurrentRow.Cells[4].Value);
        }

        //Boton cancelar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            limpiar(this);
            IdCliente = 0;
        }

        //Boton eliminar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ConexionBd bd = new ConexionBd();
            String eliminar = string.Format("Delete from clientes where IdCliente='{0}'", IdCliente);
            SqlCommand comando = new SqlCommand(eliminar, bd.conectarbd);
            bd.abrir();
            if (IdCliente != 0)
            {
                if (MessageBox.Show("Estas seguro que deseas eliminar?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Se elimino correctamente...");
                    limpiar(this);
                }
            }
            bd.cerrar();
        }

        //Funcion para actualizar datos
        private void actualizar()
        {
            ConexionBd bd = new ConexionBd();
            string actualizar = string.Format("Update clientes Set Nombre = @nombre, Apellido = @apellido, Fecha_Nacimiento = @fecha, Direccion = @direccion where IDCliente='{0}'", IdCliente);
            SqlCommand comando = new SqlCommand(actualizar, bd.conectarbd);
            bd.abrir();
            comando.Parameters.AddWithValue("@nombre", txtNombre.Text);
            comando.Parameters.AddWithValue("@apellido", txtApellido.Text);
            comando.Parameters.AddWithValue("@fecha", dtpFecha.Value.Year.ToString() + '/' + dtpFecha.Value.Month.ToString() + '/' + dtpFecha.Value.Day.ToString());
            comando.Parameters.AddWithValue("@direccion", txtDireccion.Text);
            comando.ExecuteNonQuery();
            bd.cerrar();
        }

        //Funcion que carga el gridView al comienzo
        private void cargarGrid()
        {
            List<Cliente> lista = new List<Cliente>();
            ConexionBd bd = new ConexionBd();
            //String consulta = "SELECT IdCliente, Nombre, Apellido, Fecha_Nacimiento, Direccion FROM clientes";
            SqlCommand comando = new SqlCommand("GetCliente", bd.conectarbd);
            comando.CommandType = System.Data.CommandType.StoredProcedure;

            bd.abrir();
            SqlDataReader reader = comando.ExecuteReader();         
                while (reader.Read())
                {
                    Cliente pCliente = new Cliente();
                    pCliente.Id = reader.GetInt32(0);
                    pCliente.Nombre = reader.GetString(1);
                    pCliente.Apellido = reader.GetString(2);
                    pCliente.Fecha_Nac = reader.GetString(3);
                    pCliente.Direccion = reader.GetString(4);
                    lista.Add(pCliente);
                }          
            bd.cerrar();
            dataGridView.DataSource = lista;
        }

        //Funcion que limpia los campos del formulario
        private void limpiar(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }

                if (c.Controls.Count > 0)
                {
                    limpiar(c);
                }
            }
            this.txtNombre.Focus();
            cargarGrid();
        }

        //Boton de busqueda
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            List<Cliente> lista = new List<Cliente>();
            ConexionBd bd = new ConexionBd();
            string buscar = string.Format("select * from clientes where Apellido = '{0}'", txtApellido.Text);
            SqlCommand comando = new SqlCommand(buscar, bd.conectarbd);
            bd.abrir();
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                Cliente pCliente = new Cliente();
                pCliente.Id = reader.GetInt32(0);
                pCliente.Nombre = reader.GetString(1);
                pCliente.Apellido = reader.GetString(2);
                pCliente.Fecha_Nac = reader.GetString(3);
                pCliente.Direccion = reader.GetString(4);
                lista.Add(pCliente);
            }
            bd.cerrar();
            dataGridView.DataSource = lista;
        }

        private void FmrCliente_Load(object sender, EventArgs e)
        {
            cargarGrid();
        }
    }
}
