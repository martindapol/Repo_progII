using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.dominio
{
    public class Receta
    {
        public int RecetaNro { get; set; }
        public string Nombre { get; set; }
        public int TipoDeReceta { get; set; }
        public string Cheff { get; set; }

        public List<DetalleReceta> Detalles { get; set; }

        public Receta()
        {
            Detalles = new List<DetalleReceta>();
        }

        public void AgregarDetalle(DetalleReceta detalle)
        {
            Detalles.Add(detalle);
        }


        public void QuitarDetalle(int nro)
        {
            Detalles.RemoveAt(nro);
        }

    }
}
