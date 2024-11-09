using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Color = UnityEngine.Color;
using ResourceType=GridData.ResourceType;

public class GridBuidingSystem : MonoBehaviour
{
   public enum BuidingType {
    Extractor,
    Machinery,
    Conveyor
   }
    
    public BuidingType currentBuildingType= BuidingType.Extractor;  // Tipo di costruzione selezionato
    public GameObject extractorPrefab;     // Prefab dell'estrattore
    public GameObject machineryPrefab;     // Prefab del macchinario
    public GameObject conveyorPrefab;       // Prefab del rullo
    public GridData gridData;                // Riferimento al GridData ScriptableObject
    
    public int gridSizeX=10;
    public int gridSizeY=10;
    
    
    public float cellWidth=1f;   // Larghezza della cella
    public float cellHeight=2f;  // Altezza della cella
    
    public Vector3 gridOffset=new Vector3(-5f,0f,-5f);
    
    private GameObject[,] placebuildings;        // Memorizza gli oggetti posizionati
    void Start()
    {
        placebuildings = new GameObject[gridSizeX, gridSizeY];
    }

   
    void Update()
    {
	    if (Input.GetMouseButtonDown(0)) // Controlla il click del mouse sinistro
	    {   
		    PlaceBuildings();
	    }
    }

    private void PlaceBuildings() {
	    // Usa il raycasting per trovare la posizione sulla griglia
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	  
	    if (Physics.Raycast(ray, out RaycastHit hit)) 
	    {  
		    
		    Vector3 gridOrigin = transform.position;
		  
		    int x = Mathf.FloorToInt((hit.point.x - gridOrigin.x) / cellWidth);
		    int y = Mathf.FloorToInt((hit.point.z - gridOrigin.z) / cellHeight);

		 Debug.Log($"Coordinate calcolate della griglia: ({x}, {y})");
		 
		 // Verifica che la posizione sia all'interno della griglia
		 if (x>= 0&& x<gridSizeX && y>= 0 && y<gridSizeY) 
		 {
			 // Controlla se la cella è già occupata
			 Debug.Log($"Clic all'interno della griglia: ({x}, {y})");
			 
				 Vector2Int cellPos = new Vector2Int(x, y);
				 
				 // Determina il prefab corretto in base al tipo di costruzione
				 GameObject prefabToPlace = null;
				 
				 // *** Controllo specifico per l'estrattore ***
				 if (currentBuildingType == BuidingType.Extractor) 
				 {
					 ResourceType resourceAtCell=GetResourceTypeAtCell(hit);
					 
					 // Verifica se la cella contiene una risorsa compatibile con l'estrattore
					 if (resourceAtCell != ResourceType.None)
					 {
						 prefabToPlace = extractorPrefab; // Può piazzare l'estrattore solo su risorse
					 }
					 else 
					 {
						 Debug.Log("Non puoi posizionare un estrattore su una cella senza risorsa!");
						 return;
					 }
				 }
				 else if (currentBuildingType == BuidingType.Machinery) 
				 {
					 prefabToPlace = machineryPrefab;  // Piazzamento libero per il macchinario
				 }
				 else if (currentBuildingType == BuidingType.Conveyor) {
					 prefabToPlace = conveyorPrefab;   // Piazzamento libero per il rullo
				 }
				 
				 
				 BuildingSize sizeData= prefabToPlace.GetComponent<BuildingSize>();
				 if (sizeData!=null) {
					 Vector2Int  buildingSize=new Vector2Int(sizeData.height, sizeData.width);
					 // Verifica che tutte le celle richieste siano libere

					 if (AreCellSAvaible(cellPos, buildingSize)) {
						 // Calcola la posizione centrale per il posizionamento
						 Vector3 centerPosition=CalculateCenterPosition(cellPos, buildingSize);
						 // Instanzia il prefab selezionato e memorizzalo nella griglia
						 if (prefabToPlace != null) {
							 
							 GameObject building = Instantiate(prefabToPlace, centerPosition, Quaternion.identity);
							 MarkCellsAsOccupied(cellPos, buildingSize, building);
						 }
					 }
					 else {
						 Debug.Log("Non c'è spazio sufficiente per posizionare il macchinario!");
					 }
				 }
				 else {
					 Debug.LogError("BuildingSize non trovato per il tipo: " + currentBuildingType);
				 }
		 }
		 else 
		 {
			 Debug.Log("Clic fuori dalla griglia.");
		 }
		 
	    }
	    
    }

  

    // Funzione per ottenere il tipo di risorsa presente in una cella specifica
    private ResourceType GetResourceTypeAtCell(RaycastHit hit) {
	    Resources resources = hit.collider.gameObject.GetComponent<Resources>();
	    if (resources != null) {
		    return resources.resourceType;
	    }
	    return ResourceType.None;  // Nessuna risorsa trovata
    }

    
    
    private bool AreCellSAvaible(Vector2Int startPos, Vector2Int buildingSize) {
	    for (int x = 0; x < startPos.x; x++) {
		    for (int y = 0; y < startPos.y; y++) {
			    Vector2Int cell=new Vector2Int(startPos.x+x, startPos.y+y);

			    if (cell.x>=gridSizeX||cell.y>=gridSizeY|| placebuildings[cell.x, cell.y] !=null) {
				    return false;  // Cella occupata o fuori dai confini
			    }
		    }
	    }
	    return true;
    }
    
     
    // Calcola la posizione centrale per posizionare il prefab al centro dell'area occupata
    private Vector3 CalculateCenterPosition(Vector2Int startPos, Vector2Int buildingSize) {
	    float centerX = (startPos.x + (buildingSize.x - 1) / 2f) * cellWidth;
	    float centerY = (startPos.y + (buildingSize.y - 1) / 2f) * cellHeight;
	    
	    return new Vector3(centerX, 0f, centerY);
    }
    
    private void MarkCellsAsOccupied(Vector2Int cellPos, Vector2Int size, GameObject building) {
	    for (int x = 0; x <size.x ; x++) {
		    for (int y = 0; y < size.y; y++) {
			    placebuildings[cellPos.x + x, cellPos.y + y] = building;
		    }
	    }
    }
}
