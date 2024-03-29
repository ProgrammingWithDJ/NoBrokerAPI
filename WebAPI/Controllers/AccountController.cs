﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Dtos;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUnitOfWork uow;
        private readonly IConfiguration configuration;

        public AccountController(IUnitOfWork uow, IConfiguration configuration)
        {
            this.uow = uow;
            this.configuration = configuration;
        }


        //api/account/login

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginReqDto loginReq)
        {
            var user = await uow.UserRepository.Authenticate(loginReq.UserName, loginReq.Password);
           
            if(user == null)
            {
                return Unauthorized();
            }

            var response = new LoginResponseDto();

            response.UserName = user.UserName;
            response.Token = CreateJWT(user);

            
            return Ok(response);
        }


        public string CreateJWT(User user)
        {
            var secretKey = configuration.GetSection("AppSettings:key").Value;
         
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));

            var claims = new Claim[] {

                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())


            };

            var siginingCredentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescrptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = siginingCredentials

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescrptor);

            return tokenHandler.WriteToken(token);
            
      
        }
    }
}
