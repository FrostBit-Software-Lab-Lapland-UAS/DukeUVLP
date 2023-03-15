using System;
using UnityEngine;

public static class AirpumpEvents
{

    public static event Action onEventOne;
    public static void EventOne()
    {
        if (onEventOne != null)
        {
            onEventOne.Invoke();
        }
    }

    public static event Action onEventTwo;
    public static void EventTwo()
    {
        if (onEventTwo != null)
        {
            onEventTwo.Invoke();
        }
    }

    public static event Action onEventThree;
    public static void EventThree()
    {
        if (onEventThree != null)
        {
            onEventThree.Invoke();
        }
    }

    public static event Action onEventFour;
    public static void EventFour()
    {
        if (onEventFour != null)
        {
            onEventFour.Invoke();
        }
    }

    public static event Action onEventFive;
    public static void EventFive()
    {
        if (onEventFive != null)
        {
            onEventFive.Invoke();
        }
    }

    public static event Action onEventSix;
    public static void EventSix()
    {
        if (onEventSix != null)
        {
            onEventSix.Invoke();
        }
    }

    public static event Action onEventSeven;
    public static void EventSeven()
    {
        if (onEventSeven != null)
        {
            onEventSeven.Invoke();
        }
    }

    public static event Action onEventEight;
    public static void EventEight()
    {
        if (onEventEight != null)
        {
            onEventEight.Invoke();
        }
    }

}

