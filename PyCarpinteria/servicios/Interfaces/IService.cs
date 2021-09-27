using PyCarpinteria.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyCarpinteria.servicios
{
    interface IService
    {
        bool RegistrarBajaPresupuesto(int presupuesto);
        List<Presupuesto> ConsultarPresupuestos(List<Parametro> criterios);
        List<Producto> ConsultarProductos();
        bool GrabarPresupuesto(Presupuesto oPresupuesto);

        int ObtenerProximoPresupuestoID();
        Presupuesto ObtenerPresupuestoPorID(int nro);

    }
}
