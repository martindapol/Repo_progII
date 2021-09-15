using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace PyCarpinteria.presentacion
{
    public partial class FrmConsultarPresupuestos : Form
    {
        public FrmConsultarPresupuestos()
        {
            InitializeComponent();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //Validar los filtros fecha desde <= fecha hasta
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");

            DataTable table = new DataTable();
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SP_CONSULTAR_PRESUPUESTOS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@fecha_desde", dtpDesde.Value);
            cmd.Parameters.AddWithValue("@fecha_hasta", dtpHasta.Value);

            if (String.IsNullOrEmpty(txtCliente.Text))
                cmd.Parameters.AddWithValue("@cliente", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@cliente", txtCliente.Text);

            if (chkBaja.Checked)
                cmd.Parameters.AddWithValue("@datos_baja", "S");
            else
                cmd.Parameters.AddWithValue("@datos_baja", "N");

            table.Load(cmd.ExecuteReader());
            cnn.Close();

            dgvResultados.Rows.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                dgvResultados.Rows.Add(new object[]{
                                        table.Rows[i]["presupuesto_nro"],
                                        table.Rows[i]["fecha"],
                                        table.Rows[i]["cliente"],
                                        table.Rows[i]["descuento"],
                                        table.Rows[i]["total"],
                                        table.Rows[i]["fecha_baja"]
                 });
            }

            //dgvResultados.DataSource = table;


        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvResultados.CurrentRow;
            if (row != null)
            {
                int presupuesto = Int32.Parse(row.Cells["colNro"].Value.ToString());
                if (MessageBox.Show("Seguro que desea eliminar el presupuesto seleccionado?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");
                    SqlTransaction t = null;
                    int affected = 0;
                    try
                    {
                        cnn.Open();
                        t = cnn.BeginTransaction();
                        SqlCommand cmd = new SqlCommand("SP_REGISTRAR_BAJA_PRESUPUESTOS", cnn, t);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@presupuesto_nro", presupuesto);
                        affected = cmd.ExecuteNonQuery();
                        t.Commit();
                    }
                    catch (SqlException ex)
                    {
                        t.Rollback();

                    }
                    finally
                    {
                        if (cnn != null && cnn.State == ConnectionState.Open)
                            cnn.Close();
                    }

                    if (affected == 1) { 
                        MessageBox.Show("Presupuesto eliminado!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnConsultar_Click(sender, e);
                    }
                    else
                        MessageBox.Show("Error al intentar borrar el presupuesto!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

