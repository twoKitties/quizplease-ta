using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Project.Code
{
    public class Bootstrapper : IAsyncStartable, IDisposable
    {
        private readonly IService _service;
        private readonly EnergyBarUIView _view;

        public Bootstrapper(IService service, EnergyBarUIView view)
        {
            _service = service;
            _view = view;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await _service.InitializeAsync(cancellation);
            _view.Initialize();
        }

        public void Dispose()
        {
            _service.ReleaseAsync(CancellationToken.None).Forget();
            _view.Release();
        }
    }
}
