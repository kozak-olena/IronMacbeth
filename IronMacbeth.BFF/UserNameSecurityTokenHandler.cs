using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Windows.Forms;
using System.Xml;

namespace IronMacbeth.BFF
{
    public class UserNameSecurityTokenHandler : System.IdentityModel.Tokens.UserNameSecurityTokenHandler
    {
        protected virtual bool ValidateUserNameCredentialCore(string userName, string password)
        {
            var result = UserLoginService.VerifyUserCredentials(userName, password);

            return result;
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            if (Configuration == null)
                throw new InvalidOperationException("No Configuration set");

            UserNameSecurityToken nameSecurityToken = token as UserNameSecurityToken;
            if (nameSecurityToken == null)
                throw new ArgumentException("SecurityToken is not a UserNameSecurityToken");
             
                if (!ValidateUserNameCredentialCore(nameSecurityToken.UserName, nameSecurityToken.Password))
                    throw new SecurityTokenValidationException(nameSecurityToken.UserName);
            
            List<Claim> claimList = 
                new List<Claim>()
                {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", nameSecurityToken.UserName),
                    new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"),
                    new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, "yyyy-MM-ddTHH:mm:ss.fffZ"), "http://www.w3.org/2001/XMLSchema#dateTime")
                };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claimList);

            if (Configuration.SaveBootstrapContext)
            {
                claimsIdentity.BootstrapContext = 
                    RetainPassword 
                        ? new BootstrapContext(nameSecurityToken, this)
                        : new BootstrapContext(new UserNameSecurityToken(nameSecurityToken.UserName, null), this);
            }

            return new List<ClaimsIdentity>() { new ClaimsIdentity(claimList, "Password") }.AsReadOnly();
        }

        public override bool CanValidateToken
        {
            get
            {
                return true;
            }
        }
    }
}