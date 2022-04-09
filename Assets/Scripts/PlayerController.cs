using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float playerSpeed = 7;
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
    private bool ignoreEnemy = false;

    // Start is called before the first frame update


    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        sr = player.GetComponent<SpriteRenderer>();
        boxCollider2D = player.GetComponent<BoxCollider2D>();
        anim = player.GetComponent<Animator>();
        // Debug.Log(GameInstance.getJumps());

    }

    // Update is called once per frame
    void Update()
    {
        //Attack();

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
                GameInstance.setJumps(GameInstance.getJumps() + 1);
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
            if (!ignoreEnemy)
            {
                if (dir.y < 0.65 && Mathf.Abs(dir.x) > 0.3)
                {
                    ignoreEnemy = true;
                    canMove = false;
                    moveStopTime = Time.time;
                    rb.velocity = new Vector2(dir.x * 12, 7);
                    Invoke(nameof(EnableMove), 0.5f);
                    Invoke(nameof(EnableEnemyHit), 0.5f);
                }
                else
                {
                    Debug.Log(Mathf.Abs(dir.x));
                    rb.velocity = new Vector2(0, 5);
                    other.gameObject.GetComponent<Collider2D>().enabled = false;
                    other.gameObject.GetComponent<Animator>().SetBool("Dead?", true);

                    //
                }
            }



        }

    }
    private void EnableMove()
    {
        canMove = true;
    }
    private void EnableEnemyHit()
    {
        ignoreEnemy = false;
    }

    private void Attack()
    {
        if (Input.GetKey(KeyCode.S))
        {
            atkHitbox.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            atkHitbox.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }
}




