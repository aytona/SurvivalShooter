using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(damaged)
        {
            damageImage.color = flashColour;
        }
        else
        {
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        damaged = false;
    }


    public void TakeDamage (int amount)
    {
		// Set the damaged flag so the screen will flash
        damaged = true;

		// Reduce the current health by the damage amount
        currentHealth -= amount;

		// Set the health bar's value to the current health
        healthSlider.value = currentHealth;

		// Play the hurt sound effect
        playerAudio.Play ();

		// If the player has lost all it's health and the death flag hasn't been set yet
        if(currentHealth <= 0 && !isDead)
        {
            Death ();
        }
    }


    void Death ()
    {
		// Set the death flag so this function won't be called again
        isDead = true;

		// Turn off any remaining shooting effects
        playerShooting.DisableEffects ();

		// Tell the animator that the player is dead
        anim.SetTrigger ("Die");

		// Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing)
        playerAudio.clip = deathClip;
        playerAudio.Play ();

		// Turn off the movement and shooting script
        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        Application.LoadLevel (Application.loadedLevel);
    }
}
