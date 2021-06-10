using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //

    [SerializeField]public bool isCoin, isHeal;

    [SerializeField] private int coinsToAdd;
    [SerializeField] private int healToAdd;
    [SerializeField] public GameObject pickupEffect;

    private bool isCollected;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Activate Pickup
        if(other.CompareTag("Player")&& !isCollected)
        {
            if(isCoin)
            {
                LevelManager.instance.AddCoinsToPlayer(coinsToAdd);
                //LevelManager.instance.coinsCollected += coinsToAdd;

                isCollected = true;
                Destroy(gameObject);

                Instantiate(pickupEffect, transform.position, transform.rotation);
            }

            if(isHeal)
            {
                if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
                {
                    PlayerHealthController.instance.HealPlayer(healToAdd);

                    isCollected = true;
                    Destroy(gameObject);

                    Instantiate(pickupEffect, transform.position, transform.rotation);
                }
            }
        }
    }
}
