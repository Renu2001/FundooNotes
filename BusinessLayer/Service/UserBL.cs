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
    public class UserBL : IUserBL
    {
        private readonly IUserRL _userRL;

        public UserBL(IUserRL userRL)
        {
            this._userRL = userRL;
        }

        public UserEntity RegisterUser(UserModel model)
        {
            try
            {
                return _userRL.RegisterUser(model);
            }
            catch
            {
                throw;
            }
            
        }

        
        public IEnumerable<UserEntity> GetUsers()
        {
            try
            {
                return _userRL.GetUsers();
            }
            catch 
            {
                throw;
            }
        }

    }
}
