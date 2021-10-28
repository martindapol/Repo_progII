using CarpinteriaBackend.dominio;
using CarpinteriaBackend.servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpinteriaBackend.acceso_a_datos.Interfaces
{
    interface IPresupuestoDao
    {
        bool Delete(int nro);
        List<Presupuesto> GetByFilters(List<Parametro> criterios);
        List<Producto> GetProductos();
        bool Save(Presupuesto oPresupuesto);
        int GetNextPresupuestoId();

        Presupuesto GetById(int id);

    }
}
