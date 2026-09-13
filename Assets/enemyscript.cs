using UnityEngine;

public class enemyscript : MonoBehaviour
{
    public GameObject PointA;
    public GameObject PointB;
    private Rigidbody2D rb;
    private Transform currentpoint;
    public float Speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentpoint = PointB.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentpoint.position - transform.position;
        if (currentpoint == PointB.transform)
        {
            rb.linearVelocity = new Vector2(Speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-Speed, 0);
        }

        if(Vector2.Distance(transform.position, currentpoint.position) < 0.5f && currentpoint == PointB.transform)
        {
            currentpoint = PointA.transform;
        }
        if (Vector2.Distance(transform.position, currentpoint.position) < 0.5f && currentpoint == PointA.transform)
        {
            currentpoint = PointB.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(PointB.transform.position, 0.5f);
        Gizmos.DrawLine(PointA.transform.position, PointB.transform.position);
    }
}
