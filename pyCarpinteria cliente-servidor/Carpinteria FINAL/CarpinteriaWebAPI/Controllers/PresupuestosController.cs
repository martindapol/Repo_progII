using CarpinteriaBackend.servicios;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarpinteriaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresupuestosController : ControllerBase
    {
        private IService service;

        public PresupuestosController()
        {
            service = new ServiceFactoryImp().CrearService();
        }
        
        // GET api/<PresupuestosController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 0)
                return BadRequest("Id es requerido!");
            return Ok(service.ObtenerPresupuestoPorID(id));
        }

        // POST api/<PresupuestosController>
        [HttpPost("consultar")]
        public IActionResult GetPresupuestos(List<Parametro> lst)
        {
            if (lst == null || lst.Count == 0)
                return BadRequest("Se requiere una lista de parámetros!");

            return Ok(service.ConsultarPresupuestos(lst));
        }

        // DELETE api/<PresupuestosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id == 0)
                return BadRequest("Id es requerido!");
            return Ok(service.RegistrarBajaPresupuesto(id));
        }
    }
}
