using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonVoice : MonoBehaviour
{

    [SerializeField] GameObject onVoice;
    [SerializeField] GameObject offVoice;
    private void Start()
    {

        onVoice.SetActive(true);
        SoundManager.onVoice = true;
        offVoice.SetActive(false);
    }
    public void _OnClick()
    {
        if( SoundManager.onVoice == true)
        {
            onVoice.SetActive(false);
            SoundManager.onVoice = false;
            offVoice.SetActive(true);
        }
        else
        {
            onVoice.SetActive(true);
            SoundManager.onVoice = true;
            offVoice.SetActive(false);
        }
    }
}
