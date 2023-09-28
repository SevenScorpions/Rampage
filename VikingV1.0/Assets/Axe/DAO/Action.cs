using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimState
{
    Attack,
    Move,
    Idle
}
public enum AnimTrigger
{
    Float,
    Int,
    Bool,
    Trigger
}
public class Action
{
    
    public string ParameterName;
    public float AnimLength;
    public float AnimExit;
    public AnimState State;
    public AnimTrigger Trigger;


    private float LastUsed;
    public Action(string parameterName, AnimState state,AnimTrigger trigger)
    {
        this.ParameterName = parameterName;
        this.Trigger = trigger;
        this.State = state;
    }
    public Action(string parameterName, float animLength, float animExit, AnimState state, AnimTrigger trigger)
    {
        ParameterName = parameterName;
        AnimLength = animLength;
        AnimExit = animExit;
        State = state;
        Trigger = trigger;
    }
    public void act()
    {
        LastUsed = Time.time;
    }
    public bool isReady()
    {
        return Time.time-LastUsed>AnimLength;
    }
    public bool isReadyForNext()
    {
        return Time.time -LastUsed < AnimExit && isReady();
    }
    public bool isDone()
    {
        return Time.time - LastUsed > AnimExit;
    }
}
