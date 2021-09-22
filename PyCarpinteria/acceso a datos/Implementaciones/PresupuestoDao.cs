using PyCarpinteria.acceso_a_datos.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using PyCarpinteria.dominio;
using PyCarpinteria.servicios;

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

       public List<Presupuesto> GetByFilters(List<Parametro> criterios)
        {
            List<Presupuesto> lst = new List<Presupuesto>();
            DataTable table = new DataTable();
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");
            try
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand("SP_CONSULTAR_PRESUPUESTOS", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (Parametro p in criterios)
                {
                    cmd.Parameters.AddWithValue(p.Nombre, p.Valor);
                }

                table.Load(cmd.ExecuteReader());
                //mappear los registros como objetos del dominio:

                foreach (DataRow row in table.Rows)
                {
                    //Por cada registro creamos un objeto del dominio
                    Presupuesto oPresupuesto = new Presupuesto();
                    oPresupuesto.Cliente = row["cliente"].ToString();
                    oPresupuesto.Fecha = Convert.ToDateTime(row["fecha"].ToString());
                    oPresupuesto.Descuento = Convert.ToDouble(row["descuento"].ToString());
                    oPresupuesto.PresupuestoNro = Convert.ToInt32(row["presupuesto_nro"].ToString());
                    oPresupuesto.Total = Convert.ToDouble(row["total"].ToString());
                    oPresupuesto.FechaBaja = Convert.ToDateTime(row["fecha_baja"].ToString());
                    lst.Add(oPresupuesto);
                }

                cnn.Close();

            }
            catch (SqlException ex) {
                string mensaje = ex.Message;
            
            }
            return lst;
        }
    }
}