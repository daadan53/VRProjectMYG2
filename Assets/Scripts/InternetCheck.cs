using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class InternetCheck : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI errorMessage;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private InputActionAsset inputActions;
    public bool internetConnection = true;

    private string testUrl = "https://www.google.com"; // URL pour tester la connexion

    void Start()
    {
        retryButton.onClick.AddListener(() => StartCoroutine(CheckInternetConnection()));
        quitButton.onClick.AddListener(() => OnApplicationQuit());
        StartCoroutine(CheckInternetConnection());
    }

    private void SetPlayerControlsActive(bool isActive)
    {
        if(inputActions != null)
        {
            InputAction leftLocomotion = inputActions.FindActionMap("XRI Left Locomotion").FindAction("Move");

            InputAction rightLocomotion = inputActions.FindActionMap("XRI Right Locomotion").FindAction("Teleport mode");

            if (leftLocomotion != null) 
            {
                if (isActive) 
                {
                    leftLocomotion.Enable();
                    Debug.Log("Mouvement activé");
                }
                else 
                {
                    leftLocomotion.Disable();
                    Debug.Log("Mouvement desactivé");
                }
            }

            if (rightLocomotion != null)
            {
                if (isActive) 
                {
                    rightLocomotion.Enable();
                }
                else 
                {
                    rightLocomotion.Disable();
                }
            }
        }
        else
        {
            Debug.LogError("Player inputs introuvable");
        }
    }

    IEnumerator CheckInternetConnection()
    {
        yield return null;
        
        // Désactive les contrôles au début
        SetPlayerControlsActive(false);

        //1. Vérifier la connexion au réseau
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShowError("Pas de connexion réseau. Vérifiez votre Wi-Fi.");
            yield break;
        }

        // 2. Vérifier l'accès réel à Internet via une requête HTTP
        using (UnityWebRequest request = UnityWebRequest.Get(testUrl))
        {
            request.timeout = 5; // Temps d'attente max pour éviter un blocage
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                ShowError("Pas d'accès à Internet. Vérifiez votre connexion.");
            }
            else
            {
                Debug.Log("Connexion Internet confirmée. Lancement du jeu...");
                HideError();
            }
        }

        SetPlayerControlsActive(internetConnection);
        //Debug : 
        if(!internetConnection)
        {
            ShowError("Pas d'accès à Internet. Vérifiez votre connexion.");
        }
    }

    void ShowError(string message)
    {
        if (errorPanel != null && errorMessage != null)
        {
            errorMessage.text = message;
            errorPanel.SetActive(true);
            SetPlayerControlsActive(false);
        }
        else
        {
            Debug.LogError("Pannel d'erreur non assigné dans l'inspecteur.");
        }
    }

    void HideError()
    {
        if (errorPanel != null)
        {
            errorPanel.SetActive(false);
        }
    }

    public void OnApplicationQuit() 
    {
        Application.Quit();
    }
}