using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyWater : MonoBehaviour
{
    private Coroutine waterBodyCoroutine = null;
    private void Start()
    {
        if (waterBodyCoroutine != null)
        {
            StopCoroutine(waterBodyCoroutine);
            waterBodyCoroutine = null;
        }
        waterBodyCoroutine = StartCoroutine(_BodyWater());
    }
     private IEnumerator _BodyWater()
    {
        yield return new WaitForSeconds(1);
        SimplePool.Despawn(this.gameObject); 


    }
}
