using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkHitBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.collider.tag);
        if (other.collider.tag == "Enemy")
        {
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            other.gameObject.GetComponent<Animator>().SetBool("Dead?", true);
            //Destroy(other.gameObject);
        }
        else if (other.collider.tag == "Player")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(),
            GetComponent<Collider2D>());
        }
    }
}
