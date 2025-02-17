using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadCarButtons : MonoBehaviour
{
    public VoituresData voituresData;

    //Info de la voiture
    public string modelName;
    private string description;
    
    //Vignette du catalogue
    private TextMeshProUGUI txtCarName;
    private Image carImage;
    [SerializeField] private GameObject carModel;
    [SerializeField] private GameObject spawnCar;

    //Canvas
    [SerializeField] private Canvas canvasCaract;
    [SerializeField] private Canvas canvasCatalogue;
    [SerializeField] private TextMeshProUGUI txtDescription;
    private Button button;

    [SerializeField] private Button blueButton;
    [SerializeField] private Button redButton;
    [SerializeField] private Button blackButton;

    void Awake()
    {
        LoadPrefab();
    }

    private void LoadPrefab()
    {
        modelName = voituresData.modelName;
        txtCarName = GetComponentInChildren<TextMeshProUGUI>();

        if(transform.childCount > 0)
        {
            carImage = transform.GetChild(1).GetComponent<Image>();
            carImage.sprite = voituresData.imageCar;
        }
        else 
        {
            Debug.LogWarning("Aucun enfatn trouvé pour cet objet");
        }

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
        else
        {
            Debug.LogError("Aucun composant Button n'a été trouvé sur cet objet");
        }

        //On récupère les boutons de changement de material et on les assignes à l'event
        blueButton.onClick.AddListener(() => ChangeMaterial(0));
        redButton.onClick.AddListener(() => ChangeMaterial(1));
        blackButton.onClick.AddListener(() => ChangeMaterial(2));

        canvasCaract.enabled = false;
    }

    public void SetVehicleData(Vehicules vehicule)
    {
        // Met à jour l'affichage avec les données du véhicule
        txtCarName.text = vehicule.make + " " + vehicule.model;
        description = $"La {vehicule.make} {vehicule.model}, produite en {vehicule.year}, possède {vehicule.cylinders} cylindres pour une transmission {vehicule.trany}. Elle roule au {vehicule.fueltype1}, coûtant annuellement : {vehicule.fuelcost08}€.";
        
        //Debug.Log($"Données assignées à {gameObject.name} : {description}");
    }

    //Event au clique sur un des produits du catalogue
    public void OnClick()
    {
        if(spawnCar.transform.childCount > 0)
        {
            Destroy(spawnCar.transform.GetChild(0).gameObject);
        }
        carModel = Instantiate(voituresData.prefab);
        carModel.transform.SetParent(spawnCar.transform);
        carModel.transform.localPosition = Vector3.zero;
        carModel.transform.localScale = new Vector3(1,1,1);

        canvasCaract.enabled = true;
        canvasCatalogue.enabled = false;

        SetInfoOnCanvas();
    }

    private void SetInfoOnCanvas()
    {
        txtDescription.text = description;
    }

    public void OnGoBack()
    {
        canvasCatalogue.enabled = true;
        canvasCaract.enabled = false;
    }

    // Event au clique sur les boutons de changement de couleurs
    public void ChangeMaterial(int _materialIndex)
    {
        if(carModel == spawnCar.transform.GetChild(0).gameObject)
        {
            if (_materialIndex < 0 || _materialIndex >= voituresData.materials.Count)
            {
                Debug.LogWarning("Index de matériau invalide !");
                return;
            }

            Renderer renderer = spawnCar.transform.GetChild(0).gameObject.GetComponent<Renderer>();
            if (renderer == null)
            {
                renderer = spawnCar.transform.GetChild(0).gameObject.GetComponentInChildren<Renderer>();
                renderer.material = voituresData.materials[_materialIndex];
            }
            else
            {
                renderer.material = voituresData.materials[_materialIndex];
            }
        }
    }
}
