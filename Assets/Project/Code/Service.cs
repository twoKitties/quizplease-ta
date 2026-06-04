using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Code
{
    public abstract class Service : IService
    {
        private CancellationTokenSource _cts;

        public virtual async UniTask InitializeAsync(CancellationToken ct)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        }

        public virtual async UniTask ReleaseAsync(CancellationToken ct)
        {
            if (_cts == null)
                return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
}