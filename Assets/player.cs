using UnityEngine;

public class player : MonoBehaviour
{

    public float moveSpeed = 5;
    [SerializeField]private Animator _PlayerAnimation;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0);
        transform.position += movement * moveSpeed * Time.deltaTime;
        if (horizontalInput != 0)
        {
            _PlayerAnimation.SetBool("isWalking", true);
        }
        else
        {
            _PlayerAnimation.SetBool("isWalking", false);
        }

    }

}
