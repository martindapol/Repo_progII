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
            foreach (DetallePresupuesto item in Detalles)
            {
                total += item.CalcularSubTotal();
            }
            return total;

        }

        public string GetFechaBajaFormato()
        {
            string aux = FechaBaja.ToString("dd/MM/yyyy");
            return aux.Equals("01/01/0001") ? "" : aux;
        }

    }
}
