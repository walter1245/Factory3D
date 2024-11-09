using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
	public GridBuidingSystem gridBuildingSystem;
	public Color gridColor = Color.cyan;

	private void OnDrawGizmos()
	{
		
		
			if (gridBuildingSystem == null)
			{
				Debug.LogWarning("GridBuildingSystem non è assegnato a GridVisualizer.");
				return;
			}

			Gizmos.color = gridColor;

			int gridSizeX = gridBuildingSystem.gridSizeX;
			int gridSizeY = gridBuildingSystem.gridSizeY;
			float cellWidth = gridBuildingSystem.cellWidth;
			float cellHeight = gridBuildingSystem.cellHeight;

			// Posizione di base della griglia
			Vector3 gridOrigin = gridBuildingSystem.transform.position;

			// Disegna linee verticali
			for (int x = 0; x <= gridSizeX; x++)
			{
				float posX = x * cellWidth + gridOrigin.x;
				Gizmos.DrawLine(new Vector3(posX, 0, gridOrigin.z), new Vector3(posX, 0, gridSizeY * cellHeight + gridOrigin.z));
			}

			// Disegna linee orizzontali
			for (int y = 0; y <= gridSizeY; y++)
			{
				float posY = y * cellHeight + gridOrigin.z;
				Gizmos.DrawLine(new Vector3(gridOrigin.x, 0, posY), new Vector3(gridSizeX * cellWidth + gridOrigin.x, 0, posY));
			}
		
	}
}
