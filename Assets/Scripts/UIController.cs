using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController main;

    [Header("Stats settings")]
    [SerializeField] GameObject coinCountIconObject;
    [SerializeField] TextMeshProUGUI coinCount;
    [SerializeField] TextMeshProUGUI damageCount;
    [SerializeField] TextMeshProUGUI speedAttackCount;

    [Header("Upgrade Cards settings")]
    [SerializeField] GameObject upgradeCardsPopup;
    [SerializeField] TextMeshProUGUI upgradeDamageCardCount;
    [SerializeField] TextMeshProUGUI upgradeSpeedAttackCardCount;
    [SerializeField] TextMeshProUGUI upgradeMaxHealthCardCount;

    [Header("Setting start values")]
    [SerializeField] int currentCoins = 0;
    [SerializeField] int currentDamage = 10;
    [SerializeField] int currentSpeedAttack = 1;
    [SerializeField] int currentMaxHealth = 30;

    int increaseDamage = 0;
    int increaseSpeedAttack = 0;
    int increaseMaxHealth = 0;

    void Awake()
    {
        main = this;
    }

    void Start()
    {
        if (coinCount != null)
        {
            coinCount.text = currentCoins.ToString();
        }

        if (damageCount != null)
        {
            damageCount.text = currentDamage.ToString();
        }

        if (speedAttackCount != null)
        {
            speedAttackCount.text = currentSpeedAttack.ToString();
        }
    }

    public void IncreaseCurrency(int amount)
    {
        currentCoins += amount;

        if (coinCount != null)
        {
            coinCount.text = currentCoins.ToString();
        }
    }

    public void IncreaseDamage(int amount)
    {
        currentDamage += amount;

        if (damageCount != null)
        {
            damageCount.text = currentDamage.ToString();
        }
    }

    public void IncreaseSpeedAttack(int amount)
    {
        currentSpeedAttack += amount;

        if (speedAttackCount != null)
        {
            speedAttackCount.text = currentSpeedAttack.ToString();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        currentMaxHealth += amount;
    }

    public int GetDamage()
    {
        return currentDamage;
    }

    public int GetSpeedAttack()
    {
        return currentSpeedAttack;
    }

    public int GetMaxHealth()
    {
        return currentMaxHealth;
    }

    public GameObject GetCoinCountIconObject()
    {
        return coinCountIconObject;
    }

    public void InstallUpgradeCards(int damage, int speedAttack, int maxHealth)
    {
        increaseDamage = damage;
        increaseSpeedAttack = speedAttack;
        increaseMaxHealth = maxHealth;

        if (upgradeDamageCardCount != null)
        {
            upgradeDamageCardCount.text = "+" + damage.ToString();
        }

        if (upgradeSpeedAttackCardCount != null)
        {
            upgradeSpeedAttackCardCount.text = "+" + speedAttack.ToString();
        }

        if (upgradeMaxHealthCardCount != null)
        {
            upgradeMaxHealthCardCount.text = "+" + maxHealth.ToString();
        }
    }

    public void OpenUpgradePopup()
    {
        upgradeCardsPopup.SetActive(true);
    }

    public void UpgradeDamage()
    {
        IncreaseDamage(increaseDamage);
        Upgrade();
    }

    public void UpgradeSpeedAttack()
    {
        IncreaseSpeedAttack(increaseSpeedAttack);
        Upgrade();
    }

    public void UpgradeMaxHealth()
    {
        IncreaseMaxHealth(increaseMaxHealth);
        Upgrade();
    }

    void Upgrade()
    {
        upgradeCardsPopup.SetActive(false);
        BattleController.main.EndCombat();
        Time.timeScale = 1;
    }
}
