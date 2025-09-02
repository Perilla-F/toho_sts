using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardStateChangeEvent
{
    CardStateName StateName { get; }
    object Source { get; }
}
