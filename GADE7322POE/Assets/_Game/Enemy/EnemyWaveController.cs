using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyWaveController : MonoBehaviour
{
    [Header("Chance")]
    public int goblinChance = 100;
    public int dragonChance = 0;
    public int golemChance = 0;

    [Header("Thresholds")]
    public int waveThreshold = 5;
    public int deathThreshold = 15;

    private int _mWaveNumber;
    private int _mRound;
    private int _deaths;

    public static event Action<int> OnRoundIncreased;
    public static event Action<int> OnWaveIncreased;

    private void Start()
    {
        _mWaveNumber = 1;
        _mRound = 1;
    }

    private void OnEnable()
    {
        Health.OnEnemyDied += HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        _deaths++;

        if (_deaths % deathThreshold == 0)
        {
           IncreaseWaveNumber(); 
        }
    }

    public EEnemy GetRandomEnemy()
    {
        var total = dragonChance + goblinChance + golemChance;

        var randomIndex = Random.Range(0, total);

        if (randomIndex < goblinChance)
        {
            return EEnemy.Goblin;
        }

        if (randomIndex < goblinChance + dragonChance)
        {
            return EEnemy.Dragon;
        }

        return EEnemy.Golem;


    }

    private void IncreaseWaveNumber()
    {
        _mWaveNumber++;

        if (_mWaveNumber % 5 == 0) //Once threshold is met increase round
        {
            _mRound++;
            ChangePercentageChance();
            OnRoundIncreased?.Invoke(_mRound);
            _mWaveNumber = 1;
        }
        
        OnWaveIncreased?.Invoke(_mWaveNumber);
    }

    private void ChangePercentageChance()
    {
        switch (_mRound)
        {
            case 2:
                goblinChance = 70;  // Goblin spawn chance decreases
                dragonChance = 30;  // Dragon spawn chance increases
                golemChance = 0;    // Golem is not spawned yet
                break;
            case 3:
                goblinChance = 50;
                dragonChance = 40;
                golemChance = 10;   // Introduce Golem at a low chance
                break;
            case 4:
                goblinChance = 20;
                dragonChance = 60;
                golemChance = 20;
                break;
            case 5:
                goblinChance = 20;
                dragonChance = 40;
                golemChance = 40;
                break;
            case 6:
                goblinChance = 10;
                dragonChance = 40;
                golemChance = 50;
                break;
            default:
                goblinChance = Random.Range(0,20);
                dragonChance = Random.Range(20, 40);
                golemChance = 100 - goblinChance - dragonChance;
                break;
        }
    }
}