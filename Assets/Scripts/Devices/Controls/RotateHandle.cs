
using System;
using UnityEngine;

public class RotateHandleInteractionStrategy : IInteractionStrategy
{
    public bool IsHold { get; private set; }

    private Action onClick;

    public RotateHandleInteractionStrategy(Action onBegin)
    {
        onClick = onBegin;
    }

    public void Break(object currentState) 
    {
        IsHold = false;
    }

    public bool CanBeInoked()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool IsForceBreak()
    {
        return !Input.GetMouseButton(0);
    }

    public object Invoke(object currentState)
    {
        onClick?.Invoke();
        IsHold = true;
        return IsHold;
    }    
}

public class RotateHandle : MonoBehaviour, IInteractableObject
{
    public event Action<float> OnValueChanged;

    public float Value
    {
        get => currentValue;
        set => currentValue = value;
    }

    [SerializeField] private float currentValue = 0;
    [SerializeField] private Vector3 up = Vector3.up;
    [SerializeField] private Vector3 normal = Vector3.right;

    private Vector3 startupPoint;
    private RotateHandleInteractionStrategy strategy = null;

    public IInteractionStrategy GetStrategy()
    {
        if (strategy == null)
        {
            strategy = new RotateHandleInteractionStrategy(SetPoint);
        }

        return strategy;
    }

    private void Update()
    {
        if (strategy == null || strategy.IsHold == false)
        {
            return;
        }

        var up = transform.localToWorldMatrix.MultiplyVector(this.up);
        var normal = transform.localToWorldMatrix.MultiplyVector(this.normal);
        var direction = GetPoint() - startupPoint;

        var angle = Vector3.SignedAngle(up, direction, normal);

        if (angle < 0)
        {
            angle = 360 - angle;
        }

        currentValue = angle / 360;

        OnValueChanged?.Invoke(currentValue);
    }

    private void SetPoint()
    {
        startupPoint = GetPoint();
    }

    private Vector3 GetPoint()
    {
        var normal = transform.localToWorldMatrix.MultiplyVector(this.normal);
        var plane = new Plane(normal, transform.position);
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0) * 0.5F);
        
        plane.Raycast(ray, out float enter);

        return ray.GetPoint(enter);
    }
}
