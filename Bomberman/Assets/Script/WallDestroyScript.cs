using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDestroyScript : MonoBehaviour
{
    [SerializeField]
    private GameObject debris;

    private MeshRenderer mesh;

    private BoxCollider collider;

    [SerializeField]
    private GameObject bonusBomb;

    [SerializeField]
    private GameObject bonusSpeed;

    void Start()
    {
        mesh = transform.GetComponent<MeshRenderer>();
        collider = transform.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish") StartCoroutine(Explosion());

    }

    private IEnumerator Explosion()
    {
        mesh.enabled = false;
        collider.enabled = false;
        float randomNumber = Random.Range(0.0f, 10.0f);

        if (randomNumber > 7.0f)
        {
         if (randomNumber > 9.0f) Instantiate<GameObject>(bonusSpeed, transform.position, Quaternion.identity).transform.rotation = bonusSpeed.transform.rotation;
         else {
                Instantiate<GameObject>(bonusBomb, transform.position, Quaternion.identity);
                }
        }
        GameObject obj = Instantiate<GameObject>(debris, transform);
        obj.transform.position = transform.position;
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.gameObject);
    }
}
