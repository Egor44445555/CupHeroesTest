using UnityEngine;
using System.Collections.Generic;

public class BattleWave : MonoBehaviour
{
    [SerializeField] int increaseDamage = 1;
    [SerializeField] int increaseSpeedAttack = 1;
    [SerializeField] int increaseMaxHealth = 10;
    [SerializeField] List<BattleCharacter> enemies = new List<BattleCharacter>();
    [SerializeField] bool lastWave = false;
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            BattleController.main.SetEnemies(enemies, enemies.Count, lastWave);
            UIController.main.InstallUpgradeCards(increaseDamage, increaseSpeedAttack, increaseMaxHealth);
        }
    }
}
