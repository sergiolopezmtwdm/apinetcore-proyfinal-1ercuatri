using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiDigitalGamesMx.Helpers;
using ApiProducts.Library.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiDigitalGamesMx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        readonly IConfiguration _configuration;
        readonly ITokenService _tokenService;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
           // this._tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        [HttpPost]
        public ApiProducts.Library.Models.User Login([FromBody] ApiProducts.Library.Models.UserMin user)
        {

            var ConnectionStringLocal = _configuration.GetValue<string>("CadenaConexion");
            //var ConnectionStringAzure = _configuration.GetValue<string>("ConnectionStringAzure");
            using (ApiProducts.Library.Interfaces.ILogin Login = ApiProducts.Library.Interfaces.Factorizador.CrearConexionServicioLogin(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
            {
                ApiProducts.Library.Models.User objusr = Login.ObtenerLogin(user.Email, Functions.GetSHA256(user.Contrasenia));

                if (objusr.Id > 0)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mtwdm-2020-covid19"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.Role, objusr.Rol)
                            };

                    var tokeOptions = new JwtSecurityToken(
                        issuer: "http://localhost:44300",
                        audience: "http://localhost:44300",
                        claims: claims,//new List<System.Security.Claims.Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signinCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    //var accessToken = tokenService.GenerateAccessToken(claims);
                    var refreshToken = Functions.GenerateRefreshToken();

                    objusr.Token = tokenString;
                    objusr.RefreshToken = refreshToken;
                    objusr.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);

                    using (IUser User = Factorizador.CrearConexionServicioUser(ApiProducts.Library.Models.ConnectionType.MSSQL, ConnectionStringLocal))
                    {
                        User.UpdateRefreshTokenExpiryTime(objusr);
                    }
                    return objusr;
                }

                return null;

            }
        }


    }
}
