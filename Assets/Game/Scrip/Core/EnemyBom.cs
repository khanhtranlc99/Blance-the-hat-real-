using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBom : MonoBehaviour
{
    [SerializeField] private GameObject explotion;
    private void Start()
    {

    }
  
    public IEnumerator _Bom1()
    {
        yield return new WaitForSeconds(0.5f);
        SimplePool.Despawn(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        explotion.gameObject.SetActive(true);
        StartCoroutine(_Bom1());
    }
  
}
