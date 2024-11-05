using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeDefender : MonoBehaviour
{
    [Header("References: ")] 
    [SerializeField] private Health health;
    
    [Header("Upgrades")] 
    [SerializeField] private GameObject upgrade1;
    [SerializeField] private GameObject upgrade2;
    
    private int _mUpgradeIndex = 1;
    
    public void Upgrade()
    {
        if(_mUpgradeIndex > 2) return;
        UpgradeHealth();
        UpgradeAttack();
    }

    private void UpgradeHealth()
    {
        health.ResetHealth();
    }

    private void UpgradeAttack()
    {
        if(_mUpgradeIndex == 1){ upgrade1.SetActive(true);}
        if(_mUpgradeIndex == 2) upgrade2.SetActive(true);

        _mUpgradeIndex++;
    }
    
    
}
