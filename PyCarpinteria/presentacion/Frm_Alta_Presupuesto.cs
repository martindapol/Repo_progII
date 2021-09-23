using PyCarpinteria.dominio;
using PyCarpinteria.servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyCarpinteria.presentacion
{
    public partial class Frm_Alta_Presupuesto : Form
    {
        private IService servicio;


        Presupuesto oPresupuesto = new Presupuesto();
        public Frm_Alta_Presupuesto()
        {

            InitializeComponent();
            servicio = new ServiceFactoryImp().CrearService();

        }




        private void btnAceptar_Click(object sender, EventArgs e)
        {

            if (txtCliente.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar un tipo de cliente", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCliente.Focus();
                return;
            }
            if (txtDto.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el porcetnaje de descuento", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCliente.Focus();
                return;
            }

            //pasar datos al objeto 
            oPresupuesto.Cliente = txtCliente.Text;
            oPresupuesto.Descuento = Convert.ToDouble(txtDto.Text);
            oPresupuesto.Fecha = Convert.ToDateTime(txtFecha.Text);

            if (servicio.GrabarPresupuesto(oPresupuesto))
            {
                MessageBox.Show("Presupuesto guardado con éxito!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Error al intentar grabar el presupuesto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();

            }
            else
            {
                return;
            }
        }

        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            CargarCombo();
            ConsultarUltimoPresupuesto();


            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDto.Text = "0";
            txtCliente.Text = "CONSUMIDOR FINAL";


        }

        private void CargarCombo()
        {
            List<Producto> lst = servicio.ConsultarProductos();

            //source es una lista de objetos
            cboProducto.DataSource = lst;
            //valueMember y DisplayMember serán las properties de los objetos
            cboProducto.ValueMember = "IdProducto";
            cboProducto.DisplayMember = "Nombre";

            // cboProducto.Items.AddRange(lst.ToArray());

        }

        private void ConsultarUltimoPresupuesto()
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

            lblNro.Text = "Presupuesto Nro: " + param.Value.ToString();
            cnn.Close();

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ExisteProductoEnGrilla(cboProducto.Text))
            {
                MessageBox.Show("Producto ya agregado como detalle", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DetallePresupuesto item = new DetallePresupuesto();

            Producto oProducto = (Producto)cboProducto.SelectedItem;
            item.Producto = oProducto;
            item.Cantidad = (int)nudCantidad.Value;

            oPresupuesto.AgregarDetalle(item);

            dgvDetalles.Rows.Add(new object[] { "", oProducto.Nombre, oProducto.Precio, item.Cantidad, item.CalcularSubTotal() }); ;

            CalcularTotales();




        }

        private bool ExisteProductoEnGrilla(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["producto"].Value.Equals(text))
                    return true;
            }
            return false;
        }

        private void CalcularTotales()
        {
            double subTotal = oPresupuesto.CalcularTotal();
            double desc = (Double.Parse(txtDto.Text) * subTotal) / 100;
            lblSubtotal.Text = "Subtotal: " + subTotal.ToString();
            lblDescuento.Text = "Descuento: " + desc.ToString();
            lblTotal.Text = "Total: " + (subTotal - desc).ToString();

            //pasar total al objeto
            oPresupuesto.Total = subTotal - desc;
        }




        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 5)
            {
                oPresupuesto.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                CalcularTotales();
            }
        }


    }
}
