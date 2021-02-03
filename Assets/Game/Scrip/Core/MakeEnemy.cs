using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MakeEnemy : MonoBehaviour
{
    public static MakeEnemy make;
    public EnemyBom enemyBoom;
    public EnemyWater enemyWater;
    public EnemySilicol enemySilicol; 
    public GameObject wood;
    public float timeSpawn;
    public List<GameObject> listEnemy;
    public bool wasBool;
    private Coroutine boomCoroutine = null;
    private Coroutine waterCoroutine = null;
    private Coroutine ballCoroutine = null; 
    [SerializeField] private float timeDropWater;
    [SerializeField] private float timeDropBoom;
    [SerializeField] private float timeDropBall;
    [SerializeField] GameObject postE;
    [SerializeField] Player scripPlayer;
    [SerializeField] Shake scripShake;
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
        if(Input.GetKeyDown(KeyCode.S))
        {
            _Balls();
        }    
      
    }
    /// <summary>
    ///  mode normal
    /// </summary>
    /// <returns></returns> 
    private IEnumerator _SpawnWarter()
    {
        yield return new WaitForSeconds(timeDropWater);
    
        if (wasBool == true)
        {
         
            float b = Random.Range(-1, 2);
            
            SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
            if (GameCoreManager.coreManager.coutnTime < 10)
            {
                timeDropWater = 3.5f;
            }
            if (GameCoreManager.coreManager.coutnTime < 15 && GameCoreManager.coreManager.coutnTime > 10)
            {
                timeDropWater = 2;
            }
            if (GameCoreManager.coreManager.coutnTime < 19 && GameCoreManager.coreManager.coutnTime > 15)
            {
                timeDropWater = 1.5f;
            }
            if (GameCoreManager.coreManager.coutnTime < 20 && GameCoreManager.coreManager.coutnTime > 19)
            {
                timeDropWater = 1;
            }
            if (GameCoreManager.coreManager.coutnTime > 25)
            {
                timeDropWater = 0.5f;
            }
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
         
            if (GameCoreManager.coreManager.coutnTime > 25)
            {
                timeDropWater = 5;
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

    /// <summary>
    /// mode Wood
    /// </summary>
    public void _spawnWood()
    {
       var a =  SimplePool.Spawn(wood, new Vector3(GameCoreManager.coreManager.main.transform.position.x, GameCoreManager.coreManager.main.transform.position.y + 2, 0), Quaternion.identity);
        a.transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
        GameCoreManager.coreManager.wood = a.gameObject;
    }

    /// <summary>
    /// mode BoomReject
    /// </summary>

    private IEnumerator _BoomReject() //no boom
    {   
        yield return new WaitForSeconds(timeDropWater);
        if (wasBool == true && GameCoreManager.coreManager.coutnTime > 5 )
        {             
            float b = Random.Range(-1, 2);
            SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);         
        }
        if (wasBool == true)
        {
            waterCoroutine = StartCoroutine(_BoomReject());
        }
        else
        {
            StopCoroutine(waterCoroutine);
        }
    }
    /// <summary>
    /// mode RubberBalls
    /// </summary>

    private IEnumerator _RubberBall()
    {
        yield return new WaitForSeconds(timeDropBall);

        if (wasBool == true && GameCoreManager.coreManager.coutnTime > 4 )
        {

            float b = Random.Range(-1, 2);
            SimplePool.Spawn(enemySilicol, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
           
            if (GameCoreManager.coreManager.coutnTime > 11)
            {
                timeDropBall = 0.3f;
            }
        }
        if (wasBool == true)
        {
            ballCoroutine = StartCoroutine(_RubberBall());

        }
        else
        {
            StopCoroutine(ballCoroutine);
        }

    }
    /// <summary>
    /// mode Shake
    /// </summary>
    public  void _OnShake()
    {
        if( scripPlayer.enabled == true)
        {
            scripPlayer.enabled = false;
            scripShake.enabled = true;
        }
        Debug.Log("Shake");
    }
    public void _OffShake()
    {
        if (scripPlayer.enabled == false)
        {
            scripPlayer.enabled = true;
            scripShake.enabled = false;
        }
    }
    /// <summary>
    /// mode WaterDrop
    /// </summary>
    private IEnumerator _WarterDrop()
    {
        yield return new WaitForSeconds(timeDropWater);

        if (wasBool == true && GameCoreManager.coreManager.coutnTime >3)
        {

            float b = Random.Range(-1, 2);

            SimplePool.Spawn(enemyWater, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
            if (GameCoreManager.coreManager.coutnTime < 10)
            {
                timeDropWater = 0.5f;
            }
            if ( GameCoreManager.coreManager.coutnTime > 10)
            {
                timeDropWater = 0.3f;
            }
        }
        if (wasBool == true)
        {
            waterCoroutine = StartCoroutine(_WarterDrop());

        }
        else
        {
            StopCoroutine(waterCoroutine);
        }



    }



    /// <summary>
    /// Tool
    /// </summary>
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
    
    public void _ModeNormal()
    {    
        wasBool = true;
        timeDropWater = 3.5f;
        timeDropBoom = 10;
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
    public void _ModeBoomReject()
    {
        Debug.Log("BoomReject");
        wasBool = true;
        timeDropWater = 1f;
        if (waterCoroutine != null)
        {
            StopCoroutine(waterCoroutine);
            waterCoroutine = null;
        }
        waterCoroutine = StartCoroutine(_BoomReject());
    }
    public void _ModeWaterDrop()
    {
        wasBool = true;
        timeDropWater = 1f;
        timeDropBoom = 10;
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
        waterCoroutine = StartCoroutine(_WarterDrop());
        boomCoroutine = StartCoroutine(_SpawnBoom());
    }
    public void _ModeRumbelBall()
    {
        wasBool = true;
        timeDropBall = 0.5f;
        if (ballCoroutine != null)
        {
            StopCoroutine(ballCoroutine);
            ballCoroutine = null;
        }
        ballCoroutine = StartCoroutine(_RubberBall());
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
        else if (b == 1 || b == 2)
        {

            SimplePool.Spawn(enemyBoom, new Vector3(a - 0.65f, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);
            enemyBoom.valua = 1;
            Debug.Log("1");
        }
    }
    public void _Balls()
    {
        float b = Random.Range(-1, 2);
        SimplePool.Spawn(enemySilicol, new Vector3(b, 5, 0), Quaternion.identity).transform.SetParent(GameCoreManager.coreManager.EnemyInGameContro.transform);

    }
    private void OnGameLoseHandler(object param)
    {
        make.wasBool = false;
    }
}
//Normal = 0,
//    Wood = 1,
//    BoomReject = 2,
//    RubberBalls = 3,
//    Shake = 4,
//    WaterDrop = 5