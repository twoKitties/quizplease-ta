using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Project.Code
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private EnergySettings _energySettings;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_energySettings);
            builder.Register<EnergyService>(Lifetime.Singleton)
                .As<IEnergyService>()
                .As<IService>();
            builder.Register<EnergyBarUIViewModel>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<EnergyBarUIView>();
            builder.RegisterEntryPoint<Bootstrapper>();
        }
    }
}
