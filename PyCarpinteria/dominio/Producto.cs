using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyCarpinteria.dominio
{
    class Producto : Object
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public bool Activo { get; set; }


        public Producto()
        {
                
        }
        
        public override string ToString()
        {

            return IdProducto.ToString() + "-" + Nombre;

        }
    }
}
