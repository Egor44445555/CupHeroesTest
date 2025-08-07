using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float maxHitPoint = 2f;
    [SerializeField] public GameObject healthBlock;
    [SerializeField] public Image healthBar;
    [SerializeField] float hitPoint = 2f;

    bool isDestroyed = false;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        hitPoint = maxHitPoint;
    }

    public void TakeDamage(float dmg)
    {
        hitPoint -= dmg;
        healthBar.fillAmount = hitPoint / maxHitPoint;

        if (hitPoint < maxHitPoint)
        {
            healthBlock.SetActive(true);
        }

        if (hitPoint <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            BattleController.main.NextCharacter();
            anim.SetBool("Die", true);
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
