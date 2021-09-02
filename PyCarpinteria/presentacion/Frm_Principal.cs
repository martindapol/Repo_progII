
using PyCarpinteria.presentacion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PyCarpinteria
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Version Beta de la APP", "Carpinteria");
        }

        private void nuevoPresupuestoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_Alta_Presupuesto frmNuevo = new Frm_Alta_Presupuesto();
            frmNuevo.ShowDialog();
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
            new FrmReporte().ShowDialog();
        }
    }
}
