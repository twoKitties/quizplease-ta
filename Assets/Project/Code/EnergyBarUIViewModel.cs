namespace Project.Code
{
    public class EnergyBarUIViewModel : IUIViewModel
    {
        public IReadOnlyValue<float> SecondsToNext { get; }
        public IReadOnlyValue<Progress> Progress => _progress;
        public IReadOnlyValue<bool> IsSpent => _isSpent;

        private readonly ReactiveValue<Progress> _progress;
        private readonly ReactiveValue<bool> _isSpent;
        private readonly IEnergyService _energyService;
        private readonly EnergySettings _settings;

        public EnergyBarUIViewModel(IEnergyService energyService, EnergySettings settings)
        {
            _energyService = energyService;
            _settings = settings;
            SecondsToNext = _energyService.SecondsToNext;

            _progress = new ReactiveValue<Progress>(
                new Progress(_energyService.Current.Value, _energyService.Max.Value));
            _energyService.Current.Subscribe(
                current => _progress.Value = new Progress(current, _energyService.Max.Value),
                invokeImmediately: false);

            _isSpent = new ReactiveValue<bool>(false);
        }

        public void Spend()
        {
            if (_energyService.TrySpend(_settings.SpendPerClick))
            {
                _isSpent.Value = true;
                return;
            }

            _isSpent.Value = false;
        }
    }
}
