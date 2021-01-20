using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaDownWater : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(_Destroy());
    }
    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //collision.rigidbody.AddForceAtPosition(new Vector2(1, 1) * 100, transform.position, ForceMode2D.Force);

    }
}
