using RecetasBack.datos;
using RecetasBack.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.negocio
{
    public class Aplicacion : IAplicacion
    {
        private IRecetaDao dao;

        public Aplicacion()
        {
            dao = new RecetaDao();
        }


        public List<Ingrediente> ConsultarIngredientes()
        {
            return dao.GetIngredientes();
        }

        public bool CrearReceta(Receta oReceta)
        {
            return dao.Save(oReceta);
        }

        public int ObtenerProximoNroReceta()
        {
            return dao.ProximoNroReceta();
        }
    }
}
