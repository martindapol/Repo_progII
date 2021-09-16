using PyCarpinteria.acceso_a_datos.Implementaciones;
using PyCarpinteria.acceso_a_datos.Interfaces;
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

        public bool RegistrarBajaPresupuesto(int presupuesto)
        {
            return dao.Delete(presupuesto);
        }
    }
}
