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
        public abstract void Configuration(IAppBuilder app);

        protected void Configuration(IAppBuilder app, IKernel kernel)
        {
            HttpConfiguration config = new HttpConfiguration();

            // Map http routes
            config.MapHttpAttributeRoutes();

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
