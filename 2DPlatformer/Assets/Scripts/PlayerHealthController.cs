using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    public int currentHealth;

    [SerializeField] public float invincibleLength;
    private float invincibleCounter;

    [SerializeField] public GameObject deathEffect;

    private int healthToAdd;
    private int damageToAdd;

    private SpriteRenderer theSR;

    public static PlayerHealthController instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            if(invincibleCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1);
            }
        }
    }
    
    public void DealDamage(int damageToAdd)
    {
        if(invincibleCounter <=0)
        {
            if(currentHealth - damageToAdd < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = currentHealth - damageToAdd;
            }

            //currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                //gameObject.SetActive(false);

                Instantiate(deathEffect, transform.position, transform.rotation);

                LevelManager.instance.RespawnPlayer();
            }
            else
            {
                invincibleCounter = invincibleLength;
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, .55f);

                PlayerController.instance.KnockBack();
            }

            UIController.instance.UpdateHealthDisplay();
        }

    }
    
    public void HealPlayer(int healthToAdd)
    {
        if(healthToAdd + currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth = healthToAdd + currentHealth;
        }
        UIController.instance.UpdateHealthDisplay();
    }
}
