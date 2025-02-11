using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private List<GameObject> buttonsList;
    
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

    //Récuperer les données et les distribuer au bon bouton
    public void RedistributeInformations(Vehicules[] _vehicules)
    {
        foreach (var vehicule in _vehicules)
        {
            foreach (var button in buttonsList)
            {
                LoadCarButtons loadCarButton = button.GetComponent<LoadCarButtons>();

                if (loadCarButton != null && loadCarButton.modelName == vehicule.model)
                {
                    loadCarButton.SetVehicleData(vehicule);
                }
            }
        }
    }
}
