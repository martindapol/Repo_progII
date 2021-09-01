using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyCarpinteria.dominio
{
    class DetallePresupuesto
    {
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }

        public DetallePresupuesto()
        {
            Producto = new Producto();
            Cantidad = 0;
        }

        public double CalcularSubTotal() {
            return Producto.Precio * Cantidad;
        }
    }
}
