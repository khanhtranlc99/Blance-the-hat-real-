using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimManager : MonoBehaviour
{
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private string landParam = "land";
    [SerializeField] private string idleParam = "idle";
    [SerializeField] private string dieParam = "die";
    [SerializeField] private string jumpParam = "jump";

    public void PlayLandAnim()
    {
        PlayAnim(landParam);
    }
    
    public void PlayIdleAnim()
    {
        PlayAnim(idleParam);
    }
    
    public void PlayDieAnim()
    {
        PlayAnim(dieParam);
    }
    
    public void PlayJumpAnim()
    {
        PlayAnim(jumpParam);
    }

    private void PlayAnim(string param)
    {
        itemAnimator.SetTrigger(param);
    }
}
