using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Card : MonoBehaviour, IClickable
{
	public CardDefinition Type;

	private SpriteRenderer _spriteRenderer;
	private BoxCollider _boxCollider;

	public static bool IsAnyCardBeingDragged { get; private set; }
	public bool IsBeingDragged { get; private set; }

	private Hand _handOnDragStart;
	public Hand Hand { get; private set; }
	public bool IsInHand => Hand != null;

	public void Init(CardDefinition type, Hand hand = null)
	{
		Type = type;
		Hand = hand;
	}

	void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.sprite = Type.Sprite;
		_spriteRenderer.sortingLayerName = "Hand";

		_boxCollider = gameObject.GetComponent<BoxCollider>() ?? gameObject.AddComponent<BoxCollider>();
		_boxCollider.size = new Vector2(0.4f, 0.58f);
	}

	public void Play()
	{
		Hand?.Play(this);
		_handOnDragStart = Hand;
		Hand = null;
	}
	public void Release()
	{
		_handOnDragStart.Give(this);
		Hand = _handOnDragStart;
	}

	void IClickable.OnClick()
	{
		Debug.Log($"Dragging {Type}");

		if (IsAnyCardBeingDragged && !IsBeingDragged) return;

		IsBeingDragged = true;
		IsAnyCardBeingDragged = true;

		if (IsInHand) Play();

		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
	void IClickable.OnClickDown() { }
	void IClickable.OnClickUp()
	{
		Debug.Log($"Mouse up");

		if (IsBeingDragged)
		{
			IsBeingDragged = false;
			IsAnyCardBeingDragged = false;
		}

		Release();
	}
}