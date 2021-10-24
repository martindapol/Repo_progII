using RecetasBack.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.negocio
{
    public interface IAplicacion
    {
        public List<Ingrediente> ConsultarIngredientes();

        public bool CrearReceta(Receta oReceta);

        public int ObtenerProximoNroReceta();

    }
}
