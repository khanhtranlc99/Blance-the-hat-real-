using System;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField]
    private UITextNumber number = null;
    public static UITextNumber Number { get => instance?.number; }


    [SerializeField]
    private ParticleLockAt particle = null;
    public static ParticleLockAt Particle { get => instance?.particle; }

    //public Transform testingTfFrom;
    public Transform defaultTarget;
    public static int totalCoin
    {
        get => DataManager.UserData.totalCoin;
        private set => DataManager.UserData.totalCoin = value;
    }

    private static CoinManager instance;

    private void Awake()
    {
        instance = this;
        DataManager.OnLoaded += DataManager_OnLoaded;
    }

    private void DataManager_OnLoaded(GameData gameData)
    {
        Number.DOAnimation(0, totalCoin, 0);
    }

    public static void Add(int numb, Transform fromTrans = null, Transform toTrans = null)
    {
        var current = totalCoin;
        totalCoin += numb;
        if (Number != null)
        {
            if (numb > 0)
            {
                if (fromTrans)
                {
                    Particle.Emit(numb + 1, fromTrans, toTrans ?? instance.defaultTarget);
                }
                Number.DOAnimation(current, totalCoin, Particle == null ? 0.5f : Particle.StartLifetime * 0.5f);
            }
            else
            {
                Number.DOAnimation(current, totalCoin, 0);
            }
        }
    }

    public void Test(int numb)
    {
        Add(numb);
    }
}
