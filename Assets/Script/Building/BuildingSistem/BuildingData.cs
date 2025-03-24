using UnityEngine;

[CreateAssetMenu(fileName ="newBuildingData" ,menuName = "Factory3D/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public GameObject buildingPrefab;
    public GameObject buildingGhostPrefab;
}