using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace WorkloadIdentity.Web.Helpers
{
    public static class JwtFormatter
    {
        public static string Prettify(JwtSecurityToken token)
        {

            string jwtResult = "";
            var headers = token.Header;
            var jwtHeader = "{";
            foreach (var h in headers)
            {
                jwtHeader += '"' + h.Key + "\":\"" + h.Value + "\",";
            }
            jwtHeader += "}";
            jwtResult = "Header:\r\n" + JToken.Parse(jwtHeader).ToString(Formatting.Indented);

            //Extract the payload of the JWT
            var claims = token.Claims;
            var jwtPayload = "{";
            foreach (Claim c in claims)
            {
                jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
            }
            jwtPayload += "}";
            jwtResult += "\r\nPayload:\r\n" + JToken.Parse(jwtPayload).ToString(Formatting.Indented);

            return jwtResult;
        }
    }
}
