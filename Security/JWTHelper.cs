using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GloEpidBot.Model.Domain;
using Microsoft.IdentityModel.Tokens;

namespace GloEpidBot.Security
{
    public static class JWTHelper
    {
        public static TokenReturnHelper GetJWTToken(string UserMail, string UserId,  TokenOptions option)
        {
            var claims = new List<Claim>()
           {
               new Claim(JwtRegisteredClaimNames.Sub, UserMail),
               new Claim("UserId", UserId),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim("Email",UserMail)
           };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(option.accessExpiration));

            var token = new JwtSecurityToken(option.Issuer, option.Audience, claims, expires: expires, signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenReturnHelper
            {
                ExpiresIn = expires,
                Token = jwt
            };


        }

        public class TokenReturnHelper
        {
            public string Token { get; set; }
            public DateTime ExpiresIn { get; set; }
        }
    }
}

