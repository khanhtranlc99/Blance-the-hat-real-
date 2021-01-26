using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_Destroy());
    }
    private IEnumerator _Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        GameCoreManager.coreManager.earthWake = false; 
        Destroy(this.gameObject);
    }    
}
