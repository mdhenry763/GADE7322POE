using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;

    public SoundObject[] soundObjects;

    private SoundType soundPlayed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(this);
    }

    public void PlaySound(SoundType type)
    {
        var soundObj = soundObjects.FirstOrDefault((sound) => sound.type == type);
        
        if(soundObj.clip == null) return;
        
        audioSource.PlayOneShot(soundObj.clip);
    }
}

public enum SoundType
{
    Button,
    CannonPlaced,
    CannonFire,
    EnemyKilled,
    DefenderDamaged,
    TowerDamaged,
    CoinIncrease,
}

[Serializable]
public struct SoundObject
{
    public AudioClip clip;
    public SoundType type;
}
