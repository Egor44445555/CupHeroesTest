using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CharacterType { Player, Enemy }
public enum WeaponType { Melee, Ranged }

public class BattleCharacter : MonoBehaviour
{
    public CharacterType characterType;
    public WeaponType weaponType;
    public float moveSpeed = 3f;
    public float attackRange = 1f;
    
    [SerializeField] int speedAttack = 1;
    [SerializeField] float meleeDamage = 5f;

    [SerializeField] Transform bulletCreatePoint;
    [SerializeField] GameObject bulletPrefab;

    bool isDead = false;
    bool inCombat = false;
    List<Vector3> movementPoints = new List<Vector3>();
    int currentMovementPoint = 0;
    Animator anim;
    Transform currentTarget;
    int fireCount = 0;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartCombat(List<Vector3> points, Transform target)
    {
        if (characterType == CharacterType.Player)
        {
            speedAttack = UIController.main.GetSpeedAttack();
        }
        
        inCombat = false;
        currentTarget = target;

        if (movementPoints.Count == 0)
        {
            movementPoints = points;
        }

        if (Vector3.Distance(transform.position, target.position) > attackRange && weaponType == WeaponType.Melee)
        {
            MoveToNextPoint();
        }
        else
        {
            inCombat = true;
            anim.SetBool("Attack", true);
        }
    }

    public void MoveToNextPoint()
    {
        anim.SetBool("Attack", false);
        anim.SetBool("Walk", true);

        if (currentMovementPoint <= movementPoints.Count - 1)
        {
            StartCoroutine(MoveToPosition(movementPoints[currentMovementPoint], OnMovementComplete));
            currentMovementPoint++;
        }
        else
        {
            anim.SetBool("Walk", false);

            if (BattleController.main.IsCombatActive())
            {
                anim.SetBool("Attack", true);
            }
            else
            {
                NextCharacter();
            }
        }
    }

    IEnumerator MoveToPosition(Vector3 target, System.Action onComplete = null)
    {        
        while (Vector3.Distance(transform.position, target) > 0.1f && currentTarget != null && !isDead)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);            
            yield return null;
        }

        onComplete?.Invoke();
    }

    void OnMovementComplete()
    {
        NextCharacter();
    }

    public void Fire()
    {
        if (currentTarget != null)
        {
            Health healthTarget = currentTarget.GetComponent<Health>();

            if (!isDead && !healthTarget.GetDestroy())
            {
                if (bulletCreatePoint != null && bulletPrefab != null)
                {
                    GameObject bulletObj = Instantiate(bulletPrefab, bulletCreatePoint.position, bulletCreatePoint.rotation);
                    Bullet bullet = bulletObj.GetComponent<Bullet>();
                    bullet.SetTarget(currentTarget, bulletCreatePoint);
                }

                if (weaponType == WeaponType.Melee)
                {
                    healthTarget.TakeDamage(meleeDamage);
                }
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }        
    }

    public void NextCharacter()
    {
        if (fireCount >= speedAttack - 1)
        {
            fireCount = 0;
            currentTarget = null;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", false);
            BattleController.main.NextCharacter();
        }
        else if (currentTarget != null)
        {
            BattleController.main.StartCombat();
            fireCount++;
        }
    }

    public void IsDead()
    {
        isDead = true;
    }

    public bool GetDeadStatus()
    {
        return isDead;
    }

    public void StartAnimation(string name, bool active)
    {
        if (anim != null)
        {
            anim.SetBool(name, active);
        }
    }
}