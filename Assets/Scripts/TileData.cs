using UnityEngine;

[CreateAssetMenu(fileName = "Tile", menuName = "Objects/Tile")]
public class TileData : ScriptableObject {
	public string rawName;
	public Sprite sprite;
}
