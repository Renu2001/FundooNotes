using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class TokenBL : ITokenBL
    {
        private readonly ITokenRL _tokenRL;
        public TokenBL(ITokenRL tokenRL) 
        { 
            _tokenRL = tokenRL;
        }
        public string AuthenticateUser(LoginML login)
        {
            try
            {
                return _tokenRL.AuthenticateUser(login);
            }
            catch (CustomizeException ex)
            {
                throw;
            }
        }

        public string GenerateToken(UserEntity user)
        {
            try
            {
                return _tokenRL.GenerateToken(user);
            }
            catch (CustomizeException ex)
            {
                throw;
            }
        }
    }
}
