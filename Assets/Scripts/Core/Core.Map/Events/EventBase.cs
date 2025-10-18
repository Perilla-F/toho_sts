using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class EventBase : ScriptableObject
{
    public string EventId;
    public string EventName;
    public string Description;

    [Header("見た目 (Addressable)")]
    public AssetReferenceSprite EventImageRef;

    public List<EventOption> Options;

    public abstract void Execute(string optionId);
}