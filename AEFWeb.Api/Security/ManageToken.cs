using AEFWeb.Core.ViewModels;
using AEFWeb.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AEFWeb.Api.Security
{
    public class ManageToken
    {
        public object GetLoginObject(TokenConfigurations tokenConfigurations,
                                        SigningConfigurations signingConfigurations,
                                        UserUpdatePasswordViewModel userViewModel)
        {
            try
            {
                var dates = GetDates(tokenConfigurations);
                userViewModel.Password = null;
                userViewModel.ConfirmPassword = null;
                return new
                {
                    authenticated = true,
                    created = dates.dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dates.dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = GenerateSecurityToken(tokenConfigurations, signingConfigurations, userViewModel, dates),
                    message = "OK",
                    user = userViewModel
                };
            }
            catch
            {
                return null;
            }
        }

        public ClaimsIdentity GenerateClaimsIdentity(UserUpdatePasswordViewModel userViewModel)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userViewModel.Id.ToString(), "Login"),
                new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, userViewModel.Id.ToString())
                }
            );

            return identity;
        }

        public (DateTime dataCriacao, DateTime dataExpiracao) GetDates(TokenConfigurations tokenConfigurations)
        {
            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromSeconds(tokenConfigurations.Seconds);

            return (dataCriacao, dataExpiracao);
        }

        public string GenerateSecurityToken(TokenConfigurations tokenConfigurations,
                                            SigningConfigurations signingConfigurations,
                                            UserUpdatePasswordViewModel userViewModel,
                                            (DateTime dataCriacao, DateTime dataExpiracao) dates)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = tokenConfigurations.Issuer,
                Audience = tokenConfigurations.Audience,
                SigningCredentials = signingConfigurations.SigningCredentials,
                Subject = GenerateClaimsIdentity(userViewModel),
                NotBefore = dates.dataCriacao,
                Expires = dates.dataExpiracao
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        public static Guid GetToken(HttpRequest request)
        {
            var token = request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "")?.Trim();

            if (string.IsNullOrWhiteSpace(token)) return Guid.Empty;

            var handler = new JwtSecurityTokenHandler();

            var tokenS = handler.ReadToken(token) as JwtSecurityToken;

            return Guid.Parse(tokenS.Claims.FirstOrDefault(claim => claim.Type == "unique_name").Value);
        }
    }
}
