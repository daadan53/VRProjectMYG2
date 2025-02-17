using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class FeaturesTest
{

    private GameObject content;
    private GameObject spawnCar;
    private Button[] vehicleButtons;
    private Button[] materialButtons;
    private Button backButton;

    [SetUp]
    public void SetUp()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene("Scenes/SampleScene", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "SampleScene")
        {
            // Trouver les objets dans la scène
            content = GameObject.Find("Content");
            spawnCar = GameObject.Find("SpawnCar");

            Assert.IsNotNull(content, "Le GameObject 'content' est introuvable.");
            Assert.IsNotNull(spawnCar, "Le GameObject 'SpawnCar' est introuvable.");

            vehicleButtons = content.GetComponentsInChildren<Button>();
            Assert.IsTrue(vehicleButtons.Length >= 2, "Il doit y avoir au moins deux boutons de véhicules.");

            // Récupère les boutons de changement de material un par un et les mets sous forme de liste
            materialButtons = GameObject.FindGameObjectsWithTag("MaterialButton")
                                        .Select(go => go.GetComponent<Button>())
                                        .ToArray();
            Assert.IsTrue(materialButtons.Length >= 1, "Les boutons de changement de matériau sont introuvables.");

            backButton = GameObject.Find("BackButton").GetComponent<Button>();
            Assert.IsNotNull(backButton, "Le bouton de retour est introuvable.");

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        else
        {
            Assert.Fail("Le nom de la scene est incorrect");
        }
    }

    [UnityTest]
    public IEnumerator FeaturesTestWithEnumeratorPasses()
    {
        // 1. Cliquer sur le premier bouton de voiture
        vehicleButtons[0].onClick.Invoke();
        yield return new WaitForSeconds(1f);

        // 2. Vérifier que le véhicule est instancié dans SpawnCar
        Assert.IsTrue(spawnCar.transform.childCount > 0, "Le véhicule n'a pas été instancié.");
        GameObject instantiatedVehicle = spawnCar.transform.GetChild(0).gameObject;
        Assert.IsNotNull(instantiatedVehicle, "Le véhicule instancié est null.");

        // 3. Changer son matérial via un bouton de matérial
        materialButtons[2].onClick.Invoke();
        yield return new WaitForSeconds(1f);

        // 4. Cliquer sur le bouton retour
        backButton.onClick.Invoke();
        yield return new WaitForSeconds(1f);

        // 5. Cliquer sur le deuxième bouton de voiture
        vehicleButtons[1].onClick.Invoke();
        yield return new WaitForSeconds(1f);

        // 6. Vérifier que le véhicule a changé
        GameObject newInstantiatedVehicle = spawnCar.transform.GetChild(0).gameObject;
        Assert.AreNotEqual(instantiatedVehicle, newInstantiatedVehicle, "Le véhicule n'a pas été remplacé.");
    }
}
