using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{

    [SerializeField] private CloneItem item;
    public void _OnClick()
    {
        Instantiate(item, new Vector3(0.08f, 3.23f, 0),Quaternion.identity);
        UI.uI.ChangeUI(UI.MenuUI.gamePlay);
    }
}
