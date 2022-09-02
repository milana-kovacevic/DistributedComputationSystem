using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration
{
    public static class TestConstants
    {
        public static readonly string ClientId = "51efcfc4-6aeb-4500-9440-15536a1acb97";
        public static readonly string Secret = "<placeholder>";
        public static readonly string TenantId = "059292d2-e768-40fb-a0a6-ab1e4639ea8c";
        public static readonly string TargetClientId = "b09bd1fa-9254-4d4c-94dc-ad823bb48b9e";
        public static readonly string TargetAudience = "api://frontend-webapi";
        public static readonly string Issuer = "https://sts.windows.net/059292d2-e768-40fb-a0a6-ab1e4639ea8c/v2.0";
        public static readonly string AuthServer = "https://login.microsoftonline.com/";
    }
}
