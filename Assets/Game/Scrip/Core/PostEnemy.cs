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
        
        SimplePool.Despawn(this.gameObject);
    }

}
