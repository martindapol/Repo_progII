using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecetasBack.dominio;
using RecetasBack.negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecetasWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecetasController : ControllerBase
    {
        private IAplicacion app;

        public RecetasController()
        {
            app = new Aplicacion();
        }
        
        [HttpGet("ingredientes")]
        public IActionResult GetIngredientes()
        {
            return Ok(app.ConsultarIngredientes());
        }

        [HttpGet("proximoNro")]
        public string GetProximoNroReceta()
        {
            return app.ObtenerProximoNroReceta().ToString();
        }

        

        [HttpPost]
        public IActionResult PostReceta(Receta oReceta)
        {
            if(oReceta == null)
            {
                return BadRequest();
            }

            if(app.CrearReceta(oReceta))
                return Ok("Ok");
            else
                return Ok("No se pudo grabar la receta!");
        }

    }
}
