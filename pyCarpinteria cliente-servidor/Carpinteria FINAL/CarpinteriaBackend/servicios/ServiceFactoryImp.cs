using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaBackend.servicios
{
    public class ServiceFactoryImp : AbstractServiceFactory
    {
        public override IService CrearService()
        {
            return new PresupuestoService();
        }
    }
}
