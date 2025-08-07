using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [Header("Attack settings")]
    [SerializeField] float bulletDamage = 1;

    [Header("Fly settings")]
    [SerializeField] float acceleration = 2f;
    [SerializeField] float flightTime = 10f;
    [SerializeField] float heightOffset = 2f;

    [Header("Rotate settings")]
    [SerializeField] float rotationSpeed = 5f;

    bool destroy = false;
    float currentSpeed;

    Transform target;
    Vector2 targetPosition;
    Vector2 startPosition;
    float elapsedTime = 0f;
    Vector3 previousPosition;
    Rigidbody2D rb;
    Vector2 currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        previousPosition = transform.position;
    }

    public void SetTarget(Transform _target, Transform startPoint)
    {
        target = _target;
        currentTarget = _target.GetComponent<Collider2D>().bounds.center;
    }

    void Update()
    {
        if (destroy) return;

        elapsedTime += Time.deltaTime;
        currentSpeed += acceleration * Time.deltaTime;
        float normalizedTime = elapsedTime / flightTime;

        Vector2 currentPosition = CalculateParabolicPosition(startPosition, currentTarget, heightOffset, normalizedTime);
        transform.position = currentPosition;
        RotateBullet(currentPosition);

        if (Vector3.Distance(transform.position, currentTarget) < 0.3f)
        {
            destroy = true;
        }

        previousPosition = transform.position;
    }

    void RotateBullet(Vector3 currentPosition)
    {
        Vector3 moveDirection = (currentPosition - previousPosition).normalized;

        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            Vector3 currentEuler = transform.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    Vector2 CalculateParabolicPosition(Vector2 start, Vector2 end, float height, float time)
    {
        float x = Mathf.Lerp(start.x, end.x, time);
        float y = Mathf.Lerp(start.y, end.y, time) + height * Mathf.Sin(Mathf.PI * time);
        return new Vector2(x, y);
    }
    
    void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float damage = bulletDamage;

            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            Destroy(gameObject);
        }
    } 
}
