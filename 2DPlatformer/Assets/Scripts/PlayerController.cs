using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private float moveSpeed; //Can set Public / Private
    [SerializeField] private float jumpForce;
    [SerializeField] private bool enableAirControll;
    [SerializeField] private bool enableDoubleJump;

    private bool isGrounded;
    public Transform groundCheckPoint;

    public Rigidbody2D theRB;
    public LayerMask groundReference;

    private bool canDoubleJump;
    private Animator anim;
    private SpriteRenderer theSR;

    [SerializeField] public float knockBackLength, knockbackForceY, knockbackForceX;
    private float knockBackCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(knockBackCounter <= 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, groundReference);

            //Air Control Input
            if (enableAirControll) // Air Controll Enabled
            {
                theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);
            }
            else //Air Controll Disabled
            {
                if (isGrounded)
                {
                    theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);
                }
            }


            //Check Double Jump
            if (enableDoubleJump)
            {
                if (isGrounded)
                {
                    canDoubleJump = true;
                }
            }

            //Jump Input
            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                }
                else
                {
                    if (canDoubleJump)
                    {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                        canDoubleJump = false;
                    }
                }

            }

            //Flip Character
            if (theRB.velocity.x < 0)
            {
                theSR.flipX = true;
            }
            else if (theRB.velocity.x > 0)
            {
                theSR.flipX = false;
            }

            //Fall Check
            //Debug.Log(theRB.velocity.y);
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
            if(!theSR.flipX)
            {
                theRB.velocity = new Vector2(-knockbackForceX, theRB.velocity.y);
            }
            else
            {
                theRB.velocity = new Vector2(knockbackForceX, theRB.velocity.y);
            }    
        }    

        anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("fallSpeed", theRB.velocity.y);
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        theRB.velocity = new Vector2(0f, knockbackForceY);

        anim.SetTrigger("isHurt");
    }
}
