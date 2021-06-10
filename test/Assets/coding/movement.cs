using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movement : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 0f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] Vector2 deathKickLava = new Vector2(25f, 25f);
    [SerializeField] float LevelLoadSlowMo = 0.2f;
    private Rigidbody2D rb;
    public float speed;
    public float speedAir;
    private float moveInput;
    private bool isGrounded;
    private bool isGroundedNoJump;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public LayerMask whatIsGroundNoJump;
    public float jumpForce;
    private float jumpTimeCounter;
    private float jumpdownTimeCounter;
    public float jumpTime;
    public float jumpdownTime;
    private bool isJumping;
    private bool isBreak;
    public bool isAlive = true;
    Collider2D myBodyCollider;
    public GameObject effect;
    public GameObject effect2;
    Animator myAnimator;
    private enum State { idle, jumping, falling }
    private State state = State.idle;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) { return; }

        JumpDown();
        Jump();
        Die();
        DieLava();
        StartCoroutine(LoadNextLevel());
        VelocityState();
    }
    void FixedUpdate()
    {
        if (!isAlive) { return; }

        if (isGrounded == true)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.AddForce(Vector2.right * moveInput * speed);
        }
        else
        {
            moveInput = Input.GetAxisRaw("Horizontal");
            rb.AddForce(Vector2.right * moveInput * speedAir);
        }

    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy")) && state == State.falling)
        {
            isAlive = false;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            Instantiate(effect, transform.position, Quaternion.identity);
            myAnimator.SetTrigger("Die");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        else if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Hazards", "Enemy")) && state == State.jumping)
        {
            isAlive = false;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            Instantiate(effect, transform.position, Quaternion.identity);
            myAnimator.SetTrigger("Die");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        else if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) && state == State.idle)
        {
            isAlive = false;
            GetComponent<Rigidbody2D>().velocity = deathKick;
            Instantiate(effect, transform.position, Quaternion.identity);
            myAnimator.SetTrigger("Die");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
    private void DieLava()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Lava")) && isBreak == false)
        {
            isAlive = false;
            GetComponent<Rigidbody2D>().velocity = deathKickLava;
            Instantiate(effect, transform.position, Quaternion.identity);
            myAnimator.SetTrigger("Die");
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }
    IEnumerator LoadNextLevel()

    {
        if (Input.GetKeyUp(KeyCode.X))
        {
            yield return new WaitForSecondsRealtime(LevelLoadDelay);
            //Time.timeScale = LevelLoadSlowMo;
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        if(Input.GetKeyUp(KeyCode.Z))
        {
            yield return new WaitForSecondsRealtime(LevelLoadDelay);
            //Time.timeScale = LevelLoadSlowMo;
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }
    }
    private void VelocityState()
    {
        if (state == State.jumping)
        {
            if (rb.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (isGroundedNoJump == false)
        {
            state = State.falling;
        }
        else
        {
            state = State.idle;
        }
    }
    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded == true && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.AddForce(Vector2.up * jumpForce);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if (jumpTimeCounter > 0 && isJumping == true)
            {
                rb.AddForce(Vector2.up * jumpForce);
                jumpTimeCounter -= Time.deltaTime;
                state = State.jumping;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }
    }
    private void JumpDown()
    {
        isGroundedNoJump = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGroundNoJump);

        if (state == State.falling && Input.GetKeyDown(KeyCode.S))
        {
            isBreak = true;
            jumpdownTimeCounter = jumpdownTime;
            rb.AddForce(Vector2.down * jumpForce);
            Instantiate(effect2, transform.position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (jumpdownTimeCounter > 0 && state == State.falling)
            {
                rb.AddForce(Vector2.down * jumpForce);
                jumpdownTimeCounter -= Time.deltaTime;
            }
            else
            {
                isBreak = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            isBreak = false;
        }
    }
}   

