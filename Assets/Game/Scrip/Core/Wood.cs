using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            _SpawnSmoke();
            SoundManager.Play("masat");
        }
    }
    private void _SpawnSmoke()
    {
        Instantiate(GameCoreManager.coreManager.effect.gameObject, new Vector2(GameCoreManager.coreManager.clone.transform.position.x, GameCoreManager.coreManager.clone.transform.position.y - 0.5f), Quaternion.identity);
    }
}
