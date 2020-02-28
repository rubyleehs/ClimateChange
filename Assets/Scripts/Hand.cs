using System.Collections.Generic;
using System.Linq;

public class Hand
{
    private List<Card> Cards { get; }

	public Hand(IEnumerable<Card> cards)
	{
		Cards = cards as List<Card> ?? cards.ToList();
	}
}