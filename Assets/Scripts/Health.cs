using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHitPoint;
    [SerializeField] public GameObject healthBlock;
    [SerializeField] public Image healthBar;
    [SerializeField] TextMeshProUGUI healthCount;
    [SerializeField] TextMeshProUGUI damageCount;
    [SerializeField] GameObject dropCoin;

    float hitPoint = 2f;
    bool isDestroyed = false;
    Animator anim;
    Animator countAnim;
    BattleCharacter battleCharacter;
    bool isPlayingAnimation;

    void Start()
    {
        battleCharacter = GetComponent<BattleCharacter>();
        anim = GetComponent<Animator>();
        countAnim = damageCount.GetComponent<Animator>();
        CheckHealth(true);
    }

    public void TakeDamage(float dmg)
    {
        damageCount.text = dmg.ToString();
        countAnim.SetBool("TakeDamage", true);
        isPlayingAnimation = true;
        hitPoint -= dmg;
        healthBar.fillAmount = hitPoint / maxHitPoint;

        if (hitPoint < maxHitPoint)
        {
            healthBlock.SetActive(true);
        }

        if (hitPoint <= 0 && !isDestroyed)
        {
            if (dropCoin != null)
            {
                Instantiate(dropCoin, transform.position, transform.rotation);
            }

            isDestroyed = true;
            battleCharacter.IsDead();
            anim.SetBool("Die", true);
        }

        CheckHealth();
    }

    public bool GetDestroy()
    {
        return isDestroyed;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void CheckHealth(bool startValue = false)
    {
        if (battleCharacter.characterType == CharacterType.Player)
        {
            maxHitPoint = UIController.main.GetMaxHealth();
        }

        if (startValue)
        {
            hitPoint = maxHitPoint;
        }

        hitPoint = hitPoint < 0 ? 0 : hitPoint;

        if (healthCount != null)
        {
            healthCount.text = hitPoint.ToString() + "/" + maxHitPoint.ToString();
        }
    }

    public void IncreaseHealth(int amount)
    {
        if (battleCharacter.characterType == CharacterType.Player)
        {
            maxHitPoint = amount;
            hitPoint = maxHitPoint;
            healthBar.fillAmount = 1f;
        }
    }
}
