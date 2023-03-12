using System.Collections.Generic;
using UnityEngine;

public class Spawner<T> where T : Component
{
    private T _prefab { get; }
    private Queue<T> _spawned = new Queue<T>();
    private Queue<T> _hided = new Queue<T>();

    public Spawner(T prefab)
    {
        _prefab = prefab;
    }

    public T Spawn()
    {
        T newComponent;
        if (_hided.Count == 0)
        {
            var newObject = GameObject.Instantiate(_prefab.gameObject);
            if (newObject.transform is T transform)
            {
                newComponent = transform;
            }
            else
            {
                newComponent = newObject.GetComponent<T>();
            }
        }
        else
        {
            newComponent = _hided.Dequeue();
        }

        newComponent.gameObject.SetActive(true);
        _spawned.Enqueue(newComponent);
        return newComponent;
    }

    public void Despawn()
    {
        if (_spawned.Count == 0)
        {
            return;
        }
        var component = _spawned.Dequeue();
        _hided.Enqueue(component);
        component.gameObject.SetActive(false);
    }
}