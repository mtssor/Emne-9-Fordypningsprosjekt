﻿using RogueSharp_Tutorial.Interfaces;
using RogueSharp;

namespace RogueSharp_Tutorial.Systems;

public class SchedulingSystem
{
    private int _time;
    private readonly SortedDictionary<int, List<IScheduleable>> _scheduleables ;

    public SchedulingSystem()
    {
        _time = 0;
        _scheduleables = new SortedDictionary<int, List<IScheduleable>>();
    }
    
    // add a new object to the schedule
    // place it at the current time plus the objects time property
    public void Add(IScheduleable scheduleable)
    {
        int key = _time + scheduleable.Time;
        if (!_scheduleables.ContainsKey(key))
        {
            _scheduleables.Add(key, new List<IScheduleable>());
        }
        _scheduleables[key].Add(scheduleable);
    }
    
    // remove a specific object from the schedule
    // useful for when a monster is killed to remove it before its action comes up again
    public void Remove(IScheduleable scheduleable)
    {
        KeyValuePair<int, List<IScheduleable>> scheduleableListFound =
            new KeyValuePair<int, List<IScheduleable>>(-1, null);

        foreach (var scheduleablesList in _scheduleables)
        {
            if (scheduleablesList.Value.Contains(scheduleable))
            {
                scheduleableListFound = scheduleablesList;
                break;
            }
        }

        if (scheduleableListFound.Value != null)
        {
            scheduleableListFound.Value.Remove(scheduleable);
            if (scheduleableListFound.Value.Count <= 0)
            {
                _scheduleables.Remove(scheduleableListFound.Key);
            }
        }
    }
    
    // get the next object whose turn it is from the schedule. Adcane time if necessary
    public IScheduleable Get()
    {
        var firstScheduleableGroup = _scheduleables.First();
        var firstScheduleable = firstScheduleableGroup.Value.First();
        Remove(firstScheduleable);
        _time = firstScheduleableGroup.Key;
        return firstScheduleable;
    }
    
    // get the current time (turn) for the schedule
    public int GetTime()
    {
        return _time;
    }
    
    // reset the time and clear the schedule
    public void Clear()
    {
        _time = 0;
        _scheduleables.Clear();
    }
}