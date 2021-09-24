using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WSVentas.Models;
using WSVentas.Models.Common;
using WSVentas.Models.Request;
using WSVentas.Models.Response;
using WSVentas.Tools;

namespace WSVentas.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userResponse = new UserResponse();
            using (var db = new VentaRealContext())
            {
                string password = Encrypt.GetSHA256(model.Password);
                var usuario = db.Usuarios.Where(x => x.Email.Equals(model.Email) && x.Password.Equals(password)).FirstOrDefault();

                if (usuario == null) return null;
                userResponse.Email = usuario.Email;
                userResponse.Token = GetToken(usuario);
            }
            return userResponse;
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier,usuario.Id.ToString()),
                            new Claim(ClaimTypes.Email,usuario.Email)
                        }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave),
                SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
