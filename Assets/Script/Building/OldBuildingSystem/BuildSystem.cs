using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] private GameObject extractorPrefab;  // Prefab dell'estrattore
    [SerializeField] private GameObject ghostPrefab;  // Prefab dell'estrattore fantasma
    [SerializeField] private LayerMask groundlayer; // Layer del terreno

    [SerializeField] private float maxDistance = 10f;  // Distanza massima di piazzamento
    
    private GameObject ghostExtractor; // Riferimento all'estrattore fantasma
    private Camera mainCamera;  // Camera principale
    private bool isBuilding = false;  // Flag per il piazzamento

    private void Start()
    {
        mainCamera = Camera.main;

        // Inizializza la factory con i prefab
        ExtractorFactory.Init(extractorPrefab, ghostPrefab);

        // Crea l'estrattore fantasma
        ghostExtractor = ExtractorFactory.CreateGhostExtractor();
    }

   
    void Update()
    {
        // Premi B per attivare/disattivare la modalità di piazzamento
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuilding = !isBuilding;
            ghostExtractor.SetActive(isBuilding);
        }

         // Gestisci il piazzamento quando la modalità è attiva
        if (isBuilding)
        {
            HandlePlacement();
        }
    }

    private void HandlePlacement()
    {
        // Esegui il raycast dal mouse al terreno
    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.yellow);
    if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, groundlayer))
    {
        Vector3 placementPosition = hit.point;
        ghostExtractor.transform.position = placementPosition;

        // Verifica se il cursore è sopra una risorsa

        bool canPlace = hit.collider.CompareTag("resource");
        Debug.Log(hit.collider.name);

        // Cambia colore in base alla validità
        Renderer[] ghostRenderers = ghostExtractor.GetComponentsInChildren<Renderer>();
        foreach (var renderer in ghostRenderers)
        {
            foreach (var material in renderer.materials)
            {
                material.color = canPlace ? Color.green : Color.red;
            }
        }

        // Piazza l'estrattore se l'area è valida
        if (canPlace && Input.GetMouseButtonDown(0))
        {
            PlaceExtractor(hit.collider.gameObject);
        }
    }
    else
    {
        Debug.LogWarning("Raycast non ha colpito il terreno!");
    }
    }

    private void PlaceExtractor(GameObject resource)
    {
         // Richiedi alla Factory di creare un estrattore vero e proprio
         Vector3 resourcePosition = resource.transform.position;
         Quaternion resourceRotation = Quaternion.identity;

            GameObject extractor = ExtractorFactory.CreateExtractor(resourcePosition, resourceRotation);
          if(extractor != null)  
         {
             Debug.Log("Estrattore piazzato!");
         }
    // Disattiva la modalità di piazzamento
        isBuilding = false;
        ghostExtractor.SetActive(false);
    }
}
