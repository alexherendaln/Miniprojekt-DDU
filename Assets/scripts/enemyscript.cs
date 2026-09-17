using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class enemyscript : MonoBehaviour
{
    private Rigidbody2D rb;

    public float Speed;
    [SerializeField] private float patrolDistance = 3;

    private Vector2 pointA;
    private Vector2 pointB;

    private bool movingRight = true;

    public GameObject Enemy_1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
        pointA = new Vector2(transform.position.x - patrolDistance, transform.position.y);
        pointB = new Vector2(transform.position.x + patrolDistance, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingRight == true)
        {
            rb.linearVelocity = new Vector2(Speed, 0);

            if (Vector2.Distance(transform.position, pointB) < 0.5f)
            {
                movingRight = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-Speed, 0);

            if (Vector2.Distance(transform.position, pointA) < 0.5f)
            {
                movingRight = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 leftPoint = new Vector2(
            transform.position.x - patrolDistance,
            transform.position.y
        );

        Vector2 rightPoint = new Vector2(
            transform.position.x + patrolDistance,
            transform.position.y
        );

        Gizmos.DrawWireSphere(leftPoint, 0.5f);
        Gizmos.DrawWireSphere(rightPoint, 0.5f);
        Gizmos.DrawLine(leftPoint, rightPoint);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision. CompareTag("Weapon"))
        {
            Destroy(Enemy_1.gameObject);
        }
    }
}
