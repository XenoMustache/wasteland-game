using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public Vector2 size;

	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public GameObject[,] tiles;
	[HideInInspector]
	public List<GameObject> entities;

	void Start() {
		entities = new List<GameObject>();
		Generate((int)size.x, (int)size.y);
	}

	public void Turn() {
		for (var i = 0; i < entities.Count; i++) {
			entities[i].GetComponent<TurnBound>().OnTurn();
		}
	}

	void Generate(int length, int width) {
		tiles = new GameObject[length, width];

		for (var i = 0; i < length; i++) {
			for (var j = 0; j < width; j++) {
				CreateTile(i, j);
			}
		}

		SpawnPlayer(new Vector2(18, 18));
		SpawnEnemy(new Vector2(16, 16));
		SpawnEnemy(new Vector2(17, 16));
		SpawnEnemy(new Vector2(18, 16));

		SpawnProjectile(new Vector2(1, 1), new Vector2(18, 18));

		tiles[15, 15].GetComponent<Tile>().occipied = true;
	}

	void CreateTile(int xPos, int yPos) {
		tiles[xPos, yPos] = Instantiate((GameObject)Resources.Load("Prefabs/Tile"), new Vector3(xPos, yPos), new Quaternion(), transform);
		tiles[xPos, yPos].GetComponent<Tile>().occipied = false;
	}

	void SpawnPlayer(Vector2 position) {
		player = Instantiate((GameObject)Resources.Load("Prefabs/Player"));
		player.GetComponent<PlayerController>().worldController = this;
		player.GetComponent<PlayerController>().startingPosition = position;
	}

	void SpawnEnemy(Vector2 position) {
		var enemy = Instantiate((GameObject)Resources.Load("Prefabs/Enemy"));

		enemy.GetComponent<EnemyController>().worldController = this;
		enemy.GetComponent<EnemyController>().player = player;
		enemy.GetComponent<EnemyController>().startingPosition = position;

		entities.Add(enemy);
	}

	public void SpawnProjectile(Vector2 startingPos, Vector2 targetPos) {
		var proj = Instantiate((GameObject)Resources.Load("Prefabs/Projectile"), startingPos, new Quaternion(), null).GetComponent<Projectile>();

		proj.wc = this;
		proj.startingPos = startingPos;
		proj.targetPos = targetPos;
	}
}
