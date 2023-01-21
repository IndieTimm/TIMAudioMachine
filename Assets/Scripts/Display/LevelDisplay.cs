using System.Linq;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    [SerializeField] private MonoBehaviour dataReference = null;
    [SerializeField] private int channel = 0;
    [SerializeField] private float minimalValue = 0;
    [SerializeField] private float maximalValue = 1;
    [SerializeField] private Renderer[] indicies = null;
    [SerializeField] private Color enableColor = Color.white;
    [SerializeField] private Color disaableColor = Color.gray;

    private Color[] defaultColors;
    private IDisplayDataSource<float[]> dataSource = null;


    private void Awake()
    {
        if (dataReference != null)
        {
            dataSource = dataReference as IDisplayDataSource<float[]>;
        }

        defaultColors = indicies.Select(x => x.material.color).ToArray();
    }

    private void FixedUpdate()
    {
        var index = 0;

        if (dataSource != null)
        {
            var data = dataSource.GetData();

            if (channel >= 0 && channel < data.Length)
            {
                var value = Mathf.Abs(data[channel]);
                var level = (value - minimalValue) / (maximalValue - minimalValue);
                index = Mathf.RoundToInt(level * indicies.Length);
            }
        }

        for (int i = 0; i < indicies.Length; i++)
        {
            indicies[i].material.color = defaultColors[i] * (i < index
                ? enableColor
                : disaableColor);
        }
    }
}
