using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public float cellSize=1f;
    
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
		 int x= Mathf.FloorToInt(hit.point.x/cellSize); 
		 int y = Mathf.FloorToInt(hit.point.y/cellSize);
		 
		 // Verifica che la posizione sia all'interno della griglia
		 if (x>= 0&& x<gridSizeX && y>= 0 && y<gridSizeY) 
		 {
			 // Controlla se la cella è già occupata
			 if (placebuildings[x,y] == null) 
			 {
				 Vector2Int cellPos = new Vector2Int(x, y);
				 
				 // Determina il prefab corretto in base al tipo di costruzione
				 GameObject prefabToPlace = null;
				 
				 // *** Controllo specifico per l'estrattore ***
				 if (currentBuildingType == BuidingType.Extractor) 
				 {
					 ResourceType resourceAtCell=GetResourceTypeAtCell(cellPos);
					 
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
				 
				 // Instanzia il prefab selezionato e memorizzalo nella griglia
				 if (prefabToPlace != null) {
					 Vector3 position=  new Vector3(x * cellSize, 0, y * cellSize);
					 GameObject building = Instantiate(prefabToPlace, position, Quaternion.identity);
					 placebuildings[x,y] = building;
				 }
			 }
			 else
			 {
				 Debug.Log("La cella è già occupata!"); 
			 }
		 }
		 else 
		 {
			 Debug.Log("Clic fuori dalla griglia.");
		 }
		 
	    }
	    
    }
    // Funzione per ottenere il tipo di risorsa presente in una cella specifica
    private ResourceType GetResourceTypeAtCell(Vector2Int cellPos) {
	    foreach (var resourceCell in gridData.resourceCells) {
		    if (resourceCell.position==cellPos) {
			    return resourceCell.type;
		    }
	    }
	    return ResourceType.None;  // Nessuna risorsa trovata
    }

}
