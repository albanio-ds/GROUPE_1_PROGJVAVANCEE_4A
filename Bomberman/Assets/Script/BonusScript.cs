using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusScript : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(AutoDelete());
    }

    private IEnumerator AutoDelete()
    {
        yield return new WaitForSeconds(3);
        Destroy(transform.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMovementScript>().SetPlayerBombSize();
            other.GetComponent<BotRandomScript>().SetPlayerBombSize();
            Destroy(transform.gameObject);
        }
    }
}
