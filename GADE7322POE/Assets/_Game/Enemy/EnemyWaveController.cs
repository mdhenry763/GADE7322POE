using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveController : MonoBehaviour
{
    public int goblinChance = 100;
    public int dragonChance = 0;
    public int golemChance = 0;

    private int _mWaveNumber;

    private void Start()
    {
        _mWaveNumber = 0;
    }

    public EEnemy GetRandomEnemy()
    {
        var total = dragonChance + goblinChance + golemChance;

        var randomIndex = Random.Range(0, total);

        return EEnemy.Goblin;
    }

    public void IncreaseWaveNumber()
    {
        _mWaveNumber++;
        //TODO: Check against wave threshold
        //TODO: if wave threshold change percentages
    }
}