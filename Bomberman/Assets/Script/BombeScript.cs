using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombeScript : MonoBehaviour
{

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private float delay = 1.0f;

    [SerializeField]
    private int bombLength = 2;

    //[SerializeField]
    private AudioSource son;

    private MeshRenderer mesh;

    private BoxCollider collider;

    [SerializeField]
    private LayerMask LayerMask;

    [SerializeField]
    private ParticleSystem particle;

    public void setBombSize(int value)
    {
        bombLength = value;
    }
    void Start()
    {
        mesh = transform.GetComponent<MeshRenderer>();
        collider = transform.GetComponent<BoxCollider>();
        son = transform.GetComponent<AudioSource>();
        StartCoroutine(Explosion());
    }


    private void recursiveBomb(Vector3 initialPosition, Vector3 vectorDirection)
    {

        for (int i=1; i< bombLength; i++)
        {
            if (!Physics.CheckSphere(initialPosition + (vectorDirection * i), 0.4f, LayerMask))
            {
                Instantiate<GameObject>(explosion.gameObject, initialPosition + (vectorDirection * i), Quaternion.identity);
            } else
            {
                break;
            }
            
        }

        if (vectorDirection == Vector3.forward)
        {
            recursiveBomb(initialPosition, Vector3.back);
        }
        if (vectorDirection == Vector3.left)
        {
            recursiveBomb(initialPosition, Vector3.forward);
        }
        if (vectorDirection == Vector3.right)
        {
            recursiveBomb(initialPosition, Vector3.left);
        }
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(delay);
        Vector3 positionBomb = transform.position;
        Instantiate<GameObject>(explosion.gameObject, positionBomb, Quaternion.identity);
        recursiveBomb(positionBomb, Vector3.right);
        particle.gameObject.SetActive(false);
        mesh.enabled = false;
        collider.enabled = false;
        //son.enabled = true;
        son.Play();
        yield return new WaitForSeconds(1.0f);
        Destroy(transform.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.GetComponent<BoxCollider>().isTrigger = false;
    }
}
