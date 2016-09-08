using Owin;
using Ninject;
using System.Linq;
using System.Web.Http;
using Ninject.Modules;
using Ninject.Web.Common;
using Microsoft.Owin.Cors;
using System.Net.Http.Formatting;
using DroneSoftFramework.Api.Formatters;
using System.Configuration;
using Ninject.Web.WebApi;

namespace DroneSoftFramework.Api
{
    public abstract class DroneStartup
    {
        protected void Configuration(IAppBuilder app, IKernel kernel)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Getting setups from webconfig
            string apiVersion = ConfigurationManager.AppSettings["ApiVersion"] ?? "v1";

            // Web API Routes
            config.Routes.MapHttpRoute(
                "DefaulController",
                $"{apiVersion}/", new { controller = "Default" });
            
            // Enable CORS
            app.UseCors(CorsOptions.AllowAll);

            // Setting up ninject
            config.DependencyResolver = new NinjectDependencyResolver(kernel);

            // Create jason formatter
            var jsonFormatter = new JsonMediaTypeFormatter();
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(jsonFormatter));

            // Set config to IAppBuilder
            app.UseWebApi(config);
        }

        public IKernel CreateKernel(params INinjectModule[] modules)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Initialize(() => new StandardKernel());
            if (modules.Any())
                bootstrapper.Kernel.Load(modules);
            return bootstrapper.Kernel;
        }
    }
}
