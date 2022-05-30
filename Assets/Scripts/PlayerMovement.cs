using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float jumpSpeed = 12f;

    [Header("VFX")]
    [SerializeField] GameObject playerDeathVFX;
    [SerializeField] GameObject ghostDeathVFX;
 
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;

    Animator myAnimator;
    static AudioPlayer audioPlayer;
    DeathHandler deathHandler;

    bool isAlive = true;
    bool hasWon = false;

    void Start()
    {
       myRigidbody = GetComponent<Rigidbody2D>();
       myAnimator = GetComponent<Animator>();
       myBodyCollider = GetComponent<CapsuleCollider2D>();
       myFeetCollider = GetComponent<BoxCollider2D>();
       deathHandler = FindObjectOfType<DeathHandler>();
       audioPlayer = FindObjectOfType<AudioPlayer>();
    }

    void Update()
    {
        if(hasWon) { return; }
        Die();
        if(!isAlive) { return; }
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value) 
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || gameObject.tag == "Player" && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Walkthrough"))) 
        {
            if(value.isPressed)
            {
                myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    void OnRestart()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
    }

    void Die()
    {
        if (isAlive)
        {
            if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
            {
                isAlive = false;
                myAnimator.SetTrigger("Dying");
                audioPlayer.PlayDeathClip();
                GameObject instance = Instantiate(playerDeathVFX, transform.position, Quaternion.identity);
                Destroy(instance, 1);
            }
        }
        else if(!isAlive && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Walkthrough")))
        {
            if(gameObject.tag == "Ghost" && ghostDeathVFX != null) 
            {
                GameObject instance = Instantiate(ghostDeathVFX, transform.position, Quaternion.identity);
                Destroy(instance, 1);
            }
            deathHandler.StartDeathSequence();
        }
    }

    public void SetWon() 
    {
        hasWon = true;
    }
}
