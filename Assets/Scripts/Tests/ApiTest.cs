using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ApiTest
{
    private GameObject apiObject;
    private ApiConnect apiConnect;
    private Vehicules[] receivedVehicles;
    private bool dataReceived = false;

    [SetUp]
    public void Setup()
    {
        apiObject = new GameObject();
        apiConnect = apiObject.AddComponent<ApiConnect>();

        // Écouter l'événement OnProductsRetrieved pour détecter si les données sont bien reçues
        ApiConnect.OnProductsRetrieved += HandleDataReceived;
    }

    private void HandleDataReceived(Vehicules[] vehicules)
    {
        if (vehicules != null && vehicules.Length > 0)
        {
            dataReceived = true;
            receivedVehicles = vehicules;
        }
    }


    [UnityTest]
    public IEnumerator ApiTestWithEnumeratorPasses()
    {
        Assert.IsNotNull(apiConnect, "L'objet ApiConnect est nul");

        // Lancer la récupération des données
        apiConnect.StartCoroutine(apiConnect.RetrieveDataFromInternet());

        yield return new WaitForSeconds(3f);

        // Vérifier que des données ont été reçues
        Assert.IsTrue(dataReceived, "Les données de l'API n'ont pas été reçues");

        foreach (Vehicules vehicle in receivedVehicles)
        {
            Assert.IsFalse(string.IsNullOrEmpty(vehicle.make), "Un véhicule n'a pas de marque !");
            Assert.IsFalse(string.IsNullOrEmpty(vehicle.model), "Un véhicule n'a pas de modèle !");
            Assert.IsTrue(vehicle.cylinders > 0, $"Le nombre de cylindres ({vehicle.cylinders}) est invalide !");
            Assert.IsFalse(string.IsNullOrEmpty(vehicle.fueltype1), "Un véhicule n'a pas de type de carburant !");
            Assert.IsTrue(vehicle.fuelcost08 > 0, $"Le coût du carburant ({vehicle.fuelcost08}) est invalide !");
            Assert.IsFalse(string.IsNullOrEmpty(vehicle.trany), "Un véhicule n'a pas de trany !");
            Assert.IsTrue(vehicle.year > 0, $"Un véhicule a une date de fabrication qui ne correspond pas");
        }

        Debug.Log("✅ Test réussi : Toutes les données sont valides !");
    }

    [TearDown]
    public void TearDown()
    {
        ApiConnect.OnProductsRetrieved -= HandleDataReceived;
        Object.Destroy(apiObject);
    }
}
