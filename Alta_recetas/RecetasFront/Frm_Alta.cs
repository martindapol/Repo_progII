
using Newtonsoft.Json;
using RecetasBack.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecetasFront
{
    public partial class Frm_Alta : Form
    {
        private Receta receta;
        public Frm_Alta()
        {
            InitializeComponent();
            receta = new Receta();
        }

        private async void btnAceptar_ClickAsync(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(cboProducto.Text))
            {
                MessageBox.Show("Los campos: Nombre y Tipo de receta son obligatorios!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            receta.Nombre = txtNombre.Text;
            receta.TipoDeReceta = cboTipo.SelectedIndex + 1;
            string data = JsonConvert.SerializeObject(receta);

            bool success = await GrabarRecetaAsync(data);
            if (success)
            {
                MessageBox.Show("Receta registrada con éxito!", "Confirmación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await limipiarCamposAsync();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un inconveniente al registrar la receta!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task limipiarCamposAsync()
        {
            txtNombre.Text = string.Empty;
            txtNombre.Focus();
            txtCheff.Text = string.Empty;
            cboTipo.Text = string.Empty;
            dgvDetalles.Rows.Clear();
            lblTotalIng.Text = "Total de ingredientes:";
            await AsignarNumeroRecetaAsync();
        }

        private async Task<bool> GrabarRecetaAsync(string data)
        {
            string url = "https://localhost:44301/api/Recetas";
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                string response = await result.Content.ReadAsStringAsync();
                return response.Equals("Ok");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();

            }
        }

        private async void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            await CargarComboAsync();
            await AsignarNumeroRecetaAsync();
        }

        private async Task AsignarNumeroRecetaAsync()
        {
            string url = "https://localhost:44301/api/Recetas/proximoNro";
            using (HttpClient cliente = new HttpClient())
            {
                var result = await cliente.GetStringAsync(url);
                receta.RecetaNro = Int32.Parse(result);
                lblNro.Text = "Receta #: " + result;
            }

        }

        private async Task CargarComboAsync()
        {
            string url = "https://localhost:44301/api/Recetas/ingredientes";
            using (HttpClient cliente = new HttpClient())
            {
                var result = await cliente.GetAsync(url);
                var bodyJSON = await result.Content.ReadAsStringAsync();
                List<Ingrediente> lst = JsonConvert.DeserializeObject<List<Ingrediente>>(bodyJSON);
                //cargar combo:
                cboProducto.DataSource = lst;
                cboProducto.DisplayMember = "Nombre";
                cboProducto.ValueMember = "IdIngrediente";
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ExisteProductoEnGrilla(cboProducto.Text))
            {
                MessageBox.Show("El ingrediente ya fue ingresado!", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DetalleReceta detalle = new DetalleReceta();
            detalle.Ingrediente = (Ingrediente)cboProducto.SelectedItem;
            detalle.Cantidad = (int)nudCantidad.Value;
            receta.AgregarDetalle(detalle);
            dgvDetalles.Rows.Add(new string[] { "", detalle.Ingrediente.Nombre, detalle.Cantidad.ToString() });
            ActualizarTotales();
        }

        private void ActualizarTotales()
        {
            lblTotalIng.Text = "Total de ingredientes:" + dgvDetalles.Rows.Count;
        }

        private bool ExisteProductoEnGrilla(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["ingrediente"].Value.Equals(text))
                    return true;
            }
            return false;
        }


        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 3)
            {
                receta.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                ActualizarTotales();
            }
        }
    }
}
