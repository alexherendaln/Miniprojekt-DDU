using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class playerControl : MonoBehaviour
{
    public Slider healthBar;

    public TMP_Text healthText;

    public int health = 100;
    public float invincibilityTime = 10f;
    private float invincibilityTimer = 0f;

    public int maxHealth = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health + " / " + maxHealth; 
        healthBar.value = (float)health/(float)maxHealth;
        invincibilityTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy" && invincibilityTimer <= 0)
        {
            health -= 25;
            invincibilityTimer = invincibilityTime;
        }
    }
}
