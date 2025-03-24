
using UnityEngine;


public static class ExtractorFactory 
{
    // Prefab dell'estrattore, da assegnare via codice o staticamente
   private static GameObject extractorPrefab;
   private static GameObject ghostPrefab;
   // Inizializza la factory con i prefab
   public static void Init(GameObject extractorPrefab, GameObject ghostPrefab)
   {
       ExtractorFactory.extractorPrefab = extractorPrefab;
       ExtractorFactory.ghostPrefab = ghostPrefab;
        Debug.Log("ExtractorFactory inizializzata.");
    }
    // Metodo per creare un estrattore
    public static GameObject CreateExtractor(Vector3 position, Quaternion rotation)
    {
        if (extractorPrefab == null)
        {
            Debug.LogError("ExtractorFactory non inizializzata.");
            return null;
        }
        GameObject extractor = GameObject.Instantiate(extractorPrefab, position, rotation);
        Debug.Log("Estrattore creato!");
        return extractor;
    }
    // Metodo per creare un ghost
    public static GameObject CreateGhostExtractor()
    {
        if(ghostPrefab==null)
        {
            Debug.LogError("ExtractorFactory non inizializzata.");
            return null;
        }

        GameObject ghostExtractor= GameObject.Instantiate(ghostPrefab);
        ghostExtractor.SetActive(false); // Inizia disattivato
        Debug.Log("Ghost Extractor creato!");
        return ghostExtractor;

    }
    
}
