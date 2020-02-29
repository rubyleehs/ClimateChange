using System.Collections.Generic;
using System.Linq;

public class Hand
{
    private List<Card> _cards { get; }

	public IReadOnlyCollection<Card> Cards
	{
		get => _cards.AsReadOnly();
	}

	public Hand() : this(new List<Card>()) { }
	public Hand(IEnumerable<Card> cards)
	{
		_cards = cards as List<Card> ?? cards.ToList();
	}

	public void Give(Card card)
		=> _cards.Add(card);
	public Card Play(Card card)
	{
		_cards.Remove(card);
		return card;
	}
}