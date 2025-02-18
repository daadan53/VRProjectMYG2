using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class JSonHelper
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] results;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.results;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.results = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.results = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
}

[Serializable]
public class Vehicules
{
    public string id;
    public string make;
    public string model;
    public int cylinders;
    public string fueltype1;
    public int fuelcost08;
    public int year;
    public string trany;
}

public class ApiConnect : MonoBehaviour
{
    private string _uri = "https://data.opendatasoft.com/api/explore/v2.1/catalog/datasets/all-vehicles-model@public/records?limit=20&select=id,make,model,cylinders,fueltype1,fuelcost08,year,trany";
    public static event Action<Vehicules[]> OnProductsRetrieved;
    public Vehicules[] vehicules {get; private set;}

    void Start()
    {
        StartCoroutine(RetrieveDataFromInternet());
    }

    public IEnumerator RetrieveDataFromInternet()
    {
        //La requête
        using(UnityWebRequest webRequest = UnityWebRequest.Get(_uri))
        {
            //On envoie la requête
            yield return webRequest.SendWebRequest();

            //On traite la réponse 
            switch(webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Connection Error");
                    break;
                
                //Problème à la reception des données
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"Something went wrong {webRequest.error}");
                    break;

                case UnityWebRequest.Result.Success:
                    Debug.Log("Connection successful");
                    Debug.Log(webRequest.downloadHandler.text);
                    vehicules = JSonHelper.FromJson<Vehicules>(webRequest.downloadHandler.text); //On récup les données
                    if (vehicules != null && vehicules.Length > 0)
                    {
                        OnProductsRetrieved?.Invoke(vehicules);
                    }
                    else
                    {
                        Debug.LogError("Les données des véhicules sont null ou vides");
                    }
                    break;
            }
        }
    }
}
