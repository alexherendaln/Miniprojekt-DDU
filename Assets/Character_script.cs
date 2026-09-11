using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Character_script : MonoBehaviour
{
    public Rigidbody2D myRigidbody;

    //sword1 og sword2 er de to forskellige sving (collider rects) man tager i sværdets 3 hit combo.
    public BoxCollider2D sword1_prefab;
    public BoxCollider2D sword2_prefab;
//skaber den ene instance af sværdet på skærmen, ved begge versioner af attacket.
private BoxCollider2D sword_instance;
[SerializeField] Animator _PlayerAnimation;
    private bool sword_exists = false;

    //lifetime_static bestemmer hvor lang tid sværdet er i live og samtidigt også hvor lang tid man er immovable
    public float sword1_lifetime_static;
    public float sword2_lifetime_static;

    //en timer til at holde styr på hvor lang tid sværdet er i live
    private float sword_lifetime = 0f;

    //stopper spilleren i henholdsvis at bevæge sig og angribe efter et attack i det tid
    public float attack_recovery_time;
    public float movement_recovery_time;

    public float dash_strength;
    public float stance = 1;

    //tæller hvor mange slag der har været i streg
    private float attack_pattern = 0;
    //bestemmer hvor lang tid der kan gå før at attack pattern starter forfra
    public float attack_pattern_buffer;
    private float attack_pattern_timer;
//bestemmer retningen som karakteren kigger, bruges kun til hvor sværdet slås lige nu men kan nok også bruges til at vende karakteren
// -1 er venstre og +1 er højre
private float direction = 1;

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
Debug.Log("game start");
//har skrevet det sådan at det er nemmere at ændre inde i unity
friction = dev_friction / 500 + 0.85f;
    }

    // Update is called once per frame
    void Update()
    {
        //checker om vores ground collison detector rammer jorden og ændrer variablen is_grounded
        ground_check();

        x_movement();
        y_movement();

        //opdaterer de nye vektorere så vi får movement
        myRigidbody.linearVelocity = new Vector2 (
            xVelocity,
            myRigidbody.linearVelocity.y
        );

        //vælger det rigtige attack til den stance man har valgt
        if (stance == 1)
        {
            sword_attack();
        }

        timers();
    }

    void timers()
    {
        sword_lifetime -= Time.deltaTime;

        if (sword_exists == false)
        {
            attack_pattern_timer -= Time.deltaTime;
        }

        if (attack_pattern_timer < 0)
        {
            attack_pattern = 0;
        }
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
//tilsætter friction så at spilleren langsomt stopper op
xVelocity *= friction;

if (immovable == false)
{

//laver checks om vi står op af en mur(med en collider på hver side af spilleren) da vi så ikke skal kunne tilføje kraft mod den, skabte nogle problemer med unitys physics
if (left_wall_ray.IsTouchingLayers(LayerMask.GetMask("Ground")) == false)
{
    if (Input.GetKey(KeyCode.A))
    {   
        direction = -1;
        xVelocity -= xStrength;
    }

    if (Input.GetKey(KeyCode.D))
    {
        direction = 1;
        xVelocity += xStrength;
    }
}
}

//opdater direction for joystick/axis input også (brugtes i setup)
if (Input.GetAxisRaw("Horizontal") != 0)
{
    direction = Input.GetAxisRaw("Horizontal");
}
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
        // laver check så vi kun kan hoppe når vi er på jorden/ når vores ground_ray rammer jorden
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
        //lader kun spilleren angribe hvis der er gået så lang tid efter et angreb
        if (sword_lifetime <= -attack_recovery_time)
        {
            //bruger variablen attack_pattern til at bestemme hvilket attack i comboen der skal udføres
            if (attack_pattern >= 2)
            {
                sword2_attack();
            }
            else
            {
                sword1_attack();
            }

        }

        //lader kun spilleren bevæge sig hvis der er gået så lang tid efter et angreb
        if (sword_lifetime <= movement_recovery_time && immovable == true)
        {
            immovable = false;
        }

        //hvis sværdets livstid er ovre såå forsvinder sværdet
        if (sword_lifetime <= 0 && sword_exists == true)
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
        
        if (sword_exists == false && Input.GetMouseButtonDown(0))
            {   
                sword_exists = true;
                immovable = true;

                //får timeren til at tælle ned for netop der her attacks tid, da det er kortere
                sword_lifetime = sword1_lifetime_static;
                
                //genstarter bufferen så den kan se hvor lang tid siden der sidst har været et attack
                attack_pattern_timer = attack_pattern_buffer;
                
                //bevæger karakteren i retningen af dets attack, (i fremtiden skal vi måske gøre sådan at hvis de rammer en enemy eller væg så ryger de tilbage istedet)
                xVelocity = dash_strength * direction;

                //får karakteren til at stoppe i luften hvis de angriber der
                myRigidbody.linearVelocity = new Vector2 (
                xVelocity,
                0
                );

                //skaber et sværd på spillerens position i den retning de kigger
                sword_instance = Instantiate(
                    sword1_prefab,
                    new Vector2(transform.position.x+direction,transform.position.y),
                    Quaternion.identity, 
                    this.transform
                    );
                //tæller op hvor mange gange der er blevet slået
                attack_pattern += 1;
            }
    }
    void sword2_attack()
    {
        
        if (sword_exists == false && Input.GetMouseButtonDown(0))
            {   
                sword_exists = true;

                //får timeren til at tælle ned for netop der her attacks tid, da det er længere
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
                //genstarter attack_pattern så den kan tælle op til et større attack igen
                attack_pattern = 0;
            }
    }
}
