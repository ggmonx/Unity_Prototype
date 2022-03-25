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


    private void Die()
    {
        Destroy(this.gameObject);
    }


}
