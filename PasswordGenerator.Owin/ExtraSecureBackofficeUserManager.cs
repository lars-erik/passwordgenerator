using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Configuration.UmbracoSettings;
using Umbraco.Core.Models.Identity;
using Umbraco.Core.Security;
using Umbraco.Core.Services;
using Umbraco.Web.Security;

namespace PasswordGenerator.Owin
{
    public class ExtraSecureBackofficeUserManager : BackOfficeUserManager
    {
        private static Generator generator;

        public static BackOfficeUserManager Create(
            IdentityFactoryOptions<BackOfficeUserManager> options,
            ServiceContext serviceContext,
            MembershipProviderBase membershipProvider)
        {
            if (options == null) throw new ArgumentNullException("options");
            if (serviceContext == null) throw new ArgumentNullException("serviceContext");

            var manager = new ExtraSecureBackofficeUserManager(new BackOfficeUserStore(
                serviceContext.UserService, 
                serviceContext.MemberTypeService, 
                serviceContext.EntityService,
                serviceContext.ExternalLoginService,
                Current.Configs.Global(),
                membershipProvider,
                Current.Mapper
            ));
            // TODO: Finn alle dependencies
            manager.InitUserManager(manager, membershipProvider, Current.Configs.GetConfig<IUmbracoSettingsSection>().Content, options);

            var streamSource = (IStreamSource)Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["PasswordGenerator.StreamSource"]));

            generator = new Generator(streamSource, new GeneratorConfig(
                ConfigurationManager.AppSettings["PasswordGenerator.SeedFormula"],
                ConfigurationManager.AppSettings["PasswordGenerator.Suffix"],
                ConfigurationManager.AppSettings["PasswordGenerator.SuffixFormula"]
            ));

            return manager;
        }

        public ExtraSecureBackofficeUserManager(IUserStore<BackOfficeIdentityUser, int> store) : base(store)
        {
        }

        public ExtraSecureBackofficeUserManager(IUserStore<BackOfficeIdentityUser, int> store, IdentityFactoryOptions<BackOfficeUserManager> options, MembershipProviderBase membershipProvider, IContentSection contentSectionConfig) : base(store, options, membershipProvider, contentSectionConfig)
        {
        }

        public override Task<bool> CheckPasswordAsync(BackOfficeIdentityUser user, string password)
        {
            var accounts = (ConfigurationManager.AppSettings["PasswordGenerator.ExtraSecureUsers"] ?? "")
                .Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (accounts.Contains(user.UserName))
            {
                var todaysPwd = generator.Generate(DateTime.Today, ConfigurationManager.AppSettings["PasswordGenerator.Salt"]);
                if (todaysPwd == password)
                {
                    return Task.FromResult(true);
                }
            }

            return base.CheckPasswordAsync(user, password);
        }
    }
}