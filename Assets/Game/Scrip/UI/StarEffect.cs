using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarEffect : MonoBehaviour
{
    public void _StarEffect()
    {
        SoundManager.Play("coin");
        this.gameObject.SetActive(false);
    }    
}
