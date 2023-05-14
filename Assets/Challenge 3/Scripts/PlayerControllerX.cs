using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{

    [SerializeField] private int yMin = 1;
    [SerializeField] private int yMax = 15;
    private float maxVelocity = 8.0f;

    private bool playedBounce = false;
    public bool gameOver;

    private MeshRenderer meshRenderer;

    public GameObject mainCamera;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    private AudioSource gameMusic;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();
        gameMusic = mainCamera.GetComponent<AudioSource>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && transform.position.y < yMax && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
        if (transform.position.y < yMin && !gameOver) // if the player goes too low
        {
            playerRb.AddForce(Vector3.up * 3, ForceMode.Impulse);
            if (!playedBounce)
            {
                playerAudio.PlayOneShot(bounceSound, 1.0f);
            }
            playedBounce = true;
        }
        else if (transform.position.y > yMin)
        {
            playedBounce = false;
        }
        playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, maxVelocity);
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
            meshRenderer.enabled = false;
            gameMusic.Stop();
            Destroy(this.gameObject, 4f);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
