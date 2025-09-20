using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPopup : MonoBehaviour
{
    [SerializeField]
    private GameObject popupContainer;

    [SerializeField]
    private Button retryButton;

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
