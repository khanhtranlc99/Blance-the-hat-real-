using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    public static GameManager gameManager;
    private void Awake()
    {
        gameManager = this;
    }


}

