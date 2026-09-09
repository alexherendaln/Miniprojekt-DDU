using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Character_script : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public BoxCollider2D sword1_prefab;
    private BoxCollider2D sword1_instance;
    private bool sword_exists = false;
    public float sword_lifetime_static;
    private float sword_lifetime = 0f;

    public float sword_equip = 1;

    private float direction = 1;

    public float xStrength;
    private float xVelocity;
    public float yStrength;

    public float dev_friction;
    private float friction;    


    public bool is_grounded;
    public BoxCollider2D ground_ray;
    public BoxCollider2D left_wall_ray;
    public BoxCollider2D right_wall_ray;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        friction = dev_friction / 500 + 0.85f;  
    }

    // Update is called once per frame
    void Update()
    {
        ground_check();

        x_movement();
        y_movement();

        if (sword_equip == 1)
        {
            sword1_attack();
        }

        sword_lifetime -= Time.deltaTime;

    }

    void ground_check()
    {
        if (ground_ray.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            is_grounded = true;
        }
        else
        {
            is_grounded = false;
        }
    }
    void x_movement()
    {
        xVelocity *= friction;

        if (left_wall_ray.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            if (Input.GetKey(KeyCode.A))
            {   
                direction = -1;
                xVelocity -= xStrength;
            }
        }

        if (right_wall_ray.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
        {
            if (Input.GetKey(KeyCode.D))
            {
                direction = 1;
                xVelocity += xStrength;
            }  
        }  
    }
    void y_movement()
    {
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
    void sword1_attack()
    {
        
        if (sword_lifetime <= 0f && Input.GetMouseButtonDown(0))
            {   
                sword_exists = true;
                sword_lifetime = sword_lifetime_static;


            
                sword1_instance = Instantiate(
                    sword1_prefab,
                    new Vector2(transform.position.x+direction,transform.position.y),
                    Quaternion.identity, 
                    this.transform
                    );

            }

        if (sword_lifetime <= 0f && sword_exists == true)
        {
            sword_exists = false;
            Destroy(sword1_instance.gameObject);
        }

    }
}
