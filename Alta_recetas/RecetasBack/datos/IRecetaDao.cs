using RecetasBack.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.datos
{
    interface IRecetaDao
    {
        List<Ingrediente> GetIngredientes();
        bool Save(Receta oReceta);

        int ProximoNroReceta();
    }
}
