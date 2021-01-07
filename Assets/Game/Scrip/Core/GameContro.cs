using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameContro : MonoBehaviour
{
   
    [SerializeField] private GameObject main;  
    [SerializeField] private CloneItem[] cloneItem;
   

private void Start()
    {

        //rigidMain.GetComponent<Rigidbody2D>();
        //_MadeItem();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
   
        }
    }

    private void _Reset()
    {   
        main.transform.DORotate(new Vector3(0, 0, 34.876f), 0.1f);
    }
    
}
