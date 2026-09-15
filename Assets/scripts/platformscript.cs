using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 3f;

    private Transform target;

    void Start()
    {
        target = pointB;
    }

    void Update()
    {
        // Move toward the current target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Switch target when the platform reaches it
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            if (target == pointA)
                target = pointB;
            else
                target = pointA;
        }
    }
}