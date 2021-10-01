using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    [SerializeField]
    private bool eternal;
    void Start()
    {
        if (eternal) return;
        Destroy(transform.gameObject, 0.2f);   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bonus" && eternal) Destroy(other.gameObject);
    }

}
