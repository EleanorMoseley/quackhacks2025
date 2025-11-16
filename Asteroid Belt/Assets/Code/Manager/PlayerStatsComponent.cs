using UnityEngine;

public class PlayerStatsComponent : MonoBehaviour
{
    private PlayerStats _currentStats;
    public PlayerStats CurrentStats => _currentStats; // public getter
    public bool tookDamage;

    public void InitFromBaseStats(PlayerStats baseStats)
    {
        _currentStats = baseStats;   // value-type copy
    }

    public void TakeDamage(int amount)
    {
        _currentStats.maxHealth -= amount;
        tookDamage = true;

        if (_currentStats.maxHealth <= 0)
        {
            // do death logic
            return;
        }
    }
}
