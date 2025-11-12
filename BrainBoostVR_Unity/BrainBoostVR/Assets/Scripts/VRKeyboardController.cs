using UnityEngine;
using TMPro;

public class VRKeyboardController : MonoBehaviour
{
    public TMP_InputField targetInput; // L'InputField où écrire
    public GameObject keyboardPanel;   // Ton panel clavier

    private void Start()
    {
        keyboardPanel.SetActive(false); // clavier caché au départ
    }

    public void ShowKeyboard()
    {
        keyboardPanel.SetActive(true);
    }

    public void HideKeyboard()
    {
        keyboardPanel.SetActive(false);
    }

    public void OnKeyPress(string character)
    {
        targetInput.text += character;
    }

    public void OnBackspace()
    {
        if (!string.IsNullOrEmpty(targetInput.text))
            targetInput.text = targetInput.text.Substring(0, targetInput.text.Length - 1);
    }

    public void OnEnter()
    {
        HideKeyboard();
        // ici tu peux appeler MainMenu.PlayGame() si tu veux
    }
}
