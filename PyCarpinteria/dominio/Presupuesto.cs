using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace PyCarpinteria.dominio
{
    class Presupuesto
    {
        public int PresupuestoNro { get; set; }
        public DateTime Fecha { get; set; }

        public string Cliente { get; set; }

        public double Total { get; set; }
        public double Descuento { get; set; }

        public DateTime FechaBaja { get; set; }

        public List<DetallePresupuesto> Detalles { get; }

        public Presupuesto()
        {
            //generar la relación 1 a muchos
            Detalles = new List<DetallePresupuesto>();
        }


        public void AgregarDetalle(DetallePresupuesto detalle)
        {
            Detalles.Add(detalle);
        }


        public void QuitarDetalle(int nro)
        {
            Detalles.RemoveAt(nro);
        }

        public double CalcularTotal()
        {
            double total = 0;
            foreach(DetallePresupuesto item in Detalles)
            {
                total += item.CalcularSubTotal();
            }
            return total;

        }

        public bool Confirmar()
        {
            SqlTransaction transaccion = null;
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = carpinteria_db; Integrated Security = True");

            bool flag = true;
            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();

                SqlCommand cmdMaestro = new SqlCommand("SP_INSERTAR_MAESTRO", cnn, transaccion);
                cmdMaestro.CommandType = CommandType.StoredProcedure;
                cmdMaestro.Parameters.AddWithValue("@cliente", Cliente);
                cmdMaestro.Parameters.AddWithValue("@dto", Descuento);
                cmdMaestro.Parameters.AddWithValue("@total", Total);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@presupuesto_nro";
                param.SqlDbType = SqlDbType.Int;

                param.Direction = ParameterDirection.Output;
                cmdMaestro.Parameters.Add(param);
                cmdMaestro.ExecuteNonQuery();

                int nroPresupuesto = (int)param.Value;
                int nroDetalle = 0;

                foreach (DetallePresupuesto det in Detalles)
                {
                    SqlCommand cmd = new SqlCommand("SP_INSERTAR_DETALLE", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Transaction = transaccion;
                    cmd.Parameters.AddWithValue("@id_producto", det.Producto.IdProducto);
                    cmd.Parameters.AddWithValue("@cantidad ", det.Cantidad);
                    cmd.Parameters.AddWithValue("@presupuesto_nro", nroPresupuesto);
                    cmd.Parameters.AddWithValue("@detalle", ++nroDetalle);
                    cmd.ExecuteNonQuery();
                }

                transaccion.Commit();

                
            } catch
            {
                transaccion.Rollback();
                flag = false;

            } finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                    cnn.Close();

            }

            return flag;
       
        }
    }
}
