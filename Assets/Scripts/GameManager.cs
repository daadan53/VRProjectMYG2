using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private List<GameObject> buttonsList;

    public InputActionAsset inputActions;
    private InputAction menu;
    [SerializeField] GameObject canvasStats;
    
    void Awake()
    {
        //LoadCarButtons.OnClickButton += GiveInformations;
        ApiConnect.OnProductsRetrieved += RedistributeInformations;

         if(content != null)
        {
            foreach(Transform child in content.transform)
            {
                buttonsList.Add(child.gameObject);
            }
        }
    }

    void Start()
    {
        menu = inputActions.FindActionMap("XRI Left Interaction").FindAction("Menu");
        menu.Enable();
        menu.started += OnMenuPressed;
        menu.canceled += OnMenuReleased;

        canvasStats.SetActive(false);
    }

    public void OnMenuPressed(InputAction.CallbackContext context)
    {
        canvasStats.SetActive(true);
    }

    private void OnMenuReleased(InputAction.CallbackContext context)
    {
        canvasStats.SetActive(false);
    }

    //Récuperer les données et les distribuer au bon bouton
    public void RedistributeInformations(Vehicules[] _vehicules)
    {
        foreach (var vehicule in _vehicules)
        {
            foreach (var button in buttonsList)
            {
                ButtonsManager loadCarButton = button.GetComponent<ButtonsManager>();

                if (loadCarButton != null && loadCarButton.modelName == vehicule.model)
                {
                    loadCarButton.SetVehicleData(vehicule);
                }
            }
        }
    }

    void OnDestroy()
    {
        menu.started -= OnMenuPressed;
        menu.canceled -= OnMenuReleased;
    }
}
