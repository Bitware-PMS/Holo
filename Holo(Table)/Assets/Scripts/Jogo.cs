using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogo : MonoBehaviour {

    public GameObject tiro;
    public GameObject tiroSpawn;
    public GameObject inimigo;

    public float aceleracao = 50f;
    public float virar = 60f;

    private Transform trans;

    // Use this for initialization
    void Start () {
        trans = transform;
    }

    // Update is called once per frame
    void Update () {
        virarNave();
        acelarar();
        if (Input.GetKeyDown("space"))
            disparar();
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(0, 2));
        Vector3 newEnemy = this.gameObject.transform.position + new Vector3(Random.Range(-50, 50), 0, Random.Range(-50, 50));
        if (!Physics.CheckSphere(newEnemy, 25.0f))
            Instantiate(inimigo, newEnemy, Quaternion.identity);
    }

    private void acelarar()
    {
        trans.position += trans.forward * aceleracao * Time.deltaTime * 0.5f;
    }

    private void virarNave()
    {
        trans.Rotate(0, virar * Time.deltaTime * Input.GetAxis("Horizontal"),0);
    }

    private void disparar()
    {
        GameObject tiro1 = Instantiate(tiro, tiroSpawn.gameObject.transform.position, Quaternion.identity);
        tiro1.transform.rotation = this.gameObject.transform.rotation;
    }
}
