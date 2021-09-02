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
    public partial class FrmReporte : Form
    {
        public FrmReporte()
        {
            InitializeComponent();
        }

        private void FrmReporte_Load(object sender, EventArgs e)
        {
            //Validar los filtros fecha desde <= fecha hasta
            SqlConnection cnn = new SqlConnection(@"Data Source =.\SQLEXPRESS; Initial Catalog = db_carpinteria; Integrated Security = True");

            DataTable table = new DataTable();
            cnn.Open();

            SqlCommand cmd = new SqlCommand("SP_REPORTE_PRODUCTOS", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            table.Load(cmd.ExecuteReader());
            cnn.Close();


            rvReporte.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", table));


            this.rvReporte.RefreshReport();
        }
    }
}
