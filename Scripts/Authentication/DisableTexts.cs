using UnityEngine;
using UnityEngine.UI;

public class DisableTexts : MonoBehaviour
{
    private void OnEnable()
    {
        DisableText();
    }

    void DisableText()
    {
        InputField[] fields = GetComponentsInChildren<InputField>();
        foreach (InputField field in fields)
        {
            field.text = "";
        }
    }
}