using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject EnemyPrefab;

    void Start()
    {
        GameObject parent =  new GameObject("Enemy List");

        for(int i = 0; i < 10; i++)
        {
            GameObject Obj = Instantiate(EnemyPrefab);
            Obj.transform.name = "Enemy";
            Obj.transform.SetParent(parent.transform);

            //Obj.AddComponent<HitPointController>();

            Obj.transform.position = new Vector3(
                Random.Range(-15.0f, 15.0f), 0.0f, Random.Range(-15.0f, 15.0f));
        }
    }
}
