using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneItem : MonoBehaviour
{
    [SerializeField] private  SpriteRenderer Rsprite;
    [SerializeField] private Rigidbody2D rigidbody2D;
    public bool wasJump;
    private void Start()
    {
      if(wasJump == true)
        {
            StartCoroutine(_Jump());
        }
    }
    //public PolygonCollider2D polygon;
  

  private IEnumerator _Jump()
    {
        yield return new WaitForSeconds(1);
        int a = Random.Range(-1, 2);
        rigidbody2D.AddForce(new Vector3(20*a, 100, 0));
        StartCoroutine(_Jump());
        Debug.Log("J");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.tag == "wall")
        {
            UI.uI.ChangeUI(UI.MenuUI.menu);
            Destroy(gameObject);
        }
    }
}
