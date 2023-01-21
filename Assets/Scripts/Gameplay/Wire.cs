using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Wire : MonoBehaviour
{
    public Transform From;
    public Transform To;

    private Vector3 fromPosition;
    private Vector3 toPosition;

    private LineRenderer lineRenderer = null;

    public static Wire CreateWire(Transform from, Transform to)
    {
        if (from == null || to == null)
        {
            return null;
        }

        var wireContainer = new GameObject("wire");
        var wire = wireContainer.AddComponent<Wire>();

        wire.From = from;
        wire.To = to;

        // Принудительно вызываем обновление
        wire.fromPosition = wire.From.position + Vector3.one;

        return wire;
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = WireOptions.Instance.Segments;
        lineRenderer.widthMultiplier = WireOptions.Instance.Width;
        lineRenderer.material = WireOptions.Instance.WireMaterial;
    }

    private void Update()
    {
        if (From == null || To == null)
        {
            return;
        }

        if (From.position == fromPosition && To.position == toPosition)
        {
            return;
        }

        var from = From.position + From.up * 0.05F;
        var to = To.position + To.up * 0.05F;
        var distance = Vector3.Distance(From.position, To.position);
        var gravity = WireOptions.Instance.GravityPerUnit / distance * WireOptions.Instance.GravityPower;
        gravity = Mathf.Clamp(gravity, WireOptions.Instance.MinimalGravity, WireOptions.Instance.MaximalGravity) * distance;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            var time = i / (lineRenderer.positionCount - 1.0F);
            var gravityOffset = Vector3.up * (1 - time * time) * gravity * Mathf.PingPong(time, 1);
            var position = Vector3.Lerp(from, to, time);

            if (Physics.Raycast(position, gravityOffset, out RaycastHit hit, gravityOffset.magnitude))
            {
                position = hit.point + Vector3.up * WireOptions.Instance.Width;
            }
            else
            {
                position += gravityOffset;
            }

            lineRenderer.SetPosition(i, position);
        }

        fromPosition = From.position;
        toPosition = To.position;
    }
}
