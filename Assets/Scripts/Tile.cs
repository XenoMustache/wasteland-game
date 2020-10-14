using UnityEngine;

public class Tile : TurnBound {
	public bool occipied;
	public TileData data;
	public GameObject entity;

	SpriteRenderer spr;

	void Start() {
		spr = gameObject.GetComponent<SpriteRenderer>();

		gameObject.name = $"Tile ({data.rawName})";

		if (occipied && entity == null) Instantiate(Resources.Load("Prefabs/Indicator"), transform, false);
	}

	public override void OnTurn() { }
}
