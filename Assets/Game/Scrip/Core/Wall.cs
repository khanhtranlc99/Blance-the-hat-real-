using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals(Constant.ITEM_TAG))
        {
            this.PostEvent((int) EventID.GameLose);
            GameStateManager.WaitGameOver(null);
        }
    }
}
