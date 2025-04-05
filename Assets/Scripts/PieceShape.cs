using UnityEngine;

[CreateAssetMenu(menuName = "Grid/PieceShape")]
public class PieceShape : ScriptableObject
{
    public Vector2Int[] localCells; // Relative positions (like [ (0,0), (1,0), (0,1) ])
}