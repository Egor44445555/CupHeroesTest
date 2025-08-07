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
    public float visionRange = 5f;
    public float meleeDamage = 5f;

    [SerializeField] Transform bulletCreatePoint;
    [SerializeField] GameObject bulletPrefab;

    bool inCombat = false;
    List<Vector3> movementPoints = new List<Vector3>();
    int currentMovementPoint = 0;
    Animator anim;
    Transform currentTarget;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void StartCombat(List<Vector3> points, Transform target)
    {
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
        anim.SetBool("Walk", true);

        if (currentMovementPoint < movementPoints.Count - 1)
        {
            currentMovementPoint++;
            StartCoroutine(MoveToPosition(movementPoints[currentMovementPoint], OnMovementComplete));
        }
        else
        {
            StartCoroutine(MoveToPosition(currentTarget.position, OnMovementComplete));
        }
    }

    public void Retreat()
    {
        if (!inCombat || weaponType == WeaponType.Ranged) return;

        print("Retreat");
        anim.SetBool("Walk", true);

        if (currentMovementPoint > 0)
        {
            currentMovementPoint--;
            StartCoroutine(MoveToPosition(movementPoints[currentMovementPoint], OnMovementComplete));
        }
    }

    IEnumerator MoveToPosition(Vector3 target, System.Action onComplete = null)
    {        
        while (Vector3.Distance(transform.position, target) > 0.1f && currentTarget != null)
        {
            if (Vector3.Distance(currentTarget.position, transform.position) >= 1.5f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            else
            {
                NextCharacter();
            }
            
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
            if (bulletCreatePoint != null && bulletPrefab != null)
            {
                GameObject bulletObj = Instantiate(bulletPrefab, bulletCreatePoint.position, bulletCreatePoint.rotation);
                Bullet bullet = bulletObj.GetComponent<Bullet>();
                bullet.SetTarget(currentTarget, bulletCreatePoint);
            }

            if (weaponType == WeaponType.Melee)
            {
                currentTarget.GetComponent<Health>().TakeDamage(meleeDamage);
            }
        }
    }

    public void NextCharacter()
    {
        currentTarget = null;
        anim.SetBool("Walk", false);
        anim.SetBool("Attack", false);
        BattleController.main.NextCharacter();
    }

    public void StartAnimation(string name, bool active)
    {
        if (anim != null)
        {
            anim.SetBool(name, active);
        }
    }
}