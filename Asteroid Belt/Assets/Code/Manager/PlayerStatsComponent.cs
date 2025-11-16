using UnityEngine;

public class PlayerStatsComponent : MonoBehaviour
{
    private PlayerStats _currentStats;
    public PlayerStats CurrentStats => _currentStats; // public getter

    public void InitFromBaseStats(PlayerStats baseStats)
    {
        _currentStats = baseStats;   // value-type copy
    }

    public void TakeDamage(int amount)
    {
        _currentStats.maxHealth -= amount;

        if (_currentStats.maxHealth <= 0)
        {
            // do death logic
            return;
        }
    }
}
