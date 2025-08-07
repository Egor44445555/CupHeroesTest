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
    int maxEnemies = 0;
    Rigidbody2D playerRb;
    Health playerHealth;
    Transform playerTransform;
    bool isProcessingTurn = false;
    bool lastWave = false;
    List<BattleCharacter> currentEnemies = new List<BattleCharacter>();
    List<BattleCharacter> characterQueue = new List<BattleCharacter>();

    void Awake()
    {
        main = this;
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerHealth = player.GetComponent<Health>();
    }

    public void StartMove()
    {
        StartCoroutine(EnemyCheck());
    }

    IEnumerator EnemyCheck()
    {
        while (true)
        {
            if (!combatActive)
            {
                if (currentEnemies.Count > 0)
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

    public void StartCombat()
    {
        playerRb.linearVelocity = Vector2.zero;
        player.StartAnimation("Walk", false);
        characterQueue.Clear();

        if (currentEnemies.Count > 0)
        {
            characterQueue.Add(player);
        }

        for (int i = 0; i < currentEnemies.Count; i++)
        {
            if (currentEnemies[i] != null)
            {
                BattleCharacter character = currentEnemies[i].GetComponent<BattleCharacter>();

                if (!character.GetDeadStatus())
                {
                    characterQueue.Add(character);
                }
            }
        }

        characterQueue.RemoveAll(c => c == null || c.GetDeadStatus());

        if (characterQueue.Count == 0)
        {
            Improvement();
            return;
        }

        if (characterQueue.Count > 1)
        {
            BattleCharacter target = null;

            if (characterQueue[currentCharacter].characterType == CharacterType.Player)
            {
                for (int i = 0; i < characterQueue.Count; i++)
                {
                    if (characterQueue[i].characterType == CharacterType.Enemy && !characterQueue[i].GetComponent<Health>().GetDestroy())
                    {
                        target = characterQueue[i];
                    }
                }
            }
            else
            {
                target = player;
            }

            if (target != null)
            {
                characterQueue[currentCharacter].StartCombat(CalculateMovementPoints(characterQueue[currentCharacter], target), target.transform);
                combatActive = true;
            }
        }
        else
        {
            Improvement();
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
            }
        }

        return points;
    }

    public void NextCharacter()
    {
        currentCharacter = currentCharacter < characterQueue.Count - 1 ? currentCharacter + 1 : 0;

        if (currentEnemies.Count > 0)
        {
            StartCombat();
        }
    }

    void Improvement()
    {
        if (!lastWave)
        {
            Time.timeScale = 0;
            combatActive = false;
            currentEnemies.Clear();
            UIController.main.OpenUpgradePopup();
        }
        else
        {
            player.StartAnimation("Victory", true);
            UIController.main.OpenVictoryPopup();
        }
    }

    public void EndCombat()
    {
        combatActive = false;
        currentCharacter = 0;
        maxEnemies = 0;
        playerHealth.CheckHealth();
    }

    public void SetEnemies(List<BattleCharacter> enemies, int _maxEnemies, bool _lastWave)
    {
        currentEnemies = enemies;
        maxEnemies = _maxEnemies;
        lastWave = _lastWave;
    }

    public bool IsCombatActive()
    {
        return combatActive;
    }

    public BattleCharacter GetPlayer()
    {
        return player;
    }
}