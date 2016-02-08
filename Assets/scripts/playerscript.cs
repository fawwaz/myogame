using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerscript : MonoBehaviour {
    public int maxhealth = 100;
    public int currhealth;
    public Slider healthSlider;
    bool isdead;
	// Use this for initialization
	void Awake () {
        currhealth = maxhealth;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        // Restart
        if (coll.gameObject.tag == "player")
        {
            TakeDamage(bulletscript.bulletDamage);
        }
    }

    int getHealth()
    {
        return currhealth;
    }
	// Update is called once per frame
	void Update () {
	
	}


    public void TakeDamage(int amount)
    {
        // Reduce the current health by the damage amount.
        currhealth -= amount;

        // Set the health bar's value to the current health.
        healthSlider.value = currhealth;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currhealth <= 0 && !isdead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        isdead = true;
    }       
}
