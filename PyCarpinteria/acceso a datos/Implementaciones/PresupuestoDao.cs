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
            catch (SqlException)
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
                    //validar que fecha_baja no es null:
                    if (!row["fecha_baja"].Equals(DBNull.Value))
                        oPresupuesto.FechaBaja = Convert.ToDateTime(row["fecha_baja"].ToString());

                    lst.Add(oPresupuesto);
                }

                cnn.Close();
            }
            catch (SqlException)
            {
                lst = null;
            }
            return lst;
        }

        public Presupuesto GetById(int id)
        {
            Presupuesto oPresupuesto = new Presupuesto();
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_CONSULTAR_PRESUPUESTO_POR_ID";
            cmd.Parameters.AddWithValue("@nro", id);
            SqlDataReader reader = cmd.ExecuteReader();
            bool esPrimerRegistro = true;

            while (reader.Read())
            {
                if (esPrimerRegistro)
                {
                    //solo para el primer resultado recuperamos los datos del MAESTRO:
                    oPresupuesto.PresupuestoNro = Convert.ToInt32(reader["presupuesto_nro"].ToString());
                    oPresupuesto.Cliente = reader["cliente"].ToString();
                    oPresupuesto.Fecha = Convert.ToDateTime(reader["fecha"].ToString());
                    oPresupuesto.Descuento = Convert.ToDouble(reader["descuento"].ToString());
                    oPresupuesto.PresupuestoNro = Convert.ToInt32(reader["presupuesto_nro"].ToString());
                    oPresupuesto.Total = Convert.ToDouble(reader["total"].ToString());
                    esPrimerRegistro = false;
                }

                DetallePresupuesto oDetalle = new DetallePresupuesto();
                Producto oProducto = new Producto();
                oProducto.IdProducto = Convert.ToInt32(reader["id_producto"].ToString());
                oProducto.Nombre = reader["n_producto"].ToString();
                oProducto.Precio = Convert.ToDouble(reader["precio"].ToString());
                oProducto.Activo = reader["activo"].ToString().Equals("S");
                oDetalle.Producto = oProducto;
                oDetalle.Cantidad = Convert.ToInt32(reader["cantidad"].ToString());
                esPrimerRegistro = false;
                oPresupuesto.AgregarDetalle(oDetalle);
            }
            return oPresupuesto;
        }

        public int GetNextPresupuestoId()
        {
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");
            cnn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SP_PROXIMO_ID";

            SqlParameter param = new SqlParameter();
            param.ParameterName = "@next";
            param.SqlDbType = SqlDbType.Int;
            param.Direction = ParameterDirection.Output;

            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();
            cnn.Close();

            return (int)param.Value;
        }

        public List<Producto> GetProductos()
        {
            List<Producto> lst = new List<Producto>();
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");
            cnn.Open();
            SqlCommand cmd2 = new SqlCommand("SP_CONSULTAR_PRODUCTOS", cnn);

            cmd2.CommandType = CommandType.StoredProcedure;

            DataTable table = new DataTable();
            table.Load(cmd2.ExecuteReader());

            cnn.Close();

            foreach (DataRow row in table.Rows)
            {
                Producto oProducto = new Producto();
                oProducto.IdProducto = Convert.ToInt32(row["id_producto"].ToString());
                oProducto.Nombre = row["n_producto"].ToString();
                oProducto.Precio = Convert.ToDouble(row["precio"].ToString());
                oProducto.Activo = row["activo"].ToString().Equals("S");

                lst.Add(oProducto);
            }

            return lst;
        }

        public bool Save(Presupuesto oPresupuesto)
        {
            SqlTransaction transaccion = null;
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");

            bool flag = true;
            try
            {
                cnn.Open();
                transaccion = cnn.BeginTransaction();

                SqlCommand cmdMaestro = new SqlCommand("SP_INSERTAR_MAESTRO", cnn, transaccion);
                cmdMaestro.CommandType = CommandType.StoredProcedure;
                cmdMaestro.Parameters.AddWithValue("@cliente", oPresupuesto.Cliente);
                cmdMaestro.Parameters.AddWithValue("@dto", oPresupuesto.Descuento);
                cmdMaestro.Parameters.AddWithValue("@total", oPresupuesto.Total);

                SqlParameter param = new SqlParameter();
                param.ParameterName = "@presupuesto_nro";
                param.SqlDbType = SqlDbType.Int;

                param.Direction = ParameterDirection.Output;
                cmdMaestro.Parameters.Add(param);
                cmdMaestro.ExecuteNonQuery();

                int nroPresupuesto = (int)param.Value;
                int nroDetalle = 0;

                foreach (DetallePresupuesto det in oPresupuesto.Detalles)
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

            }
            catch
            {
                transaccion.Rollback();
                flag = false;

            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                    cnn.Close();

            }

            return flag;

        }
    }
}