using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWater : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    public float forceWater;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
        {
            SoundManager.Play("waterdrop");
            collision.rigidbody.AddForceAtPosition(new Vector2(forceWater, 50), transform.position);
            Debug.Log("forceWater" + forceWater);

        }
        var a = Instantiate(explosion, new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, 0), Quaternion.identity);
            SimplePool.Despawn(this.gameObject);


    }
}
