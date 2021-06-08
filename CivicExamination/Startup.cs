using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CivicExamination.Startup))]
namespace CivicExamination
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
