using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        yield return new WaitForSeconds(Random.Range(10,20));
        Destroy(this.gameObject);
    }
}
