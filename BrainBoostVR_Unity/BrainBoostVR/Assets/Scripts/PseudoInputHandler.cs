using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Microsoft.VisualBasic;

public class PseudoInputHandler : MonoBehaviour, IPointerClickHandler
{
	private OpenVirtualKeyboard keyboardController;
	private TMP_InputField inputField;

	void Start()
	{
		keyboardController = Object.FindFirstObjectByType<OpenVirtualKeyboard>();
		inputField = GetComponent<TMP_InputField>();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (keyboardController != null)
		{
			keyboardController.OnOpenVirtualKeyboard();
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		if (keyboardController != null)
		{
			keyboardController.OnCloseVirtualKeyboard();
		}
	}
}