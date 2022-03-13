using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    private float maxVelocityCap = 2;
    public float jumpVelocity;
    public int maxJumps;
    private int numJumps = 0;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer sr;
    public GameObject player;
    private Animator anim;

    public LayerMask platformslayerMask;
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
        sr.flipX = Input.GetAxis("Horizontal") < 0;
        if (isGrounded())
        {
            numJumps = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && numJumps < maxJumps)
        {
            //anim.SetBool("Attacking?", true);
            numJumps += 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
        }
        else
        {
            //anim.SetBool("Attacking?", false);
        }
        rb.velocity = new Vector2((Input.GetAxis("Horizontal") * 1.5f * playerSpeed), rb.velocity.y);



    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2D.bounds.center,
             boxCollider2D.bounds.size, 0f, Vector2.down, .1f, platformslayerMask);
        Debug.Log(raycastHit2d.collider);
        return raycastHit2d.collider != null;
    }



}
