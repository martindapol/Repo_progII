using PyCarpinteria.acceso_a_datos.Implementaciones;
using PyCarpinteria.acceso_a_datos.Interfaces;
using PyCarpinteria.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyCarpinteria.servicios
{
    class PresupuestoService : IService
    {
        private IPresupuestoDao dao;

        public PresupuestoService()
        {
            dao = new PresupuestoDao();
        }

        public List<Presupuesto> ConsultarPresupuestos(List<Parametro> criterios)
        {
            return dao.GetByFilters(criterios);        }

        public List<Producto> ConsultarProductos()
        {
            return dao.GetProductos();
        }

        public bool GrabarPresupuesto(Presupuesto oPresupuesto)
        {
            return  dao.Save(oPresupuesto);
        }

        public bool RegistrarBajaPresupuesto(int presupuesto)
        {
            return dao.Delete(presupuesto);
        }


        
    }
}
