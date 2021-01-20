using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUI : MonoBehaviour
{
    int number;
    // Start is called before the first frame update
    private void Update()
    {
        number += 1;
        transform.eulerAngles = new Vector3(0, 0, number);
    }
}
