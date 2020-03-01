using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class Card : MonoBehaviour, IClickable
{
	public CardDefinition Type;

	private SpriteRenderer _spriteRenderer;
	private BoxCollider _boxCollider;

	public static Card CurrentlyDraggedCard { get; private set; }
	public static bool IsAnyCardBeingDragged => CurrentlyDraggedCard != null;
	public bool IsBeingDragged => CurrentlyDraggedCard == this;

	private Hand _handOnDragStart;
	public Hand Hand { get; private set; }
	public bool IsInHand => Hand != null;
	public bool WasPlayed { get; private set; } = false;

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

	public void PushToHand(Hand hand)
	{
		Hand = hand;
	}
	public void PopFromHand()
	{
		Hand?.Play(this);
		_handOnDragStart = Hand;
		Hand = null;
	}
	public void CancelDrag()
	{
		CurrentlyDraggedCard = null;
		_handOnDragStart.Give(this);
		Hand = _handOnDragStart;
	}
	public void PlayOn(Tile tile)
	{
		Debug.Log($"Card {Type} was played on tile at (x: {tile.Position.x}, y: {tile.Position.y}).");

		WasPlayed = true;

		CurrentlyDraggedCard = null;
		_handOnDragStart = null;
		Hand = null;

		var fromScale = transform.localScale;
		StartCoroutine(Animation.Tween(0.5f,
			(t) => { transform.localScale = Vector2.Lerp(fromScale, new Vector2(0, 0), t); },
			Animation.EaseInOutCubic));

		var gameManager = GameManager.Instance;
		foreach (var effect in Type.Effects)
		{
			gameManager.Board.AddCardEffect(effect, this, tile);
			effect.OnPlay(tile, Type);
		}
		gameManager.EndTurn();
	}

	void IClickable.OnClickDown()
	{
		if (IsAnyCardBeingDragged && !IsBeingDragged) return;

		CurrentlyDraggedCard = this;

		if (IsInHand) PopFromHand();

		var fromScale = transform.localScale;
		StartCoroutine(Animation.Tween(0.5f,
			(t) => { transform.localScale = Vector2.Lerp(fromScale, new Vector2(8, 8), t); },
			Animation.EaseInOutCubic));

		StartCoroutine(OnDrag());
	}
	void IClickable.OnClick() { }
	IEnumerator OnDrag()
	{
		while (!Input.GetMouseButtonUp(0))
		{
			transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(8, -18, 0), 0.08f);
			yield return null;
		}

		Debug.Log("Drag cancelled");
		if (IsBeingDragged) CurrentlyDraggedCard = null;
		if (!WasPlayed) CancelDrag();
	}
	void IClickable.OnClickUp() { }
}