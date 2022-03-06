using DitenProductAPI.Interfaces;
using DitenProductAPI.MyEntities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DitenProductAPI.Managers
{
    public class JWTAuthenticateManager : IJWTAuthenticateManager
    {
        private readonly string key;
        public JWTAuthenticateManager(string key)
        {
            this.key = key;
        }
        

        public string createToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(token), SecurityAlgorithms.HmacSha256Signature)
            };
            var token1 = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token1);
        }
    }
}
