using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Project.Code
{
    public abstract class UIView : MonoBehaviour
    {
        public abstract void Initialize();
        public abstract void Release();
    }

    public class UIView<TVm> : UIView where TVm : IUIViewModel
    {
        protected TVm ViewModel;
        protected List<IDisposable> Disposables;

        [Inject]
        private void Construct(TVm viewModel)
        {
            ViewModel = viewModel;
        }

        public override void Initialize()
        {
            Disposables = new List<IDisposable>();
        }

        public override void Release()
        {
            foreach (var disposable in Disposables)
            {
                disposable.Dispose();
            }
            Disposables.Clear();
            Disposables = null;
        }
    }
}