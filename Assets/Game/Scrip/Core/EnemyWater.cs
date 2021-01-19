using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWater : MonoBehaviour
{
    [SerializeField] private GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.rigidbody != null)
        //{
        //    collision.rigidbody.AddForceAtPosition(new Vector2(1, 1)*0.002f, transform.position, ForceMode2D.Force);
           
        //}
        var a = Instantiate(explosion, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.identity);
            SimplePool.Despawn(this.gameObject);


    }
}
