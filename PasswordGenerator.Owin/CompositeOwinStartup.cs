using System.Reflection;
using Owin;

namespace PasswordGenerator.Owin
{
    public abstract class CompositeOwinStartup
    {
        private readonly object[] startups;

        protected CompositeOwinStartup(params object[] startups)
        {
            this.startups = startups;
        }

        public virtual void Configuration(IAppBuilder app)
        {
            foreach (var startup in startups)
            {
                startup.GetType().GetMethod("Configuration", BindingFlags.Public | BindingFlags.Instance)
                    ?.Invoke(startup, new[] {(object) app});
            }
        }
    }
}