using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Code
{
    public class EnergyBarUIView : UIView<EnergyBarUIViewModel>
    {
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Image _progressBar;
        [SerializeField] private Button _spendButton;
        [SerializeField] private Color _spendFlashColor = Color.green;
        [SerializeField] private float _flashDuration = 0.15f;
        [SerializeField] private float _fadeDuration = 0.4f;

        private Color _defaultBarColor;
        private Coroutine _signalCoroutine;
        
        public override void Initialize()
        {
            base.Initialize();
            _defaultBarColor = _progressBar.color;
            var barDisposable = ViewModel.SecondsToNext.Subscribe(RefreshProgressBar);
            Disposables.Add(barDisposable);
            var textDisposable = ViewModel.Progress.Subscribe(RefreshProgressText);
            Disposables.Add(textDisposable);
            var spendDisposable = ViewModel.IsSpent.Subscribe(Signal);
            Disposables.Add(spendDisposable);
            _spendButton.onClick.AddListener(SpendEnergy);
        }

        public override void Release()
        {
            base.Release();
            _spendButton.onClick.RemoveListener(SpendEnergy);
        }

        private void RefreshProgressBar(float secondsToNext)
        {
            _progressBar.fillAmount = secondsToNext;
        }

        private void RefreshProgressText(Progress progress)
        {
            _progressText.text = $"{progress.Current} / {progress.Max}";
        }

        private void SpendEnergy()
        {
            ViewModel.Spend();
        }

        private void Signal(bool isSpent)
        {
            if (!isSpent)
                return;

            if (_signalCoroutine != null)
                StopCoroutine(_signalCoroutine);

            _signalCoroutine = StartCoroutine(SpendSignalCoroutine());
        }

        private IEnumerator SpendSignalCoroutine()
        {
            _progressBar.color = _spendFlashColor;
            yield return new WaitForSeconds(_flashDuration);

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                _progressBar.color = Color.Lerp(_spendFlashColor, _defaultBarColor, elapsed / _fadeDuration);
                yield return null;
            }

            _progressBar.color = _defaultBarColor;
            _signalCoroutine = null;
        }
    }
}