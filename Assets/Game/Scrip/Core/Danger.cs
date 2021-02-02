using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    [SerializeField] private GameObject bodyWater;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("danger");



        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Item")
        {
            Debug.Log("Outdanger");
            SimplePool.Spawn(bodyWater, new Vector2(1.262f, -0.73f), Quaternion.identity);
        }

    }
}
