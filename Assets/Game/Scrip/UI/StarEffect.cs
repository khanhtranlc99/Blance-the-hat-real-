using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    public void _StarEffect()
    {
        GameContro.instance._PlusScore();
        this.gameObject.SetActive(false);
    }    
}
