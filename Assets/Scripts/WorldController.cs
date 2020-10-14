using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {
	public Vector2 size;
	public GameObject player;
	public GameObject[,] tiles;
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

		player = Instantiate((GameObject)Resources.Load("Prefabs/Player"));
		player.GetComponent<PlayerController>().worldController = this;

		SpawnEnemy(new Vector2(16, 16));
		SpawnEnemy(new Vector2(17, 16));
		SpawnEnemy(new Vector2(18, 16));

		tiles[15, 15].GetComponent<Tile>().occipied = true;
	}

	void CreateTile(int xPos, int yPos) {
		tiles[xPos, yPos] = Instantiate((GameObject)Resources.Load("Prefabs/Tile"), new Vector3(xPos, yPos), new Quaternion(), transform);
		tiles[xPos, yPos].GetComponent<Tile>().occipied = false;
	}

	void SpawnEnemy(Vector2 position) {
		var enemy = Instantiate((GameObject)Resources.Load("Prefabs/Enemy"));

		enemy.GetComponent<EnemyController>().worldController = this;
		enemy.GetComponent<EnemyController>().player = player;
		enemy.GetComponent<EnemyController>().startingPosition = position;

		entities.Add(enemy);
	}
}
