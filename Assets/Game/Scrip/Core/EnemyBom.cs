using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBom : MonoBehaviour
{
    [SerializeField] private GameObject explosion; 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var a = Instantiate(explosion, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
        SimplePool.Despawn(this.gameObject);
      
    }
}
