using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Code
{
    public abstract class Service : IService
    {
        private CancellationTokenSource _cts;

        public UniTask InitializeAsync(CancellationToken ct)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            return OnInitializeAsync(_cts.Token);
        }

        public async UniTask ReleaseAsync(CancellationToken ct)
        {
            if (_cts == null)
                return;

            try
            {
                _cts.Cancel();
                await OnReleaseAsync(ct);
            }
            finally
            {
                _cts.Dispose();
                _cts = null;
            }
        }

        protected virtual UniTask OnInitializeAsync(CancellationToken ct) => UniTask.CompletedTask;

        protected virtual UniTask OnReleaseAsync(CancellationToken ct) => UniTask.CompletedTask;
    }
}