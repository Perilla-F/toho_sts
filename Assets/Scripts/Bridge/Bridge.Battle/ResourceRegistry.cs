using System.Collections.Generic;

public class ResourceRegistry
{
    private readonly Dictionary<ResourceType, IResource> resources = new();

    public void Register(IResource resource)
    {
        resources[resource.Type] = resource;
    }

    public IResource Get(ResourceType type)
    {
        return resources.TryGetValue(type, out var r) ? r : null;
    }
}