using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : MonoBehaviour {
    public float aceleracao = 500f;

    private Transform trans;


    // Use this for initialization
    void Start () {
        trans = transform;
        StartCoroutine(Count());
    }

    // Update is called once per frame
    void Update () {
        trans.position += trans.forward * aceleracao * Time.deltaTime * 1f;

    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }

}
