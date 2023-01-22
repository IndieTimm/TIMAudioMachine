using UnityEngine;

public class Led : MonoBehaviour
{
    [SerializeField] private MonoBehaviour dataReference = null;
    [SerializeField] private int channel = 0;
    [SerializeField] private bool setColorMode = false;
    [SerializeField] private Color enableColor = Color.white;
    [SerializeField] private Color disaableColor = Color.gray;
    [SerializeField] private Renderer led = null;

    private Color defaultColor;
    private IDisplayDataSource<bool[]> dataSource = null;

    private void Awake()
    {
        if (dataReference != null)
        {
            dataSource = dataReference as IDisplayDataSource<bool[]>;
        }

        defaultColor = led.material.color;
    }

    private void FixedUpdate()
    {
        if (dataSource != null)
        {
            var data = dataSource.GetData();

            if (channel >= 0 && channel < data.Length)
            {
                led.material.color = data[channel]
                    ? enableColor
                    : disaableColor;

                if (!setColorMode)
                {
                    led.material.color = defaultColor * led.material.color;
                }
            }
        }
    }
}
