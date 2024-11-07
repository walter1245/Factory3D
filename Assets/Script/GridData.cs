using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGridData", menuName = "Grid Data", order = 1)]
public class GridData : ScriptableObject
{
    public Vector2Int gridSize;
    public List<ResourceCell> resourceCells;

    [System.Serializable]
    public struct ResourceCell {

        public ResourceType type;
        public Vector2Int position;

        public ResourceCell(ResourceType type, Vector2Int position) {
            this.type = type;
            this.position = position;
        }

    }

    public enum ResourceType {
        None,
        Gas,
        Liquid,
        Rock
    }
        
}


