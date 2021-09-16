using PyCarpinteria.acceso_a_datos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PyCarpinteria.acceso_a_datos.Implementaciones
{
    class PresupuestoDao : IPresupuestoDao
    {
        public bool Delete(int nro)
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
                cmd.Parameters.AddWithValue("@presupuesto_nro", nro);
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

            return affected == 1;

        }
    }
}