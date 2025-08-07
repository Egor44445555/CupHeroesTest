using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int currencyWorth = 1;

    float elapsedTime = 0f;
    float flightTime = 0.5f;
    GameObject target;

    void Start()
    {
        target = UIController.main.GetCoinCountIconObject();
    }
    
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (target != null)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(target.transform.position);
            targetPosition = new Vector3(targetPosition.x, targetPosition.y, 0f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
            if (transform.position == targetPosition)
            {
                UIController.main.IncreaseCurrency(currencyWorth);
                Destroy(gameObject);
            }
        }        
    }
}
