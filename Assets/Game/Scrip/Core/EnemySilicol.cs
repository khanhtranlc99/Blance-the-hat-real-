using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySilicol : MonoBehaviour
{
    public float forceBalls;
    [SerializeField] SpriteRenderer spriteRender;
    [SerializeField] Sprite[] sprites;
    private void Start()
    {
        var i = Random.Range(0, sprites.Length);
        spriteRender.sprite = sprites[i];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
        {
            SoundManager.Play("Ballsdrop");
            collision.rigidbody.AddForceAtPosition(new Vector2(forceBalls, 50), transform.position);
            Debug.Log("forceBalls" + forceBalls);
        }
        if(collision.gameObject.tag == "wall")
        {
            SimplePool.Despawn(this.gameObject);
        }    
        
    }
}
