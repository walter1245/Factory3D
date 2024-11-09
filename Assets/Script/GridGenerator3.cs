using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceType = GridData.ResourceType;

[ExecuteInEditMode]
public  class GridGenerator : MonoBehaviour
{
    public GridData gridData;  // Riferimento al GridData ScriptableObject

    // Prefab per ogni tipo di risorsa
    public GameObject gasPrefab;
    public GameObject liquidPrefab;
    public GameObject rockPrefab;
    public GameObject nonePrefab; // Prefab per celle vuote

    private Dictionary<ResourceType, GameObject> resourcesPrefab; // Mappa dei prefab

    // Dimensione del piano (default 10x10 unità)
    public float planeUnitSize = 10f;
    
    public Transform parentObject;

    void Start()
    {
        // Inizializza la mappa dei prefab con i tipi di risorsa
        resourcesPrefab = new Dictionary<ResourceType, GameObject>()
        {
            { ResourceType.Gas, gasPrefab },
            { ResourceType.Liquid, liquidPrefab },
            { ResourceType.Rock, rockPrefab },
            { ResourceType.None, nonePrefab }
        };

        GenerateGrid(); // Genera la griglia all'avvio
    }

    public void GenerateGrid()
    {
        if (gridData == null)
        {
            Debug.LogError("GridData non assegnato.");
            return;
        }
        // Elimina eventuali oggetti già esistenti sotto il parentObject
        if (parentObject != null)
        {
            while (parentObject.childCount > 0)
            {
                DestroyImmediate(parentObject.GetChild(0).gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Parent Object non assegnato. Creerò i modelli nella scena principale.");
        }


        // Ciclo su tutte le celle della griglia, piazzando un modello per ogni cella
        for (int x = 0; x < gridData.gridSize.x; x++)
        {
            for (int y = 0; y < gridData.gridSize.y; y++)
            {
                Vector2Int cellPos = new Vector2Int(x, y);

                // Determina il tipo di risorsa per la cella o usa None se non specificato
                ResourceType type = ResourceType.None;
                if (gridData.resourceCells.Exists(c => c.position == cellPos))
                {
                    type = gridData.resourceCells.Find(c => c.position == cellPos).type;
                }

                // Calcola la posizione nella scena
                Vector3 worldPosition = new Vector3(x * planeUnitSize, 0, y * planeUnitSize);

                // Instanzia il prefab corrispondente
                if (resourcesPrefab.TryGetValue(type, out GameObject prefab))
                {
                    GameObject model = Instantiate(prefab, worldPosition, Quaternion.identity,parentObject);
                    model.name = $"{type}_Model_{x}_{y}"; // Assegna un nome per debug
                }
                else
                {
                    Debug.LogWarning($"Prefab per {type} non trovato.");
                }
            }
        }
    }
}
