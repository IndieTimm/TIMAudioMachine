using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ConfigsBank : MonoBehaviour
{
    private float timer = 0F;
    private Button currentBankClicked = null;

    private void Awake()
    {
        var buttons = GetComponentsInChildren<Button>();

        foreach (var button in buttons)
        {
            button.RegisterClickCallback(() => currentBankClicked = button);
        }
    }

    private async void Update()
    {
        if (!Input.GetMouseButton(0) && currentBankClicked != null)
        {
            if (timer < 0.5F)
            {
                var bankGameObject = currentBankClicked.gameObject;
                currentBankClicked = null;

                await LoadBank(bankGameObject);
            }

            currentBankClicked = null;
        }

        if (currentBankClicked != null)
        {
            if (timer > 2)
            {
                var bankGameObject = currentBankClicked.gameObject;
                currentBankClicked = null;

                await SaveBank(bankGameObject);
            }

            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }

    private async Task LoadBank(GameObject bankGameObject)
    {
        var text = bankGameObject.GetComponentInChildren<TMPro.TextMeshPro>();

        for (int i = 0; i < 3; i++)
        {
            if (text != null)
            {
                text.color = i % 2 == 0
                    ? Color.white
                    : Color.red;
            }

            await Task.Delay(500);
        }

        SaveManager.SetBankFolder(bankGameObject.name);

        foreach (var managedObject in GameSceneUtility.FindObjects<IManagedObject>())
        {
            managedObject.Load();
        }
    }

    private async Task SaveBank(GameObject bankGameObject)
    {
        var text = bankGameObject.GetComponentInChildren<TMPro.TextMeshPro>();

        for (int i = 0; i < 5; i++)
        {
            if (text != null)
            {
                text.color = i % 2 == 0
                    ? Color.white
                    : Color.red;
            }

            await Task.Delay(500);
        }

        SaveManager.ClearBankFolder(bankGameObject.name);
        SaveManager.SetBankFolder(bankGameObject.name);

        foreach (var managedObject in GameSceneUtility.FindObjects<IManagedObject>())
        {
            managedObject.Save();
        }
    }
}
