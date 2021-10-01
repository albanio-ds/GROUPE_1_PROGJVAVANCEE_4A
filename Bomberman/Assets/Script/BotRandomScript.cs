using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRandomScript : MonoBehaviour
{
    [SerializeField]
    private float Speed = 5.0f;

    [SerializeField]
    private LayerMask LayerMask;

    [SerializeField]
    private GameManager gameManager;


    private int bombLength = 2;
    private int compteur = 0;
    private int action = 1;

    public void resetValues()
    {
        bombLength = 2;
        Speed = 4;
        compteur = 30;
    }

    [SerializeField]
    private GameObject bomb;
    void Update()
    {

            if (compteur == 0)
            {
                action = (int)Random.Range(0.0f, 10.0f);
                compteur = 30;
            }

            if (action == 1)
            {
                transform.position += Time.deltaTime * Speed * Vector3.forward;
            }

            if (action == 2)
            {
                transform.position += Time.deltaTime * Speed * Vector3.back;

            }

            if (action == 3)
            {
                transform.position += Time.deltaTime * Speed * Vector3.right;

            }

            if (action == 4)
            {
                transform.position += Time.deltaTime * Speed * Vector3.left;
            }

            if (action == 0 && (int)Random.Range(0.0f, 30.0f) > 25)
            {
            Vector3 positionToPut = new Vector3(Mathf.Round(transform.position.x), 0.5f, Mathf.Round(transform.position.z));
            if (!Physics.CheckSphere(positionToPut, 0.5f, LayerMask))
            {
                GameObject obj = Instantiate<GameObject>(bomb, positionToPut, Quaternion.identity);
                obj.GetComponent<BombeScript>().setBombSize(bombLength);
            }
            compteur = 1;
            }

        compteur--;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            gameManager.onPlayerDeath(transform.gameObject);
        }
    }

    public void SetPlayerBombSize(int size = 1)
    {
        if (bombLength + size >= 10) return;
        bombLength += size;
    }

    public void SetPlayerSpeed(float size = 0.5f)
    {
        if (Speed + size >= 6) return;
        Speed += size;
    }
}
