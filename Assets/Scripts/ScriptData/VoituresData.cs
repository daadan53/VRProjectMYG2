using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "Voitures", menuName = "Scriptable Objects/Voitures")]
public class VoituresData : ScriptableObject
{
    //Image de la vignette du catalogue
    public Sprite imageCar;
    public GameObject prefab;
    public string modelName;
    public List<Material> materials;
}
