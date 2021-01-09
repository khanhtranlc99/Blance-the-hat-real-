using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUI : MonoBehaviour
{

    [SerializeField] private CloneItem item;
    public void _OnClick()
    {
        var a = Instantiate(item, new Vector3(0.08f, 3.23f, 0),Quaternion.identity);
        a.transform.SetParent(GameContro.instance.itemInGameContro.transform);
        GameContro.instance.clone = a;
        GameContro.instance._Reset();
        UI.uI.ChangeUI(UI.MenuUI.gamePlay);
    }
}
