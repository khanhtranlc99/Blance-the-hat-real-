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

    public List<GameObject> listEnemy;
    public bool wasBool;
    private Coroutine boomCoroutine = null;
    private Coroutine waterCoroutine = null;

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
        yield return new WaitForSeconds(3.5f);
    
        if ( GameCoreManager.coreManager.coutnTime > 3 && wasBool == true)
        {
         
            float b = Random.Range(1, 2);
            SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
           
        }
        if( wasBool == true)
        {
            waterCoroutine = StartCoroutine(_SpawnWarter());
        }
        else
        {
            StopCoroutine(waterCoroutine);
        }
     

    }
    private IEnumerator _SpawnBoom()
    {
        yield return new WaitForSeconds(10);

        if ( GameCoreManager.coreManager.coutnTime > 8 && wasBool == true)
        {
       
            float i = Random.Range(-1, 2);
            SimplePool.Spawn(enemyBoom, new Vector3(i, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
        }
        if ( wasBool == true)
        {
            boomCoroutine = StartCoroutine(_SpawnBoom());
        }
        else
        {
            StopCoroutine(boomCoroutine);
        }
    }

        public void _spawnWood()
    {
       var a =  SimplePool.Spawn(wood, new Vector3(GameCoreManager.coreManager.main.transform.position.x, GameCoreManager.coreManager.main.transform.position.y + 2, 0), Quaternion.identity);
        a.transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
        GameCoreManager.coreManager.wood = a.gameObject;
    }
    
    public void _PauseSpawn()
    {
        StopAllCoroutines();
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
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
            waterCoroutine = null;
        }
        if (boomCoroutine != null)
        {
            StopCoroutine(boomCoroutine);
            boomCoroutine = null;
        }

        waterCoroutine =  StartCoroutine(_SpawnWarter());
        boomCoroutine =  StartCoroutine(_SpawnBoom());
   
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
