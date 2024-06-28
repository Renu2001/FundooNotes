using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotes.Controllers
{
    [Route("api/Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;
        private readonly Token _token;
        private readonly Email _email;

        public ResponseModel mod;

        public UserController(IUserBL userBL, Token token, Email email)
        {
            this.userBL = userBL;
            _token = token;
            _email = email;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(UserModel model)
        {
            
            try
            {
                var result = userBL.RegisterUser(model);
                if (result != null)
                {
                    mod = new ResponseModel()
                    {
                        Success = "true",
                        Message = "User Added Successfully",
                        Data = result
                    };

                    HttpContext.Session.SetString("UserId", result.firstName.ToString());
                    HttpContext.Session.SetString("UserEmail", result.email);
                }

            }
            catch(CustomizeException ex) 
            {
                mod = new ResponseModel()
                {
                    Success = "false",
                    Message = ex.Message
                };
                return StatusCode(400, mod);
            }

            return StatusCode(200, mod);
        }

      
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginML login)
        { 
            try
            {
                var token = _token.AuthenticateUser(login);
                if (token != null)
                {
                   
                    mod = new ResponseModel()
                        {
                            Success = "true",
                            Message = "Login Successfully",
                            Data = token
                    };
                }
            }
            catch (CustomizeException ex)
            {
                mod = new ResponseModel()
                {
                    Success = "false",
                    Message = ex.Message
                };
                return StatusCode(400, mod);
            }
            return StatusCode(200, mod);

        }

       
        [HttpPost]
        [Route("ResetPassword/{token}")]
        public IActionResult ResetPassword(string token, [FromBody] string newPassword)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;
            Console.WriteLine(emailClaim);
            try
            {
                userBL.ResetPassword(emailClaim, newPassword); ;
                mod = new ResponseModel()
                {
                    Success = "true",
                    Message = "Password Updated Successfully",

                };
            }
            catch (CustomizeException ex)
            {
                mod = new ResponseModel()
                {
                    Success = "false",
                    Message = ex.Message
                };
                return StatusCode(400, mod);
            }
            
            return StatusCode(200, mod);

        }

        [HttpPost("Forgetpassword")]
        public IActionResult ForgetPassword(string emailId)
        {
            try
            {
                var result = _email.SendResetMail(emailId);

                mod = new ResponseModel()
                {
                    Success = "true",
                    Message = "Email Sent Successfully",
                    Data = result
                };

            }
            catch (CustomizeException ex)
            {
                mod = new ResponseModel()
                {
                    Success = "false",
                    Message = ex.Message
                };
                return StatusCode(404, mod);

            }
            return StatusCode(200, mod);

        }
        [HttpGet]
        [Route("GetUserSession")]
        public IActionResult GetUserSession()
        {
            // Retrieve session values
            var userId = HttpContext.Session.GetString("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userEmail))
            {
                return StatusCode(404, new ResponseModel
                {
                    Success = "false",
                    Message = "Session data not found"
                });
            }

            var sessionData = new
            {
                UserId = userId,
                UserEmail = userEmail
            };

            return StatusCode(200, new ResponseModel
            {
                Success = "true",
                Message = "Session data retrieved successfully",
                Data = sessionData
            });
        }

    }


}
