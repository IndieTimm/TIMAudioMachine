using System;
using UnityEngine;
using UnityEngine.Events;

public enum MouseButton
{
    Left = 0,
    Right = 1,
    Middle = 2
}

public class Button : MonoBehaviour, IInteractableObject, IInteractionStrategy
{
    [SerializeField] private MouseButton mouseButton = MouseButton.Left;
    [SerializeField] private UnityEvent onClicked = null;

    public void RegisterClickCallback(Action callback)
    {
        onClicked.AddListener(() => callback?.Invoke());
    }

    public IInteractionStrategy GetStrategy()
    {
        return this;
    }

    public bool CanBeInoked()
    {
        return Input.GetMouseButtonDown((int)mouseButton);
    }

    public object Invoke(object currentState)
    {
        onClicked?.Invoke();
        return null;
    }

    public void Break(object currentState)
    {
        
    }

    public bool IsForceBreak()
    {
        return false;
    }
}
