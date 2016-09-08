using Ninject.Modules;
using DroneSoftFramework.Database.Relational.Repository;

namespace DroneSoftFramework.Database.Relational.Ninject
{
    public class DatabaseRelationalModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IRepository<,>))
                .To(typeof(DroneRepository<,>));
        }
    }
}
