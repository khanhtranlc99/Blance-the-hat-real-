using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEnemy : MonoBehaviour
{
    void Start()
    {
     
    }
    public void _Destroy()
    {
        GameCoreManager.coreManager._GotoInit();
        SimplePool.Despawn(this.gameObject);
    }
   

}
