//#define TRACK_LONG_CALLS
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EventSynchronizer : MonoBehaviour
{
    //public static void Main()
    //{
    //    Debug.Log("Event Synchronizer Main");
    //    instance = instance ?? new GameObject("EventSynchronizer").AddComponent<EventSynchronizer>();
    //}
    private static EventSynchronizer instance;
    private static readonly ConcurrentQueue<Action> SyncronizationQueue = new ConcurrentQueue<Action>();
    private static readonly HashSet<Action> UpdateActions = new HashSet<Action>();
    private static readonly Dictionary<Action, bool> ActiveActions = new Dictionary<Action, bool>();
    private static readonly SortedList<float, Action> EventQueue = new SortedList<float, Action>();

    void Awake()
    {
        Debug.Log("Event Synchronizer Awake");

        instance = instance ?? this;
    }

    public static void Enqueue(Action action)
    {
        //Debug.Log("EventSynchronizer ||| Enqueue ||| " + action.GetHashCode());
        SyncronizationQueue.Enqueue(action);
    }

    public static void Enqueue(Action action, float time)
    {
        if (action != null) Enqueue(() => EnqueueWithoutSynchronization(time, action));
    }

    private static void EnqueueWithoutSynchronization(float time, Action action)
    {
        lock (EventQueue)
            if (EventQueue.ContainsKey(time))
            {
                //Debug.Log("EventSynchronizer ||| EnqueueWithoutSynchronization ||| Found Time Key: " + time + " ||| " + action.GetHashCode());
                EventQueue[time] += action;
            }
            else
            {
                EventQueue.Add(time, action);
            }
    }

    public static void AddUpdateListener(Action action)
    {
        try
        {
            ActiveActions[action] = true;
            UpdateActions.Add(action);
        }
        catch (Exception e)
        {
            Debug.LogError("Event Synchronizer Exception");
            Debug.LogException(e);
            throw;
        }
    }

    public static void RemoveUpdateListener(Action action)
    {
        if (UpdateActions.Contains(action))
        {
            ActiveActions[action] = false;
            UpdateActions.Remove(action);
        }
    }

#if TRACK_LONG_CALLS
    public static double longestCallsListTime = 0;
    public static double longestCallTime = 0;
    public static string longestCallDescription = "None";
    public static int callsCount = 0;
    int loggedAt = 50;
#endif

    void Update()
    {
#if TRACK_LONG_CALLS
        #region Tracking Long Calls
        Stopwatch totaltimer = new Stopwatch();
        totaltimer.Start();
        if (longestCallTime > 1)
            longestCallTime *= (1f - longestCallTime * 0.0001f);
        Action longestAction = null;
        callsCount = UpdateActions.Count;
        #endregion

        Stopwatch timer = new Stopwatch();
#endif
        var actions = UpdateActions.ToArray();
        foreach (var action in actions)
            if (action != null && ActiveActions[action])
                try
                {
#if TRACK_LONG_CALLS
                    timer.Restart();
#endif
                    action();

#if TRACK_LONG_CALLS
                    #region Tracking Long Calls
                    TimeSpan timeSpan = timer.Elapsed;
                    var callTime = timeSpan.TotalMilliseconds;
                    if (callTime > longestCallTime)
                    {
                        longestCallTime = callTime;
                        longestAction = action;
                    }


                    if (timeSpan.TotalMilliseconds > 15)
                    {
                        Debug.LogWarning(
                            "An update call takes too much time Elapsed: " + timeSpan.TotalMilliseconds +
                            " Method: " + action.Method + " Target: " + action.Target, this);
                    }

                    #endregion
#endif
                }
                catch (Exception e)
                {
                    Debug.LogError("\"Error in EventSynchronizer Method: \" + action.Method + \" Target: \" + action.Target", this);
                    Debug.LogException(e, this);
                }


#if TRACK_LONG_CALLS
        #region Tracking Long Calls
        var totalMs = totaltimer.Elapsed.TotalMilliseconds;
        totaltimer.Stop();

        if (longestAction != null)
            longestCallDescription = longestAction.Method + " in " + longestAction.Target + " (" + longestCallTime + " ms. )";

        if (totalMs > longestCallsListTime && totalMs > 15)
        {
            longestCallsListTime = totalMs;

            Debug.LogError("Longest Calls Loop Ever: " + totalMs + " slowest action: " + longestCallTime + " ms " + longestCallDescription);
        }

        totaltimer.Stop();
        #endregion

        if (actions.Length > loggedAt && loggedAt < 500)
        {
            loggedAt = actions.Length;
        }
        timer.Stop();
#endif
        lock (EventQueue)
            while (EventQueue.Count > 0 && EventQueue.Keys[0] < Time.time)
            {
                EventQueue[EventQueue.Keys[0]].Invoke();
                EventQueue.RemoveAt(0);
            }
    }


    void LateUpdate()
    {
        while (SyncronizationQueue.TryDequeue(out Action action))
        {
            //Debug.Log("EventSynchronizer ||| Dequeue ||| " + action.GetHashCode());
            action?.Invoke();
        }
    }
}
