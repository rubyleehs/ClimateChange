using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class Hand : MonoBehaviour
{
	private Camera _camera;

    private List<Card> _cards { get; set; }
	public IReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

	public void Init(Camera camera)
	{
		_cards = new List<Card>();

		_camera = camera;
		gameObject.layer = LayerMask.NameToLayer("UI");
	}
	
	public Card Give(Card card)
	{
		card.transform.SetParent(this.transform);
		card.gameObject.layer = LayerMask.NameToLayer("UI");

		card.PushToHand(this);
		_cards.Add(card);
		ArrangeCards();
		return card;
	}
	public void Give(IEnumerable<Card> cards)
	{
		foreach (var card in cards)
			Give(card);
	}
	public Card GiveFrom(CardDefinition cardType)
	{
		var card = new GameObject("Card").AddComponent<Card>();
		card.Init(cardType, this);
		return Give(card);
	}

	public Card Play(Card card)
	{
		_cards.Remove(card);
		ArrangeCards();
		return card;
	}

	private void ArrangeCards()
	{
		for (int i = 0; i < _cards.Count; i++)
		{
			var card = _cards[i];
			var offset = (i - ((_cards.Count - 1) / 2f));
			var factor = 1 / (float)Math.Pow(_cards.Count, 1 / 2);

			card.GetComponent<SpriteRenderer>().sortingOrder = i;

			var fromPosition = card.transform.localPosition;
			StartCoroutine(Animation.Tween(1f,
				(t) => { card.transform.localPosition = Vector2.Lerp(fromPosition, new Vector2(offset * (3 * factor), (-Math.Abs(offset) + Math.Abs(offset) / 2) * (float)(0.5 * factor)), t); },
				Animation.EaseInOutCubic));
			var fromRotation = card.transform.localRotation;
			StartCoroutine(Animation.Tween(1f,
				(t) => { card.transform.localRotation = Quaternion.Lerp(fromRotation, Quaternion.Euler(0, 0, -offset * (6 * factor)), t); },
				Animation.EaseInOutCubic));
			var fromScale = card.transform.localScale;
			StartCoroutine(Animation.Tween(0.5f,
				(t) => { card.transform.localScale = Vector2.Lerp(fromScale, new Vector2(15, 15), t); },
				Animation.EaseInOutCubic));
		}
	}
}