using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly FundooContext fundooContext;

        public UserRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }


        public UserEntity RegisterUser(UserModel user)
        {
            UserEntity userEntity = new UserEntity();
            var result = fundooContext.Users.FirstOrDefault(x => x.email == user.email);
            if (result == null)
            {
                userEntity.firstName = user.firstName;
                userEntity.lastName = user.lastName;
                userEntity.email = user.email;
                userEntity.password = user.password;
                fundooContext.Users.Add(userEntity);
                fundooContext.SaveChanges();
                return userEntity;

            }
            else
            {
                throw new CustomizeException("User Already Exists !!");
            }

            
        }

        public UserEntity Login(LoginML login)
        {
            
            var useremail = fundooContext.Users.FirstOrDefault(x => x.email == login.email);
            var userpassword = fundooContext.Users.FirstOrDefault(y => y.password == login.password);
            if (useremail != null && userpassword != null)
            {
                UserEntity userEntity = fundooContext.Users.FirstOrDefault(x => x.email == login.email);
                return userEntity;
            }
            else
            {
                throw new CustomizeException("User Doesnt Exists !! Please Register First");
            }
        }

    }
}
