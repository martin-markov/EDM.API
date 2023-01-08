using EDM.Data.Models;
using EDM.Data;
using EDM.Models;
using EDM.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace EDM.Services
{
    public class JwtTokenProvider : ITokenProvider
    {
        private readonly IConfiguration config;

        public JwtTokenProvider(IConfiguration config)
        {
            this.config = config;
        }

        public string GetToken(UserDTO user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]!));
            var creadentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.UserData, user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creadentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
