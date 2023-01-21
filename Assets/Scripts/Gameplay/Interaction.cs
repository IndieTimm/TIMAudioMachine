using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Color color = Color.black;
    [SerializeField] private Image image = null;

    private object currentState = null;
    private IInteractionStrategy currentStrategy = null;

    private void Update()
    {
        var colorFactor = 0F;
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width, Screen.height, 0) * 0.5F);

        if (currentStrategy != null)
        {
            if (Input.GetMouseButtonDown(1) || currentStrategy.IsForceBreak())
            {
                currentStrategy?.Break(currentState);
                currentStrategy = null;
                currentState = null;
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 2F))
        {
            var interactableObject = hit.collider.GetComponent<IInteractableObject>();

            if (interactableObject != null)
            {
                var strategy = interactableObject.GetStrategy();

                if (strategy.CanBeInoked())
                {
                    currentState = strategy.Invoke(currentState);

                    if (currentState != null)
                    {
                        currentStrategy = strategy;
                    }
                }

                colorFactor = Mathf.Sin(Time.time * 2) * 0.5F + 0.5F;
            }
        }

        image.color = Color.Lerp(Color.white, color, colorFactor);
    }
}
