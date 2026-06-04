namespace Project.Code
{
    public interface IEnergyService
    {
        IReadOnlyValue<int> Current { get; }
        IReadOnlyValue<int> Max { get; }
        IReadOnlyValue<float> SecondsToNext { get; }
        bool TrySpend(int amount);
    }
}