using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVentas.Models.Request;
using WSVentas.Models.Response;
using WSVentas.Services;

namespace WSVentas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Autentificar([FromBody] AuthRequest model)
        {
            Respuesta oRespuesta = new Respuesta();
            var userresponse = _userService.Auth(model);

            if (userresponse == null)
            {
                oRespuesta.Mensaje = "Usuario o contraseña incorrecta";
                oRespuesta.Exito = 0;
                return BadRequest();
            }

            oRespuesta.Exito = 1;
            oRespuesta.Data = userresponse;
            return Ok(oRespuesta);
        }
    }
}
