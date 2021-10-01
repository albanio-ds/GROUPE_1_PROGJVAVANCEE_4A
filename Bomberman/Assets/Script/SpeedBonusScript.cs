using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBonusScript : MonoBehaviour
{
    private void Start()
    {
        Destroy(transform.gameObject, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerMovementScript>().SetPlayerSpeed();
            other.GetComponent<BotRandomScript>().SetPlayerSpeed();
            Destroy(transform.gameObject);
        }
    }
}
