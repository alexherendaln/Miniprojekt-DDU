using UnityEngine;

public class Character_script : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float xStrength;
    public float yStrength;
    private float xVelocity;
    private bool is_grounded;
    public BoxCollider2D ground_ray;
    public float coyote_time;
    public float coyote_timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ground_ray.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            is_grounded = true;
        }
        else
        {
            is_grounded = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            xVelocity = -xStrength;
        }

        if (Input.GetKey(KeyCode.D))
        {
            xVelocity = xStrength;
        }

        if (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false)
        {
            xVelocity = 0;
        }

        myRigidbody.linearVelocity = new Vector2 (
            xVelocity,
            myRigidbody.linearVelocity.y
        );
        

        if (Input.GetKeyDown(KeyCode.Space) == true && is_grounded)
        {
            myRigidbody.linearVelocity = new Vector2 (
                myRigidbody.linearVelocity.x,
                yStrength
            );
        }
    }
}
