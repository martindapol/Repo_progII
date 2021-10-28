using CarpinteriaBackend.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaBackend.servicios
{
   public interface IService
    {
       public bool RegistrarBajaPresupuesto(int presupuesto);
        public List<Presupuesto> ConsultarPresupuestos(List<Parametro> criterios);
        public List<Producto> ConsultarProductos();
        public bool GrabarPresupuesto(Presupuesto oPresupuesto);

        public int ObtenerProximoPresupuestoID();
        public Presupuesto ObtenerPresupuestoPorID(int nro);

    }
}
