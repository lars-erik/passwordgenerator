using System;
using Microsoft.Owin.Logging;
using Owin;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web.Security;

namespace PasswordGenerator.Owin
{
    public static class AppBuilderExtensions
    {
        public static void ConfigureExtraSecureUserManagerForUmbracoBackOffice(
            this IAppBuilder app,
            ServiceContext serviceContext,
            MembershipProviderBase userMembershipProvider
        )
        {
            if (serviceContext == null) throw new ArgumentNullException("serviceContext");
            if (userMembershipProvider == null) throw new ArgumentNullException("userMembershipProvider");

            app.CreatePerOwinContext<BackOfficeUserManager>(
                (options, owinContext) => ExtraSecureBackofficeUserManager.Create(
                    options,
                    serviceContext,
                    MembershipProviderExtensions.GetUsersMembershipProvider().AsUmbracoMembershipProvider()));

            app.CreatePerOwinContext<BackOfficeSignInManager>((options, context) => BackOfficeSignInManager.Create(options, context, Current.Configs.Global(), app.CreateLogger<BackOfficeSignInManager>()));
        }

    }
}