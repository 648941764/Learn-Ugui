using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void EventHandle(EventParam eventParm);
public class EventManager : MonoSingleton<EventManager>
{
    public event EventHandle EventHandle;
    public void Add(EventHandle eventHandle)
    {
        EventHandle += eventHandle;
    }

    public void Del(EventHandle eventHandle)
    {
        EventHandle -= eventHandle;
    }

    public void BroadCast(EventParam eventParm)
    {
        EventHandle?.Invoke(eventParm);
        EventParam.Release(eventParm);
    }
}

public class EventParam
{
    public EventType eventName;
    private List<object> _params = new List<object>();
    public static readonly Stack<EventParam> pool = new Stack<EventParam>();


    public EventParam Push(object param)
    {
        _params.Add(param);
        return this;
    }

    public T Get<T>(int index)
    {
        if ((uint)index < _params.Count)
        {
            return (T)_params[index];
        }
        return default(T);
    }

    public static EventParam Get(EventType eventType, params object[] eventParams)
    {
        EventParam eventParam = pool.Count > 0 ? pool.Pop() : new EventParam();
        eventParam.eventName = eventType;
        for (int i = 0; i < eventParams.Length; i++)
        {
            eventParam.Push(eventParams[i]);
        }
        return eventParam;
    }

    public static void Release(EventParam eventParam)
    {
        eventParam.eventName = EventType.None;
        eventParam._params.Clear();
        pool.Push(eventParam);
    }

}

public enum EventType 
{
    None,
    Example,
    OnTextChange,
}

public sealed class EventRegistry 
{
    private readonly List<EventHandle> _eventHandles = new List<EventHandle>();

    public void AddEvent(EventHandle eventHandle) 
    {
        _eventHandles.Add(eventHandle);
    }

    public void DelEvent(EventHandle eventHandle)
    {
        _eventHandles.Remove(eventHandle);
    }

    public void AddEvents()
    {
        int i = -1;
        while (++i < _eventHandles.Count)
        {
            EventManager.Instance.Add(_eventHandles[i]);

        }
    }

    public void DelEvents()
    {
        int i = -1;
        while (++i < _eventHandles.Count)
        {
            EventManager.Instance.Del(_eventHandles[i]);
        }
    }

    public void ClearEvens()
    {
        int i = -1;
        while (++i < _eventHandles.Count)
        {
            EventManager.Instance.Del(_eventHandles[i]);
        }
        _eventHandles.Clear();
    }
}


