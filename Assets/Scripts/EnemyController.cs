using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject enemy;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Vector3 dir = (other.gameObject.transform.position - enemy.transform.position).normalized;
            var rb = other.gameObject.GetComponent<Rigidbody2D>();
            if (dir.y > 0)
            {
                // hit top
                rb.velocity = new Vector2(0, 5);
                Destroy(enemy);

            }

        }

    }
}
