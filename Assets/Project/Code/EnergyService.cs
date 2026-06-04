using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.Code
{
    public class EnergyService : Service, IEnergyService
    {
        public IReadOnlyValue<int> Current => _current;
        public IReadOnlyValue<int> Max => _max;
        public IReadOnlyValue<float> SecondsToNext => _secondsToNext;

        private readonly ReactiveValue<int> _current;
        private readonly ReactiveValue<int> _max;
        private readonly ReactiveValue<float> _secondsToNext;
        private readonly EnergySettings _settings;

        private UniTask _regenTask;
        private float _accumulatedStartTime;
        private static float Now => Time.time;

        public EnergyService(EnergySettings settings)
        {
            _settings = settings;
            _current = new ReactiveValue<int>(_settings.MaxEnergy);
            _max = new ReactiveValue<int>(_settings.MaxEnergy);
            _secondsToNext = new ReactiveValue<float>();
        }

        protected override UniTask OnInitializeAsync(CancellationToken ct)
        {
            _accumulatedStartTime = Now;
            UpdateProgress();
            _regenTask = RegenLoop(ct);
            return UniTask.CompletedTask;
        }

        protected override async UniTask OnReleaseAsync(CancellationToken ct)
        {
            await _regenTask.AttachExternalCancellation(ct).SuppressCancellationThrow();
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0)
                return false;
            if (_current.Value < amount)
                return false;

            var wasFull = _current.Value >= _settings.MaxEnergy;
            _current.Value -= amount;

            if (wasFull)
            {
                _accumulatedStartTime = Now;
                UpdateProgress();
            }
            return true;
        }

        private async UniTask RegenLoop(CancellationToken ct)
        {
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(0.1), DelayType.DeltaTime, PlayerLoopTiming.Update, ct);
                    TickRegen();
                }
            }
            catch (OperationCanceledException) { }
        }

        private void TickRegen()
        {
            if (_current.Value >= _settings.MaxEnergy)
            {
                _accumulatedStartTime = Now;
                SetProgress(1f);
                return;
            }

            var elapsed = Now - _accumulatedStartTime;
            if (elapsed >= _settings.RegenSeconds)
            {
                int gained = (int)(elapsed / _settings.RegenSeconds);
                int space = _settings.MaxEnergy - _current.Value;
                int applied = Mathf.Min(gained, space);

                _current.Value += applied;
                _accumulatedStartTime += applied * _settings.RegenSeconds;

                if (_current.Value >= _settings.MaxEnergy)
                    _accumulatedStartTime = Now;
            }

            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (_current.Value >= _settings.MaxEnergy)
            {
                SetProgress(1f);
                return;
            }
            var elapsed = Now - _accumulatedStartTime;
            SetProgress(Math.Clamp(elapsed / _settings.RegenSeconds, 0f, 1f));
        }

        private void SetProgress(float value)
        {
            _secondsToNext.Value = value;
        }
    }
}
