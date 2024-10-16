using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventDispatcher : Singleton<EventDispatcher>
{
    Dictionary<EventID, Action<object>> _listeners = new Dictionary<EventID, Action<object>>();

    public void RegisterListener(EventID eventID, Action<object> callback)
    {
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] += callback;
        }
        else
        {
            _listeners.Add(eventID, null);
            _listeners[eventID] += callback;
        }
    }

    public void PostEvent(EventID eventID, object param = null)
    {
        if (!_listeners.ContainsKey(eventID))
        {
            return;
        }
        var callbacks = _listeners[eventID];
        if (callbacks != null)
        {
            callbacks(param);
        }
        else
        {
            _listeners.Remove(eventID);
        }
    }

    public void RemoveListener(EventID eventID, Action<object> callback)
    {
        if (_listeners.ContainsKey(eventID))
        {
            _listeners[eventID] -= callback;
        }
    }

    public void ClearAllListener()
    {
        _listeners.Clear();
    }
}

#region Extension Class
public static class EventDispatcherExtension
{
    public static void RegisterListener(this MonoBehaviour listener, EventID eventID, Action<object> callback)
    {
        EventDispatcher.Ins.RegisterListener(eventID, callback);
    }

    public static void PostEvent(this MonoBehaviour listener, EventID eventID, object param)
    {
        EventDispatcher.Ins.PostEvent(eventID, param);
    }

    public static void PostEvent(this MonoBehaviour sender, EventID eventID)
    {
        EventDispatcher.Ins.PostEvent(eventID, null);
    }
}
#endregion
