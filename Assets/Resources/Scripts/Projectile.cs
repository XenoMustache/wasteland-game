using UnityEngine;

public class Projectile : MonoBehaviour {
	public float accuracy = 1, speed = 5;
	public int damage = 5;
	public Vector2 targetPos;
	public Tile targetTile;

	SpriteRenderer spr;
	Transform tr;

	void Start() {
		spr = gameObject.GetComponent<SpriteRenderer>();
		tr = transform;
	}

	void Update() {
		if ((Vector2)tr.position != targetPos) tr.position = Vector3.MoveTowards(tr.position, targetPos, Time.deltaTime * speed);
		else {
			targetTile.entity.GetComponent<AttributeHandler>().data.health -= damage;
			Destroy(gameObject);
		}
	}
}
