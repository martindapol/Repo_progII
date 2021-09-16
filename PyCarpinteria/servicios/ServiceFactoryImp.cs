using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyCarpinteria.servicios
{
    class ServiceFactoryImp : AbstractServiceFactory
    {
        public override IService CrearService()
        {
            return new PresupuestoService();
        }
    }
}
