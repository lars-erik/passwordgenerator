using Microsoft.Owin;
using Owin;
using PasswordGenerator.Owin;
using Umbraco.Web;

[assembly: OwinStartup("ExtraSecureOwinStartup", typeof(OwinStartup))]
namespace PasswordGenerator.Owin
{
    public class OwinStartup : CompositeOwinStartup
    {
        public OwinStartup()
            : base(new UmbracoDefaultOwinStartup(), new DefaultExtraSecureOwinStartup())
        {
        }

        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);
        }
    }
}