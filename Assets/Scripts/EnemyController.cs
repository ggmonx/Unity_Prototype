using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D bc;
    private LayerMask platformslayerMask;
    private int direction = 1;
    private Vector3 lastPosition;
    private const float dirInterval = 0.5f;
    private float lastCheckTime;
    private Animator animator;
    private bool shouldTurn = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        bc = this.GetComponent<BoxCollider2D>();
        animator = this.GetComponent<Animator>();
        platformslayerMask = LayerMask.GetMask("Platforms");
        lastPosition = rb.transform.position;
        lastCheckTime = Time.time;
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isGrounded() || lastPosition == rb.transform.position || shouldTurn)
        {
            shouldTurn = false;
            direction *= -1;
            lastCheckTime = Time.time;
            lastPosition = rb.transform.position;

        }
        else if (((Time.time - lastCheckTime) >= dirInterval))
        {// periodically update the position and check time
            lastCheckTime = Time.time;
            lastPosition = rb.transform.position;
        }

        if (animator.GetBool("Dead?") == false)
        {
            rb.velocity = new Vector2(5 * (direction), rb.velocity.y);
            sr.flipX = rb.velocity.x < 0;
        }
        else
        {
            rb.gravityScale = 0;
        }


    }

    private bool isGrounded()
    {
        const float extentMultiplier = 1.8f;
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(bc.bounds.center +
         new Vector3(bc.bounds.extents.x * (sr.flipX ? -extentMultiplier : extentMultiplier), 0, 0),
             bc.bounds.size, 0f, Vector2.down + (sr.flipX ? Vector2.left : Vector2.right), 0.1f, platformslayerMask);

        return raycastHit2d.collider != null;
    }


    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.tag == "Enemy")
        {

            shouldTurn = true;
        }
        else if (other.collider.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.collider, other.otherCollider, true);
            IEnumerator co = enableCollision(other.collider, other.otherCollider);
            StartCoroutine(co);

        }
    }

    IEnumerator enableCollision(Collider2D col1, Collider2D col2)
    {

        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(col1, col2, false);
    }


}
