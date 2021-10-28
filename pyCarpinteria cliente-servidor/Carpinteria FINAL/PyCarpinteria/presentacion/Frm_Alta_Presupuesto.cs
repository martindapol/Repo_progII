using PyCarpinteria.dominio;

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
    public enum Accion
    {
        CREATE,
        READ,
        UPDATE,
        DELETE
    }


    public partial class Frm_Alta_Presupuesto : Form
    {
        private IService servicio;
        private Accion modo;

        Presupuesto oPresupuesto = new Presupuesto();
        public Frm_Alta_Presupuesto(Accion modo, int nro)
        {
            InitializeComponent();
            servicio = new ServiceFactoryImp().CrearService();
            this.modo = modo;
           

            if (modo.Equals(Accion.READ))
            {
                gbDatosPresupuesto.Enabled = false;
                btnAceptar.Enabled = false;
                this.Text = "VER PRESUPUESTO";
                this.Cargar_presupuesto(nro);
            }
        }

        private void Cargar_presupuesto(int nro)
        {
            this.oPresupuesto = servicio.ObtenerPresupuestoPorID(nro);
            txtCliente.Text = oPresupuesto.Cliente;
            txtFecha.Text = oPresupuesto.Fecha.ToString("dd/MM/yyyy");
            txtDto.Text = oPresupuesto.Descuento.ToString();
            lblNro.Text = "Presupuesto Nro: " + oPresupuesto.PresupuestoNro.ToString();

            dgvDetalles.Rows.Clear();
            foreach (DetallePresupuesto oDetalle in oPresupuesto.Detalles)
            {
                dgvDetalles.Rows.Add(new object[] { "", oDetalle.Producto.Nombre, oDetalle.Producto.Precio, oDetalle.Cantidad, oDetalle.CalcularSubTotal() }); ;
            }
            CalcularTotales();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos un producto como detalle", "Validaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboProducto.Focus();
                return;
            }

            if (txtCliente.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar un tipo de cliente", "Validaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCliente.Focus();
                return;
            }
            if (txtDto.Text.Trim() == "")
            {
                MessageBox.Show("Debe ingresar el porcetnaje de descuento", "Validaciones", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar el registro?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
        }

        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            if (modo.Equals(Accion.CREATE))
            {
                CargarCombo();
                ConsultarUltimoPresupuesto();
                txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDto.Text = "0";
                txtCliente.Text = "CONSUMIDOR FINAL";
            }
        }

        private void CargarCombo()
        {
            List<Producto> lst = servicio.ConsultarProductos();

            //source es una lista de objetos
            cboProducto.DataSource = lst;
            //valueMember y DisplayMember serán las properties de los objetos
            cboProducto.ValueMember = "IdProducto";
            cboProducto.DisplayMember = "Nombre";
            //Otra opción: cboProducto.Items.AddRange(lst.ToArray()); 

        }

        private void ConsultarUltimoPresupuesto()
        {
            lblNro.Text = "Presupuesto Nro: " + servicio.ObtenerProximoPresupuestoID();
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

        private void btnAgregar_Click_1(object sender, EventArgs e)
        {

        }

        private void dgvDetalles_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
