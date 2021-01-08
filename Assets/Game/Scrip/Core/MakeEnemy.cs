using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeEnemy : MonoBehaviour
{
    public static MakeEnemy make;
    public GameObject enemy;
    public GameObject enemyBoom;
    public float timeSpawn;
    public int numberOfEnemy2;
    public List<GameObject> listEnemy;
    public bool wasBool;
    private void Awake()
    {
        make = this;
    }
    private void Start()
    {
        wasBool = true;
        StartCoroutine(_SpawnEnemy());
    }
    private IEnumerator _SpawnEnemy()
    {
        yield return new WaitForSeconds(1);
        
        if (GameContro.instance.coutnTime % 2 == 0 && GameContro.instance.coutnTime > 3 && wasBool == true)
        {
            int a = Random.Range(0, 5);
            if (a == 2 || a == 3)
            {
                int b = Random.Range(-1, 2);
                SimplePool.Spawn(enemyBoom, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameContro.instance.EnemyInGameContro.transform);
            }
            else
            {
                int i = Random.Range(-1, 2);
                SimplePool.Spawn(enemy, new Vector3(i, 5, 0), Quaternion.identity).transform.SetParent(GameContro.instance.EnemyInGameContro.transform);
            }                
         
            //listEnemy.Add(enemy.gameObject);
        }
        StartCoroutine(_SpawnEnemy());
    }

    public void _PauseSpawn()
    {
        wasBool = false;
        var a = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < a.Length; i++)
        {
            SimplePool.Despawn(a[i]);
        }
    }

    public void _ResuameSpawn()
    {
        wasBool = transform;
       
    }
}
