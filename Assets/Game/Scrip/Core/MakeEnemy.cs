using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeEnemy : MonoBehaviour
{
    public static MakeEnemy make;
    public GameObject enemy;
    public GameObject enemyBoom;
    public GameObject enemyWater;
    public GameObject wood;
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
        //StartCoroutine(_SpawnEnemy());
    }
    private void Update()
    {
        if( Input.GetKeyDown(KeyCode.W))
        {
            _Water();
        }   
        if(Input.GetKeyDown(KeyCode.B))
        {
            _Boom();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            _spawnWood();
        }
    }
    private IEnumerator _SpawnWarter()
    {
        yield return new WaitForSeconds(1);
        if (GameCoreManager.coreManager.coutnTime % 2 == 0 && GameCoreManager.coreManager.coutnTime > 3 && wasBool == true)
        {
            yield return new WaitForSeconds(3);

            float b = Random.Range(-1, 2);
              SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
                 
        }
        StartCoroutine(_SpawnWarter());
    }
    private IEnumerator _SpawnBoom()
    {
        yield return new WaitForSeconds(1);
        if ( GameCoreManager.coreManager.coutnTime > 8 && wasBool == true)
        {
            yield return new WaitForSeconds(10);
            float i = Random.Range(-1, 2);
            SimplePool.Spawn(enemyBoom, new Vector3(i, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
        }
        StartCoroutine(_SpawnBoom());
    }

        public void _spawnWood()
    {
       var a =  SimplePool.Spawn(wood, new Vector3(GameCoreManager.coreManager.main.transform.position.x, GameCoreManager.coreManager.main.transform.position.y + 2, 0), Quaternion.identity);
        a.transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
        GameCoreManager.coreManager.wood = a.gameObject;
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
        wasBool = true;
        StartCoroutine(_SpawnWarter());
        StartCoroutine(_SpawnBoom());
    }
    private void _Water()
    {
        float b = Random.Range(-1, 2);
        SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
    }
    private void _Boom()
    {
        float i = Random.Range(-1, 2);
        SimplePool.Spawn(enemyBoom, new Vector3(i, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
    }
}
