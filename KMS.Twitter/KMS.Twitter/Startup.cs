using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KMS.Twitter.Startup))]
namespace KMS.Twitter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
