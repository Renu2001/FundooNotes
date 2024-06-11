using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface ITokenBL
    {
        public string AuthenticateUser(LoginML login);
        public string GenerateToken(UserEntity user);
    }
}
