using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
		// If the enemy should be sinking
        if(isSinking)
        {
			// move the enemy down by the sinkSpeed per second
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

		// Reduce the current health by the amount of damage sustaind
        currentHealth -= amount;
        
		// Set the position of the particle system to where the hit was sustained
        hitParticles.transform.position = hitPoint;

		// And play the particles
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
		// The enemy is dead
        isDead = true;

		// Turn the collider into a trigger so shots can pass through it
        capsuleCollider.isTrigger = true;

		// Tell the animator that the enemey is dead
        anim.SetTrigger ("Dead");

		// Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing) 
        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
		// Find and disable the Nav Mesh agent
        GetComponent <NavMeshAgent> ().enabled = false;

		// FInd the rigidbody component and make it kinematic (sinc we use Translate to sink the enemy)
        GetComponent <Rigidbody> ().isKinematic = true;

		// The enemy should now sink
        isSinking = true;

		// Incrase the score by the enemy's score value
        ScoreManager.score += scoreValue;

		// After 2 seconds, destroy the enemy
        Destroy (gameObject, 2f);
    }
}
