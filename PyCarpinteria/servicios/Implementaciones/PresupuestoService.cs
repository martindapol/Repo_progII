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
            dao = new PresupuestoDao(); //acá también podríamos tener una fábrica para crear el DAO.
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

        public Presupuesto ObtenerPresupuestoPorID(int nro)
        {
            return dao.GetById(nro);
        }

        public int ObtenerProximoPresupuestoID()
        {
            return dao.GetNextPresupuestoId();
        }

        public bool RegistrarBajaPresupuesto(int presupuesto)
        {
            return dao.Delete(presupuesto);
        }


        
    }
}
