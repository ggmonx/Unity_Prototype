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


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        bc = this.GetComponent<BoxCollider2D>();
        platformslayerMask = LayerMask.GetMask("Platforms");
        lastPosition = rb.transform.position;
        lastCheckTime = Time.time;
    }


    // Update is called once per frame
    void Update()
    {

        if (!isGrounded() || ((Time.time - lastCheckTime) >= dirInterval && lastPosition == rb.transform.position))
        {

            direction *= -1;
            lastCheckTime = Time.time;
            lastPosition = rb.transform.position;

        }
        if (((Time.time - lastCheckTime) >= dirInterval))
        {// periodically update the position and check time
            lastCheckTime = Time.time;
            lastPosition = rb.transform.position;
        }


        rb.velocity = new Vector2(5 * (direction), rb.velocity.y);
        sr.flipX = rb.velocity.x < 0;

    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(bc.bounds.center,
             bc.bounds.size, 0f, Vector2.down + (sr.flipX ? Vector2.left : Vector2.right), 1f, platformslayerMask);
        Debug.DrawLine(bc.bounds.center, raycastHit2d.point + raycastHit2d.normal, Color.blue, 30, false);
        return raycastHit2d.collider != null;
    }


    private void Die()
    {
        Destroy(this.gameObject);
    }


}
