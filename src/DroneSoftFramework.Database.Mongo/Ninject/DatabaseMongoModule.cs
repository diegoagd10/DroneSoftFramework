using System;
using Ninject.Modules;
using DroneSoftFramework.Database.Mongo.Repository;

namespace DroneSoftFramework.Database.Mongo.Ninject
{
    public class DatabaseMongoModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<>))
                .To(typeof(DroneMongoRepository<>));
        }
    }
}
