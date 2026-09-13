using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Character_script : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public BoxCollider2D sword1_prefab;
    private BoxCollider2D sword_instance;
    public BoxCollider2D sword2_prefab;
    [SerializeField] Animator _PlayerAnimation;
    private bool sword_exists = false;
    public float sword1_lifetime_static;
    public float sword2_lifetime_static;
    private float sword_lifetime = 0f;
    public float dash_strength;
    public float sword_equip = 1;
    private float attack_pattern = 0;
    public float attack_pattern_buffer;
    private float attack_pattern_timer;

    public float xStrength;
    private float xVelocity;
    public float yStrength;
  
    
    public bool immovable;

    public bool is_grounded;
    public BoxCollider2D ground_ray;
    public BoxCollider2D left_wall_ray;
    public BoxCollider2D right_wall_ray;
    private float direction = 1;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ground_check();

        x_movement();
        y_movement();

        if (sword_equip == 1)
        {
            sword_attack();
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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        transform.position += movement * xStrength * Time.deltaTime;

        if ( Input.GetAxisRaw("Horizontal")!= 0)
        {
        direction = Input.GetAxisRaw("Horizontal");
        }


        if (horizontalInput != 0)
        {
            _PlayerAnimation.SetBool("isWalking", true);
        }
        else
        {
            _PlayerAnimation.SetBool("isWalking", false);
        }

        Debug.Log(Input.GetAxisRaw("Horizontal"));

    }
    void y_movement()
    {
        myRigidbody.linearVelocity = new Vector2 (
            xVelocity,
            myRigidbody.linearVelocity.y
        );
        

        if (Input.GetKeyDown(KeyCode.Space) == true && is_grounded && immovable == false)
        {
            myRigidbody.linearVelocity = new Vector2 (
                myRigidbody.linearVelocity.x,
                yStrength
            );
        }
    }

    void sword_attack()
    {
        if (attack_pattern >= 2)
        {
            sword2_attack();
        }
        else
        {
            sword1_attack();
        }

        if (attack_pattern_timer < 0)
        {
            attack_pattern = 0;
        }

        if (sword_exists == false)
        {
            attack_pattern_timer -= Time.deltaTime;
        }

        if (sword_lifetime <= 0.05 && sword_exists == true)
        {
            sword_exists = false;
            Destroy(sword_instance.gameObject);
            immovable = false;
            xVelocity = 0;
            myRigidbody.linearVelocity = new Vector2 (
            0,
            0
            );            
        }
    }
    void sword1_attack()
    {
        
        if (sword_lifetime <= 0 && Input.GetMouseButtonDown(0))
            {   
                sword_exists = true;
                sword_lifetime = sword1_lifetime_static;
                
                immovable = true;

                attack_pattern_timer = attack_pattern_buffer;

                xVelocity = dash_strength * direction;
                myRigidbody.linearVelocity = new Vector2 (
                xVelocity,
                0
                );

                sword_instance = Instantiate(
                    sword1_prefab,
                    new Vector2(transform.position.x+direction,transform.position.y),
                    Quaternion.identity, 
                    this.transform
                    );
                attack_pattern += 1;
            }
    }
    void sword2_attack()
    {
        
        if (sword_lifetime <= 0 && Input.GetMouseButtonDown(0))
            {   
                sword_exists = true;
                sword_lifetime = sword2_lifetime_static;

                immovable = true;
                
                xVelocity = dash_strength*3 * direction;
                myRigidbody.linearVelocity = new Vector2 (
                myRigidbody.linearVelocity.x,
                0
                );

                sword_instance = Instantiate(
                    sword2_prefab,
                    new Vector2(transform.position.x+direction,transform.position.y),
                    Quaternion.identity, 
                    this.transform
                    );
                attack_pattern = 0;
            }
    }
}
