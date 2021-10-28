
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarpinteriaFrontend
{
    public partial class Form1 : Form
    {
        public Frm_Principal()
        {
            InitializeComponent();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version Beta de la APP", "Carpinteria");
        }

        private void nuevoPresupuestoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Frm_Alta_Presupuesto frmNuevo = new Frm_Alta_Presupuesto(Accion.CREATE, 0);
            //frmNuevo.ShowDialog();
        }

        private void consultaDePresupuestoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConsultarPresupuestos frmConsulta = new FrmConsultarPresupuestos();
            frmConsulta.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void reporteDeProductosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new FrmReporte().ShowDialog();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("¿Seguro que desea salir de la aplicación?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==  DialogResult.Yes)
            {
                this.Dispose();
            }
           
        }
    }
}
