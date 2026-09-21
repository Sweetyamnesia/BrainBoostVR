using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ConfirmPanel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI confirmText;
    public Button buttonYes;
    public Button buttonNo;

    private Action onConfirm;
    private Action onCancel;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (buttonYes != null)
            buttonYes.onClick.AddListener(() => { onConfirm?.Invoke(); Close(); });

        if (buttonNo != null)
            buttonNo.onClick.AddListener(() => { onCancel?.Invoke(); Close(); });
    }

    public void Open(string message, Action confirmAction, Action cancelAction = null)
    {
        confirmText.text = message;
        onConfirm = confirmAction;
        onCancel = cancelAction;
        gameObject.SetActive(true);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }
}
