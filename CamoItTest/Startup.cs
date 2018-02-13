using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CamoItTest.Startup))]

namespace CamoItTest {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.MapSignalR(new HubConfiguration() {

            });
        }
    }
}