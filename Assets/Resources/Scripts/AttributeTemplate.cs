using UnityEngine;

[CreateAssetMenu(fileName = "Attribute Template", menuName = "Objects/Attribute Template")]
public class AttributeTemplate : ScriptableObject {
	public string id;
	public int health, damage, speed;
	public bool isInvulnerable;
}
