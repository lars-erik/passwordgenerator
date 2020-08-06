using Owin;
using Umbraco.Core.Composing;
using Umbraco.Core.Security;

namespace PasswordGenerator.Owin
{
    public class DefaultExtraSecureOwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.ConfigureExtraSecureUserManagerForUmbracoBackOffice(
                Current.Services,
                MembershipProviderExtensions.GetUsersMembershipProvider().AsUmbracoMembershipProvider());
        }
    }
}