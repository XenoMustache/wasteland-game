using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float lerpSpeed = 5;
	public Sprite[] sprites;
	public Vector2 startingPosition, worldPosition;
	
	[HideInInspector]
	public int damage, health;
	[HideInInspector]
	public WorldController worldController;
	[HideInInspector]
	public TextMeshProUGUI textHealth;
	[HideInInspector]
	public AttributeTemplate data;

	int facing = 0;
	SpriteRenderer spr;
	Vector3 pos;
	Transform tr;
	GameObject prevTile, curTile;

	void Start() {
		data = GetComponent<AttributeHandler>().data;

		health = data.health;
		damage = data.damage;
		spr = gameObject.GetComponent<SpriteRenderer>();
		tr = transform;

		textHealth = GameObject.Find("Text Health").GetComponent<TextMeshProUGUI>();

		transform.position = startingPosition;
		pos = startingPosition;
		worldPosition = startingPosition;

		curTile = worldController.tiles[(int)startingPosition.x, (int)startingPosition.y];
		curTile.GetComponent<Tile>().entity = gameObject;
	}

	void Update() {
		textHealth.text = $"Health: {health}";

		if (Input.GetKeyDown(KeyCode.A)) Move(Vector3.left);
		if (Input.GetKeyDown(KeyCode.D)) Move(Vector3.right);
		if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.up);
		if (Input.GetKeyDown(KeyCode.S)) Move(Vector3.down);

		spr.sprite = sprites[facing];
		tr.position = Vector3.MoveTowards(tr.position, pos, Time.deltaTime * lerpSpeed);

		if (health <= 0) Death();
	}

	void Move(Vector3 translation) {
		bool canMove = false;

		if (translation == Vector3.up) canMove = CheckMovement(0, 1);
		else if (translation == Vector3.down) canMove = CheckMovement(0, -1);
		else if (translation == Vector3.left) {
			facing = 0;
			canMove = CheckMovement(-1, 0);
		}
		else if (translation == Vector3.right) {
			facing = 1;
			canMove = CheckMovement(1, 0);
		}

		if (canMove) pos += translation;
	}

	void Death() {
		curTile.GetComponent<Tile>().entity = null;
		curTile.GetComponent<Tile>().occipied = false;

		worldPosition = startingPosition;
		Start();
	}

	void Attack(int dmg, GameObject target) {
		if (target != null)
			if (!target.GetComponent<AttributeHandler>().data.isInvulnerable)
				target.GetComponent<EnemyController>().health -= dmg;
	}

	bool CheckMovement(int xOffset, int yOffset) {
		var tile = worldController.GetComponent<WorldController>().tiles[(int)worldPosition.x + xOffset, (int)worldPosition.y + yOffset];

		if (tile) {
			if (worldPosition.x + xOffset < worldController.GetComponent<WorldController>().size.x - 1 && worldPosition.y + yOffset < worldController.GetComponent<WorldController>().size.y - 1 &&
					worldPosition.y + yOffset > 0 && worldPosition.x + xOffset > 0) {
				if (!tile.GetComponent<Tile>().occipied && tile.GetComponent<Tile>().entity == null) {
					worldPosition += new Vector2(xOffset, yOffset);

					tile.GetComponent<Tile>().entity = gameObject;

					prevTile = curTile;
					curTile = tile.gameObject;

					prevTile.GetComponent<Tile>().entity = null;
					
					worldController.Turn();
					
					return true;
				}
				else {
					Attack(damage, tile.GetComponent<Tile>().entity);
					
					worldController.Turn();
					
					return false;
				};
			}
			else return false;
		}
		else return false;
	}
}
