using UnityEngine;

namespace Project.Code
{
    [CreateAssetMenu(fileName = "EnergySettings", menuName = "Settings/EnergySettings", order = 0)]
    public class EnergySettings : ScriptableObject
    {
        public int MaxEnergy => _maxEnergy;
        public float RegenSeconds => _regenSeconds;
        public int SpendPerClick => _spendPerClick;

        [SerializeField] private int _maxEnergy;
        [SerializeField] private int _spendPerClick;
        [SerializeField] private float _regenSeconds;
    }
}