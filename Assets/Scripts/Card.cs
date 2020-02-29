using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Card : MonoBehaviour
{
	public CardDefinition Type;

	private SpriteRenderer _spriteRenderer;

	public void Init(CardDefinition type)
	{
		Type = type;
	}

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = Type.Sprite;
	}
}