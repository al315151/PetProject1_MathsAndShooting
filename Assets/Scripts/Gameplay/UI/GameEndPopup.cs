using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject popupContainer;

    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private TMP_Text popupText;

    [SerializeField]
    private TMP_Text buttonText;

    public Action RetryButtonPressed;

    private void Awake()
    {
        HidePopup();

        retryButton.onClick.AddListener(OnRetryButtonPressed);
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveListener(OnRetryButtonPressed);
    }

    public void SetupButtonText(string buttonText, string popupText)
    {
        this.buttonText.SetText(buttonText);
        this.popupText.SetText(popupText);
    }

    public void ShowPopup()
    {
        popupContainer.SetActive(true);
    }

    public void HidePopup()
    {
        popupContainer.SetActive(false);
    }

    private void OnRetryButtonPressed()
    {
        RetryButtonPressed?.Invoke();
    }
}
