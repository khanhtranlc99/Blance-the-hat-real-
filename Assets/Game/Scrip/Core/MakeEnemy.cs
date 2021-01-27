using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MakeEnemy : MonoBehaviour
{
    public static MakeEnemy make;
    public GameObject enemy;
    public EnemyBom enemyBoom;
    public EnemyWater enemyWater;
    public GameObject wood;
    public float timeSpawn;

    public List<GameObject> listEnemy;
    public bool wasBool;
    private Coroutine boomCoroutine = null;
    private Coroutine waterCoroutine = null;
    [SerializeField] float timeDropWater;
    [SerializeField] float timeDropBoom;
    [SerializeField] GameObject postE;

    private void OnEnable()
    {
        this.RegisterListener((int) EventID.GameLose, OnGameLoseHandler);
    }

    private void OnDisable()
    {
        EventDispatcher.Instance?.RemoveListener((int) EventID.GameLose, OnGameLoseHandler);
    }

    private void Awake()
    {
        make = this;
    }
    private void Start()
    {
        wasBool = true;
        timeDropWater = 3.5f;
        timeDropBoom = 10;


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
        yield return new WaitForSeconds(timeDropWater);
    
        if (wasBool == true)
        {
         
            float b = Random.Range(-1, 2);
            
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
        yield return new WaitForSeconds(timeDropBoom);

        if (  wasBool == true)
        {

            var a = GameCoreManager.coreManager.clone.transform.position.x;
            var b = Random.Range(0, 2);
            if(b == 0)
            {
               
                SimplePool.Spawn(enemyBoom, new Vector3(a + 0.65f, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
                enemyBoom.valua = -1;
            }
            else
            {
               
                SimplePool.Spawn(enemyBoom, new Vector3(a - 0.65f, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
                enemyBoom.valua = 1;
            }
           
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
    public void _Water()
    {
        float b = Random.Range(-1, 2);
        SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
    }
    public void _Boom()
    {

        var a = GameCoreManager.coreManager.clone.transform.position.x;
        var b = Random.Range(0, 2);
        if (b == 0)
        {

            SimplePool.Spawn(enemyBoom, new Vector3(a + 0.65f, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
            enemyBoom.valua = -1;
            Debug.Log("-1");
        }
        else if(b == 1 || b == 2)
        {

            SimplePool.Spawn(enemyBoom, new Vector3(a - 0.65f, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
            enemyBoom.valua = 1;
            Debug.Log("1");
        }
    }

    private void OnGameLoseHandler(object param)
    {
        make.wasBool = false;
    }
}
