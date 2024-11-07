using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class MapEditorTool : EditorWindow
{
    private Vector2Int gridSize = new Vector2Int(10, 10); // Dimensione della griglia
    private float cellSize = 40f; // Dimensione di ogni cella in pixel
    private Vector2 scrollPosition; // Per lo scroll della griglia

    // Dizionario per memorizzare le risorse piazzate
    private Dictionary<Vector2Int, ResourceSpot> placedResources = new();

    // Parametri di configurazione
    private ResourceType currentResourceType = ResourceType.None;
    private int resourceCellCount = 1;

    public GridData gridData;
    

    // Dizionario per memorizzare i colori delle risorse
    private Dictionary<ResourceType, Color> resourceColors = new()
    {
        { ResourceType.None, Color.white },
        { ResourceType.Gas, Color.green },
        { ResourceType.Rock, Color.blue },
        { ResourceType.Liquid, Color.red }
    };

    [MenuItem("Tools/Map Editor")]
    public static void OpenWindow()
    {
        GetWindow<MapEditorTool>("Map Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("Map Editor Tool", EditorStyles.boldLabel);

        gridData = EditorGUILayout.ObjectField("Grid Data", gridData, typeof(GridData), false) as GridData;
        // Parametri di configurazione della griglia e delle risorse
        gridSize = EditorGUILayout.Vector2IntField("Grid Size", gridSize);
        cellSize = EditorGUILayout.FloatField("Cell Size (pixels)", cellSize);
        currentResourceType = (ResourceType)EditorGUILayout.EnumPopup("Resource Type", currentResourceType);
        resourceCellCount = EditorGUILayout.IntField("Resource Cells Count", resourceCellCount);

        // Selettori di colore per ogni tipo di risorsa
        GUILayout.Label("Resource Colors", EditorStyles.boldLabel);
        foreach (ResourceType type in System.Enum.GetValues(typeof(ResourceType)))
        {
            if (type != ResourceType.None)
            {
                resourceColors[type] = EditorGUILayout.ColorField($"{type} Color", resourceColors[type]);
            }
        }

        if (GUILayout.Button("Generate Resources")) {
            GenerateResources();
        }
        
        if (GUILayout.Button("Clear Grid"))
        {
            placedResources.Clear();
        }

        if (GUILayout.Button("Save Grid Data")) {
            SaveGridData();
        }

        if (GUILayout.Button("Load Grid Data")) {
            LoadGridData();
        }
        
        GUILayout.Space(10);

        // Inizia la sezione scrollabile della griglia
        scrollPosition = GUILayout.BeginScrollView(
            scrollPosition, 
            GUILayout.Width(position.width),
            GUILayout.Height(position.height-200));
        
        GUILayout.BeginHorizontal();
        GUILayout.Space(gridSize.x * cellSize);
        
        GUILayout.BeginVertical();
        GUILayout.Space(gridSize.y*cellSize);
        
        // Disegna la griglia
        DrawGrid();
        
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        
        GUILayout.EndScrollView();
    }

   

    private void DrawGrid()
    {
        // Disegna le celle della griglia
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Vector2Int cellPos = new Vector2Int(x, y);
                Rect cellRect = new Rect(x * cellSize, y * cellSize, cellSize, cellSize);

                // Se c'è una risorsa in questa cella, disegna con il colore corrispondente
                if (placedResources.TryGetValue(cellPos, out ResourceSpot spot))
                {
                    EditorGUI.DrawRect(cellRect, resourceColors[spot.type]);
                }
                else
                {
                    // Colore di sfondo per celle vuote
                    EditorGUI.DrawRect(cellRect, new Color(0.9f, 0.9f, 0.9f, 0.3f));
                }

                // Disegna il bordo della cella
                Handles.DrawSolidRectangleWithOutline(cellRect, Color.clear, Color.black);
            }
        }
        
    }
    
    private void SaveGridData() {
        if (gridData == null) {
            return;
        }
        gridData.gridSize = gridSize;
        gridData.resourceCells.Clear();

        foreach (KeyValuePair<Vector2Int, ResourceSpot> kvp in placedResources) {
            gridData.resourceCells.Add(new GridData.ResourceCell((GridData.ResourceType)kvp.Value.type, kvp.Key));
        }
        EditorUtility.SetDirty(gridData); // Segna lo ScriptableObject come modificato
        AssetDatabase.SaveAssets(); // Salva le modifiche su disco
    }
    
    
    private void LoadGridData() {
        if (gridData == null) {
            return;
        }
        gridData.gridSize = gridSize;
        placedResources.Clear();
        foreach (var cell in gridData.resourceCells)
        {
            placedResources[cell.position] = new ResourceSpot((ResourceType)cell.type, cell.position);
        }

        Repaint();
    }
    
    
    private void GenerateResources() {
        Vector2Int startCell = GetRandomFreeCells();// Trova una cella iniziale libera
        List<Vector2Int> cells = new List<Vector2Int> { startCell };
        
       Queue<Vector2Int> cellQueue = new Queue<Vector2Int>();
       cellQueue.Enqueue(startCell);

       while (cells.Count < resourceCellCount && cellQueue.Count > 0) {
           Vector2Int currentCell = cellQueue.Dequeue();
           Vector2Int[] directions = new Vector2Int[]
           {
               new Vector2Int(0, 1),  // Su
               new Vector2Int(1, 0),  // Destra
               new Vector2Int(0, -1), // Giù
               new Vector2Int(-1, 0)  // Sinistra
           };

           foreach (var dir in directions)
           {
               Vector2Int nextCell = currentCell + dir;

               if (IsWithinGrid(nextCell) && !placedResources.ContainsKey(nextCell) && !cells.Contains(nextCell))
               {
                   cells.Add(nextCell);
                   cellQueue.Enqueue(nextCell);

                   if (cells.Count >= resourceCellCount)
                       break; // Interrompi se hai piazzato tutte le celle
               }
           }
       }
       
       if (cells.Count < resourceCellCount)
       {
           Debug.LogWarning("Non ci sono abbastanza celle disponibili per posizionare tutte le risorse.");
       }

       foreach (var cell in cells)
       {
           placedResources[cell] = new ResourceSpot(currentResourceType, cell);
       }
       
       
        Repaint(); // Aggiorna la finestra
    }

   

    private Vector2Int GetRandomFreeCells() {
        Vector2Int cell;
        do {
            int x = Random.Range(0, gridSize.x);
            int y = Random.Range(0, gridSize.y);
            cell = new Vector2Int(x, y);
        }
        while (placedResources.ContainsKey(cell)); // Cerca fino a trovare una cella libera
        return cell;
    }
    
   
    
    
    private bool IsWithinGrid(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < gridSize.x && cell.y >= 0 && cell.y < gridSize.y;
    }
}

// Enum per i tipi di risorsa
public enum ResourceType
{
    None,
    Gas,
    Liquid,
    Rock,
}

// Struttura che rappresenta uno spot per le risorse
public struct ResourceSpot
{
    public ResourceType type;
    public Vector2Int position;

    public ResourceSpot(ResourceType type, Vector2Int position)
    {
        this.type = type;
        this.position = position;
    }
}