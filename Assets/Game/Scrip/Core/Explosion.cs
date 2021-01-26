using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public float forceEplositon;
    
    void Start()
    {
        StartCoroutine(_Destroy());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
           collision.attachedRigidbody.AddForceAtPosition(new Vector2(forceEplositon * EnemyBom.enemyBom.valua, 100), transform.position);
            Debug.Log("forceOfExplosion" + forceEplositon * EnemyBom.enemyBom.valua);
        }
        Debug.Log("forceOfExplosion" + forceEplositon * EnemyBom.enemyBom.valua);
    }
    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        GameCoreManager.coreManager.earthWake = false; 
        Destroy(this.gameObject);
    }    
}
