using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private List<Card> _cards { get; set; }
	public IReadOnlyCollection<Card> Cards => _cards.AsReadOnly();

	public void Init() => Init(new List<Card>());
	public void Init(IEnumerable<Card> cards)
	{
		_cards = cards as List<Card> ?? cards.ToList();
	}

	public void Give(Card card)
	{
		card.transform.SetParent(this.transform);
		_cards.Add(card);
	}
	public void Give(IEnumerable<Card> cards)
	{
		foreach (var card in cards)
			Give(card);
	}
	public void GiveFrom(CardDefinition cardType)
	{
		var card = new GameObject("Card").AddComponent<Card>();
		card.Init(cardType);
		Give(card);
	}

	public Card Play(Card card)
	{
		_cards.Remove(card);
		return card;
	}
}