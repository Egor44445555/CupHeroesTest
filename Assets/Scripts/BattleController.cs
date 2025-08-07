using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour
{
    public static BattleController main;
    [SerializeField] BattleCharacter player;
    [SerializeField] float checkInterval = 0.2f;
    [SerializeField] LayerMask enemyLayerMask;
    
    bool combatActive = false;
    int currentCharacter = 0;
    Rigidbody2D playerRb;
    Transform playerTransform;
    bool isProcessingTurn = false;
    List<BattleCharacter> characterQueue = new List<BattleCharacter>();

    void Awake()
    {
        main = this;
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        StartCoroutine(EnemyCheck());
    }
    
    IEnumerator EnemyCheck()
    {
        while (true)
        {
            if (!combatActive)
            {
                var hits = Physics2D.OverlapCircleAll(
                    playerTransform.position, 
                    player.visionRange, 
                    enemyLayerMask);
                    
                if (hits.Length > 0)
                {
                    StartCombat();
                }
                else
                {
                    playerRb.linearVelocity = new Vector2(player.moveSpeed, playerRb.linearVelocity.y);
                    player.StartAnimation("Walk", true);
                }
            }
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void StartCombat()
    {
        playerRb.linearVelocity = Vector2.zero;
        player.StartAnimation("Walk", false);
        characterQueue.Clear();

        var hits = Physics2D.OverlapCircleAll(
            playerTransform.position, 
            player.visionRange, 
            enemyLayerMask);

        if (hits.Length > 0)
        {
            characterQueue.Add(player);

            for (int i = 0; i < hits.Length; i++)
            {
                characterQueue.Add(hits[i].GetComponent<BattleCharacter>());
            }
        }

        if (characterQueue.Count > 0)
        {
            BattleCharacter target;

            if (characterQueue[currentCharacter].characterType == CharacterType.Player)
            {
                target = characterQueue[1];
            }
            else
            {
                target = player;
            }

            characterQueue[currentCharacter].StartCombat(CalculateMovementPoints(characterQueue[currentCharacter], target), target.transform);
            combatActive = true;
        }
    }
    
    List<Vector3> CalculateMovementPoints(BattleCharacter character, BattleCharacter target)
    {
        List<Vector3> points = new List<Vector3>();
    
        if (character.weaponType == WeaponType.Melee)
        {
            Vector3 startPos = character.transform.position;
            Vector3 endPos = target.transform.position;
            Vector3 direction = (endPos - startPos).normalized;
            
            float totalDistance = Vector3.Distance(startPos, endPos);
            int steps = Mathf.FloorToInt(totalDistance / character.attackRange);
            
            if (steps > 0)
            {
                float stepDistance = totalDistance / steps;
                
                for (int i = 1; i < steps; i++)
                {
                    points.Add(startPos + direction * (stepDistance * i));
                }
                
                // float finalPointDistance = totalDistance * 0.85f;
                points.Add(startPos + direction * totalDistance);
            }
        }
        
        return points;
    }

    public void NextCharacter()
    {
        currentCharacter = currentCharacter < characterQueue.Count - 1 ? currentCharacter + 1 : 0;
        StartCombat();
    }
    
    public void EndCombat()
    {
        combatActive = false;
        currentCharacter = 0;
    }
}