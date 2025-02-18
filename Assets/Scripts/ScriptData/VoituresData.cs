using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Voitures", menuName = "Scriptable Objects/Voitures")]
public class VoituresData : ScriptableObject
{
    //Image de la vignette du catalogue
    [SerializeField] private Sprite ImageCar;
    public Sprite imageCar => ImageCar;
    [SerializeField] private GameObject Prefab;
    public GameObject prefab => Prefab;

    [SerializeField] private string ModelName;
    public string modelName => ModelName;
    [SerializeField] private List<Material> Materials;
    public List<Material> materials => Materials;
}
