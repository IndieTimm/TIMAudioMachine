using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDisplayDataSource<T>
{
    T GetData();
}

[RequireComponent(typeof(LineRenderer))]
public class Display : MonoBehaviour
{
    [SerializeField] private MonoBehaviour dataReference = null;
    [SerializeField] private int samplePerSegment = 4;
    [SerializeField] private int bufferSize = 256;
    [SerializeField] private Rect displayRect = new Rect(0, 0, 1, 1);

    private LineRenderer lineRenderer = null;
    private IDisplayDataSource<float> dataSource = null;
    private List<float> buffer = null;
    private float minimalValue = float.MaxValue;
    private float maximalValue = float.MinValue;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (dataReference != null)
        {
            dataSource = dataReference as IDisplayDataSource<float>;
            buffer = new List<float>();
        }

        if (lineRenderer != null)
        {
            Setup();
        }
    }

    private void Setup()
    {
        lineRenderer.positionCount = bufferSize;
        Refresh(0.5F);
    }

    private void FixedUpdate()
    {
        if (dataSource == null || lineRenderer == null)
        {
            return;
        }

        var value = dataSource.GetData();

        if (value < minimalValue)
        {
            minimalValue = value;
        }

        if (value > maximalValue)
        {
            maximalValue = value;
        }

        if (Mathf.Approximately(maximalValue - minimalValue, 0))
        {
            value = 0;
        }
        else
        {
            value = (value - minimalValue) / (maximalValue - minimalValue);
        }

        if (buffer.Count == bufferSize)
        {
            buffer.RemoveAt(0);
        }

        buffer.Add(value);

        Refresh();
    }

    private void Refresh(float? forceValue = null)
    {
        for (int i = 0; i < bufferSize; i++)
        {
            var time = Mathf.Clamp01(samplePerSegment * i / (bufferSize - 1.0F));
            var value = forceValue.HasValue 
                ? forceValue.Value 
                : buffer.ElementAtOrDefault(i);

            lineRenderer.SetPosition(i, GetPoint(time, value));
        }
    }

    private void OnDrawGizmosSelected()
    {
        var rect = GetBounds();
        var radius = rect.size.magnitude * 0.5F * 0.15F;

        for (int i = 0; i < 5; i++)
        {
            var time = i / 4F;
            var point = GetPoint(time, 0.5F);

            Gizmos.DrawSphere(point, radius);
        }

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(displayRect.center - displayRect.size * 0.5F, displayRect.size);
    }

    private Vector3 GetPoint(float time, float valueNormalized)
    {
        var x = displayRect.x + (time - 0.5F) * displayRect.width;
        var y = displayRect.y + (valueNormalized - 0.5F) * displayRect.height;

        return transform.localToWorldMatrix.MultiplyPoint(new Vector3(x, y, 0));
    }

    private Bounds GetBounds()
    {
        var position = transform.position + (Vector3)displayRect.position;
        var scale = transform.localToWorldMatrix.MultiplyVector(displayRect.size);

        return new Bounds(position, scale);
    }
}
