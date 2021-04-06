using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;


[assembly: OwinStartup(typeof(ApplicationServer.Startup))]

namespace ApplicationServer {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.Map("/signalr", map => {
                CorsPolicy corsPolicy = new CorsPolicy {
                    AllowAnyMethod = false,
                    AllowAnyHeader = true,
                    AllowAnyOrigin = false,
                    SupportsCredentials = true,
                };
                corsPolicy.Origins.Add("https://localhost:44305"); //2460
                corsPolicy.Methods.Add("GET");
                //corsPolicy.Methods.Add("POST");

                map.UseCors(new CorsOptions {
                    PolicyProvider = new CorsPolicyProvider {
                        PolicyResolver = context => Task.FromResult(corsPolicy)
                    }
                });

                map.RunSignalR();
            });
        }
    }
}
