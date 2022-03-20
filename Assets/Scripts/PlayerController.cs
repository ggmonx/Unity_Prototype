using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public float jumpVelocity;
    private bool hasJumped = false;
    public int maxJumps;
    private int numJumps = 0;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer sr;
    public GameObject player;
    private Animator anim;
    private float moveStopTime;
    private bool canMove = true;
    public LayerMask platformslayerMask;
    [SerializeField] GameObject atkHitbox;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        sr = player.GetComponent<SpriteRenderer>();
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        anim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            atkHitbox.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            atkHitbox.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jump") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.SetBool("Jumping?", false);
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Land") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            anim.SetBool("Landing?", false);
        }

        if (canMove)
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                anim.SetBool("Walking?", true);
            }
            else
            {
                anim.SetBool("Walking?", false);
            }
            sr.flipX = Input.GetAxis("Horizontal") < 0;
            var hs = (Input.GetAxis("Horizontal") * playerSpeed);
            rb.velocity = new Vector2(hs, rb.velocity.y);
            if (isGrounded())
            {
                if (hasJumped)
                {
                    anim.SetBool("Landing?", true);
                }

                numJumps = 0;
                hasJumped = false;
            }
            else if (numJumps == 0)
            { //walked off edge
                numJumps += 1;
            }
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            && numJumps < maxJumps)
            {
                anim.SetBool("Jumping?", true);
                numJumps += 1;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y <= 0 ? jumpVelocity : rb.velocity.y + jumpVelocity);
                hasJumped = true;
            }
        }
        else
        {
            anim.SetBool("Walking?", false);
        }

        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("Jump"));










    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2D.bounds.center,
             boxCollider2D.bounds.size, 0f, Vector2.down, .1f, platformslayerMask);
        //Debug.Log(raycastHit2d.collider);
        return raycastHit2d.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy" && other.otherCollider.tag != "AtkHitbox")
        {

            Vector3 dir = (player.transform.position - other.gameObject.transform.position).normalized;
            //Debug.Log(dir.y);
            if (dir.y < 0.65)
            {
                canMove = false;
                moveStopTime = Time.time;
                rb.velocity = new Vector2(dir.x * 12, 7);
                Invoke(nameof(EnableMove), 0.5f);
            }
            else
            {

                rb.velocity = new Vector2(0, 5);
                other.gameObject.GetComponent<Animator>().SetBool("Dead?", true);
                //
            }


        }

    }
    private void EnableMove()
    {
        canMove = true;
    }
}




