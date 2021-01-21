using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnSmoke : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_DespwanSmoke());
    }

    private IEnumerator _DespwanSmoke()
    {
        yield return new WaitForSeconds(1);
        SimplePool.Despawn(this.gameObject);
    }    
}
