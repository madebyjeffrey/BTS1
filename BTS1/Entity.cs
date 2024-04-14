using System.Runtime.InteropServices;

namespace BTS1;

public class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    private readonly MultiValueDictionary<string, Aspect> _multiValueDictionary = new();

    public void AddComponent<T>(T component) where T : struct, Aspect 
    {
        _multiValueDictionary.AddOrUpdate(typeof(T).Name, component);
    }

    public bool GetComponent<T>(out T obj) where T : struct, Aspect
    {
        if (_multiValueDictionary.TryGetValue(typeof(T).Name, out var componentList))
        {
            obj = (T)componentList.FirstOrDefault();

            return true;
        }
        
        obj = default;
        return false; 
    }

    public bool GetComponents<T>(out T[] objs) where T : struct, Aspect
    {
        if (_multiValueDictionary.TryGetValue(typeof(T).Name, out var componentList))
        {
            objs = componentList.Cast<T>().ToArray();

            return true;
        }

        objs = [];
        return false;
    }

    public IEnumerable<Aspect> GetAllComponents()
    {
        return _multiValueDictionary.Values;
    }

    public void ClearComponent<T>() where T : struct, Aspect
    {
        _multiValueDictionary.RemoveKey(typeof(T).Name);
    }

    public bool HasComponent<T>() where T : struct, Aspect
    {
        return _multiValueDictionary.ContainsKey(typeof(T).Name);
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public bool HasComponent(Type T)
    {
        return _multiValueDictionary.ContainsKey(T.Name);
    }

    public bool HasAnyComponent(IEnumerable<Type> ts)
    {
        return ts.Any(HasComponent);
    }
    
}

