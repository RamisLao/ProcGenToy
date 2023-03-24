using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Variable<T> : ScriptableObject
{
    [SerializeField] protected T _value;
    public bool IsEmpty => _value == null;

    public T Value
    {
        get { return _value; }
        set
        {
            _value = value;
            OnChanged.Invoke(_value);
        }
    }

    public UnityEvent<T> OnChanged;

    private void Awake()
    {
        if (IsEmpty) _value = default(T);
    }
}
