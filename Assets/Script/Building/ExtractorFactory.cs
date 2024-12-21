
using UnityEngine;


public static class ExtractorFactory 
{
    // Prefab dell'estrattore, da assegnare via codice o staticamente
    private static GameObject extractorPrefab; 
    
     // Metodo per inizializzare la factory (impostare il prefab)
     public static void Initialize( GameObject prefab)
     {
         extractorPrefab=prefab;
          Debug.Log("ExtractorFactory inizializzata.");
     }

    public static GameObject CreateExtractor(Vector3 position, Quaternion rotation)
    {
        if (extractorPrefab == null)
        {
            Debug.LogError("Extractor prefab is null");
            return null;
        }

        GameObject extractor = Object.Instantiate(extractorPrefab, position, rotation);
        Debug.Log("Extractor created at " + position);
        return extractor;
    }
    
}
