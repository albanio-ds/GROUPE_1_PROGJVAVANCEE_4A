using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float Speed;

    [SerializeField]
    private string upName = "Up";

    [SerializeField]
    private string downName = "Down";

    [SerializeField]
    private string rightName = "Right";
    [SerializeField]
    private string leftName = "Left";

    [SerializeField]
    private string bombeName = "Bombe";

    [SerializeField]
    private GameObject bomb;

    [SerializeField]
    private LayerMask LayerMask;

    [SerializeField]
    private GameManager gameManager;

    private int bombLength = 2;


    public void resetValues()
    {
        bombLength = 2;
        Speed = 4;
    }
    void Update()
    {
     if (Input.GetButton(upName))
        {
            transform.position += Time.deltaTime * Speed * Vector3.forward;
        }

        if (Input.GetButton(downName))
        {
            transform.position += Time.deltaTime * Speed * Vector3.back;
        }

        if (Input.GetButton(rightName))
        {
            transform.position += Time.deltaTime * Speed * Vector3.right;
        }

        if (Input.GetButton(leftName))
        {
            transform.position += Time.deltaTime * Speed * Vector3.left;
        }

        if (Input.GetButtonDown(bombeName))
        {
            Vector3 positionToPut = new Vector3(Mathf.Round(transform.position.x), 0.5f, Mathf.Round(transform.position.z));
            if (!Physics.CheckSphere(positionToPut, 0.5f, LayerMask))
            {
               GameObject obj = Instantiate<GameObject>(bomb, positionToPut, Quaternion.identity);
                obj.GetComponent<BombeScript>().setBombSize(bombLength);
            }
        }
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
