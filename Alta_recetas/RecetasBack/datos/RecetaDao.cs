using RecetasBack.dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasBack.datos
{
    class RecetaDao : IRecetaDao
    {
        public List<Ingrediente> GetIngredientes()
        {
            List<Ingrediente> lst = new List<Ingrediente>();

            DataTable t = HelperDao.GetInstance().ConsultaSQL("SP_CONSULTAR_INGREDIENTES");
            foreach (DataRow row in t.Rows)
            {
                Ingrediente oIngrediente = new Ingrediente();
                oIngrediente.IdIngrediente = Convert.ToInt32(row[0].ToString());
                oIngrediente.Nombre = row[1].ToString();
                oIngrediente.Unidad = row[2].ToString();

                lst.Add(oIngrediente);
            }

            return lst;
        }

        public int ProximoNroReceta()
        {
            return HelperDao.GetInstance().EjecutarSQLConValorOUT("SP_PROXIMO_ID", "@next");
        }

        public bool Save(Receta oReceta)
        {
            return HelperDao.GetInstance().EjecutarInsert(oReceta, "SP_INSERTAR_RECETA", "SP_INSERTAR_DETALLES");
        }
    }
}
