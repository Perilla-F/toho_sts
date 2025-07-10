using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomFromList
{
    public static T GetRandom<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}

