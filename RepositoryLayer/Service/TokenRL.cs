using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class TokenRL : ITokenRL
    {
        private IConfiguration _configuration;
        private readonly FundooContext fundooContext;

        public TokenRL(IConfiguration configuration, FundooContext fundooContext)
        {
            _configuration = configuration;
            this.fundooContext = fundooContext;
        }
        public string AuthenticateUser(LoginML login)
        {
            var useremail = fundooContext.Users.FirstOrDefault(x => x.email == login.email);
            //var password = HashingPassword.EncryptPassWord(login.password);
            var password = HashingPassword.DecryptPassWord(useremail.password);
            if (useremail != null && password == login.password)
            {
                
                UserEntity userEntity = fundooContext.Users.FirstOrDefault(x => x.email == login.email);
                var token = GenerateToken(userEntity);
                return token;
            }
            else
            {
                throw new CustomizeException("User Doesnt Exists !! Please Register First");
            }
        }

        public string GenerateToken(UserEntity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.firstName),
                new Claim(ClaimTypes.Email, user.email)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
