using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeBoxChirent : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }
}
