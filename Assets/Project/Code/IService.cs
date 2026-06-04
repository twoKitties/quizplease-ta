using System.Threading;
using Cysharp.Threading.Tasks;

namespace Project.Code
{
    public interface IService
    {
        UniTask InitializeAsync(CancellationToken ct);
        UniTask ReleaseAsync(CancellationToken ct);
    }
}