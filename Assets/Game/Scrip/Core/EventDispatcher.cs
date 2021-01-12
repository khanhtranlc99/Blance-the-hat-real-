using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
    private static EventDispatcher localInstance;
    public static EventDispatcher Instance
    {
        get
        {
            if (localInstance == null)
            {
                GameObject singletonObject = new GameObject();
                localInstance = singletonObject.AddComponent<EventDispatcher>();
                singletonObject.name = "Singleton - EventDispatcher";
                Debug.Log($"Create singleton : {singletonObject.name}");
            }

            return localInstance;
        }

        set { }
    }

    private Dictionary<EventID, Action<object>> listeners = new Dictionary<EventID, Action<object>>();

    public void RegisterListener(EventID ev, Action<object> callback)
    {
        if (listeners.ContainsKey(ev))
        {
            listeners[ev] += callback;
        } else
        {
            listeners.Add(ev, null); // ? why not add directly
            listeners[ev] += callback;
        }
    }

    public void FireEvent(EventID ev, object param = null)
    {
        if (!listeners.ContainsKey(ev))
        {
            Debug.Log("No listener for this event");
            return;
        }

        var callbacks = listeners[ev];

        if (callbacks != null)
        {
            callbacks(param);
        } else
        {
            Debug.Log($"Fire event {ev}, but no listener. Delete event {ev} instead !");
            listeners.Remove(ev);
        }
    }

    public void RemoveListener(EventID ev, Action<object> callback)
    {
        if (listeners.ContainsKey(ev))
        {
            listeners[ev] -= callback;
        } else
        {
            Debug.Log($"{ev} not existed. ");
        }
    }

    public void ClearListeners()
    {
        listeners.Clear();
    }

    private void OnDestroy()
    {
        if (localInstance == this)
        {
            ClearListeners();
            localInstance = null;
        }
    }
}



public static class EventDispatcherExtension
{
    /// Use for registering with EventsManager
    public static void RegisterListener(this MonoBehaviour listener, EventID ev, Action<object> callback)
    {
        EventDispatcher.Instance.RegisterListener(ev, callback);
    }

    /// Post event with param
    public static void FireEvent(this MonoBehaviour listener, EventID ev, object param = null)
    {
        EventDispatcher.Instance.FireEvent(ev, param);
    }

    /// Post event with no param (param = null)
    public static void RemoveListener(this MonoBehaviour sender, EventID ev)
    {
        EventDispatcher.Instance.FireEvent(ev);
    }
}
