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
using CarpinteriaBackend.servicios;
using CarpinteriaBackend.dominio;

namespace CarpinteriaFrontend.presentacion
{

    public partial class FrmConsultarPresupuestos : Form
    {
        private IService servicio;

        public FrmConsultarPresupuestos()
        {
            InitializeComponent();
            servicio = new ServiceFactoryImp().CrearService();
        }

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            List<Parametro> filtros = new List<Parametro>();
            Parametro fecha_desde = new Parametro();
            fecha_desde.Nombre = "@fecha_desde";
            fecha_desde.Valor = dtpDesde.Value;
            filtros.Add(fecha_desde);
            filtros.Add(new Parametro("@fecha_hasta", dtpHasta.Value));

            object val = DBNull.Value;
            if (!String.IsNullOrEmpty(txtCliente.Text))
                val = txtCliente.Text;
            filtros.Add(new Parametro("@cliente", val));

            string conBaja = "N";
            if (chkBaja.Checked)
                conBaja = "S";
            filtros.Add(new Parametro("@datos_baja", conBaja));
            List<Presupuesto> lst = servicio.ConsultarPresupuestos(filtros);

            dgvResultados.Rows.Clear();
            foreach (Presupuesto oPresupuesto in lst)
            {
                dgvResultados.Rows.Add(new object[]{
                                        oPresupuesto.PresupuestoNro,
                                        oPresupuesto.Fecha.ToString("dd/MM/yyyy"),
                                        oPresupuesto.Cliente,
                                        oPresupuesto.Descuento,
                                        oPresupuesto.Total,
                                        oPresupuesto.GetFechaBajaFormato()
                 }); ;
            }




        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvResultados.CurrentRow; // fila actual o seleccionada
            if (row != null)
            {
                int presupuesto = Int32.Parse(row.Cells["colNro"].Value.ToString());
                if (MessageBox.Show("Seguro que desea eliminar el presupuesto seleccionado?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool respuesta = servicio.RegistrarBajaPresupuesto(presupuesto);

                    if (respuesta)
                    {
                        MessageBox.Show("Presupuesto eliminado!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.btnConsultar_Click(null, null);
                    }
                    else
                        MessageBox.Show("Error al intentar borrar el presupuesto!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FrmConsultarPresupuestos_Load(object sender, EventArgs e)
        {

        }

        private void dgvResultados_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Si el índice de la columna de la fila actual es 6: (botón Ver detalles)
            if(dgvResultados.CurrentCell.ColumnIndex == 6)
            {
                int nroPresupuesto = Convert.ToInt32(dgvResultados.CurrentRow.Cells["colNro"].Value.ToString());
                Frm_Alta_Presupuesto frm = new Frm_Alta_Presupuesto(Accion.READ, nroPresupuesto);
                frm.ShowDialog();
            }
        }
    }
}

