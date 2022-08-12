using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Entites;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config) => _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        public string CreateToken(AppUser user)
        {
            var claim = new List<Claim>{
            new  Claim(JwtRegisteredClaimNames.NameId,user.Id.ToString()),
            new  Claim(JwtRegisteredClaimNames.UniqueName,user.UserName),
           };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };
            var tokenHander = new JwtSecurityTokenHandler();
            var token = tokenHander.CreateToken(tokenDescriptor);
            return tokenHander.WriteToken(token);
        }
    }
}