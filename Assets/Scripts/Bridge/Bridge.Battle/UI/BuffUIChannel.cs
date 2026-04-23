using System;
using UnityEngine;

public static class BuffUIChannel
{
    public static Action<StatusEffect> OnBuffAdded;
    public static Action<StatusEffect> OnBuffUpdated;
}