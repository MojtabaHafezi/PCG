using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WeaponComponents : MonoBehaviour
{

	public Sprite[] modules;
	
	private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.sprite = modules [Random.Range (0, modules.Length)];
	}

	public void EnableSpriteRenderer ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();

	}

	public SpriteRenderer getSpriteRenderer ()
	{
		return spriteRenderer;
	}
}
