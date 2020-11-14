using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Xml;

namespace IronMacbeth.BFF
{
    public class UserNameSecurityTokenHandler : System.IdentityModel.Tokens.UserNameSecurityTokenHandler
    {
        public static string Generated = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public static Claim Now
        {
            get
            {
                return new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationinstant", XmlConvert.ToString(DateTime.UtcNow, Generated), "http://www.w3.org/2001/XMLSchema#dateTime");
            }
        }

        protected virtual bool ValidateUserNameCredentialCore(string userName, string password)
        {
            return true;
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(
          SecurityToken token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));
            if (this.Configuration == null)
                throw new InvalidOperationException("No Configuration set");
            UserNameSecurityToken nameSecurityToken = token as UserNameSecurityToken;
            if (nameSecurityToken == null)
                throw new ArgumentException("SecurityToken is not a UserNameSecurityToken");
            if (!this.ValidateUserNameCredentialCore(nameSecurityToken.UserName, nameSecurityToken.Password))
                throw new SecurityTokenValidationException(nameSecurityToken.UserName);
            List<Claim> claimList = new List<Claim>()
              {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", nameSecurityToken.UserName),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2008/06/identity/authenticationmethod/password"),
                Now
              };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity((IEnumerable<Claim>)claimList);
            if (this.Configuration.SaveBootstrapContext)
                claimsIdentity.BootstrapContext = !this.RetainPassword ? (object)new BootstrapContext((SecurityToken)new UserNameSecurityToken(nameSecurityToken.UserName, (string)null), (SecurityTokenHandler)this) : (object)new BootstrapContext((SecurityToken)nameSecurityToken, (SecurityTokenHandler)this);
            return new List<ClaimsIdentity>()
      {
        new ClaimsIdentity((IEnumerable<Claim>) claimList, "Password")
      }.AsReadOnly();
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