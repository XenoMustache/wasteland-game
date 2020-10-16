using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : TurnBound {
	public float lerpSpeed = 5, maxMoveTime = 0;
	public Sprite[] sprites;
	public Vector2 startingPosition, worldPosition;

	[HideInInspector]
	public int damage, health;
	[HideInInspector]
	public WorldController worldController;
	[HideInInspector]
	public GameObject player;

	float range = 5, moveTime;
	int facing = 0;
	SpriteRenderer spr;
	Vector3 pos;
	Transform tr;
	GameObject prevTile, curTile;
	AttributeTemplate data;

	List<GameObject> neighborTiles;

	public override void OnTurn() {
		if (moveTime > 0) {
			moveTime--;
		}
		else {
			if (Vector2.Distance(player.transform.position, transform.position) < range) {
				if (Vector2.Distance(player.transform.position, transform.position) > 1) {
					Move(true);
				}
				else {
					Attack(damage, player);
				}
			}
			else Move(false);
			moveTime = maxMoveTime;
		}

		UpdateNeighborTiles();
	}

	void Start() {
		data = GetComponent<AttributeHandler>().data;

		health = data.health;
		damage = data.damage;

		moveTime = maxMoveTime;

		spr = gameObject.GetComponent<SpriteRenderer>();
		pos = transform.position;
		tr = transform;

		transform.position = startingPosition;
		pos = startingPosition;
		worldPosition = startingPosition;

		curTile = worldController.tiles[(int)startingPosition.x, (int)startingPosition.y];
		curTile.GetComponent<Tile>().entity = gameObject;
		curTile.GetComponent<Tile>().occipied = true;

		UpdateNeighborTiles();
	}

	void Update() {
		spr.sprite = sprites[facing];
		tr.position = Vector3.MoveTowards(tr.position, pos, Time.deltaTime * lerpSpeed);

		if (health <= 0) Death();
	}

	void Move(bool followPlayer) {
		prevTile = curTile;

		if (followPlayer) {
			var distances = new List<float>();

			for (var i = 0; i < neighborTiles.Count; i++) {
				distances.Add(Vector2.Distance(neighborTiles[i].transform.position, player.transform.position));
			}

			var lowest = distances.IndexOf(distances.Min());

			if (neighborTiles[lowest].GetComponent<Tile>().entity != player) {
				if (neighborTiles[lowest].GetComponent<Tile>().occipied == false) {
					pos += GetDirection(lowest);

					curTile = worldController.tiles[(int)worldPosition.x, (int)worldPosition.y];
					curTile.GetComponent<Tile>().entity = gameObject;
					curTile.GetComponent<Tile>().occipied = true;

					prevTile.GetComponent<Tile>().entity = null;
					prevTile.GetComponent<Tile>().occipied = false;
				}
			}
			//else Attack(damage, player);
		}
	}

	void UpdateNeighborTiles() {
		neighborTiles = new List<GameObject>() {
			worldController.tiles[(int)worldPosition.x, (int)worldPosition.y + 1], // up
			worldController.tiles[(int)worldPosition.x, (int)worldPosition.y - 1], // down 
			worldController.tiles[(int)worldPosition.x - 1, (int)worldPosition.y], // left
			worldController.tiles[(int)worldPosition.x + 1, (int)worldPosition.y], // right
		};
	}

	void Death() {
		curTile.GetComponent<Tile>().entity = null;
		curTile.GetComponent<Tile>().occipied = false;
		worldController.GetComponent<WorldController>().entities.Remove(gameObject);
		Destroy(gameObject);
	}

	void Attack(int dmg, GameObject target) {
		if (target != null)
			if (!target.GetComponent<AttributeHandler>().data.isInvulnerable)
				target.GetComponent<PlayerController>().health -= dmg;
	}

	Vector3 GetDirection(int index) {
		switch (index) {
			default: return Vector3.zero;
			case 0: worldPosition.y += 1; return Vector3.up;
			case 1: worldPosition.y -= 1; return Vector3.down;
			case 2:
				facing = 0;
				worldPosition.x -= 1;
				return Vector3.left;
			case 3:
				facing = 1;
				worldPosition.x += 1;
				return Vector3.right;
		}
	}
}
