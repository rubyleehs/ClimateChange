using System;
using System.Collections.Generic;
using System.Linq;

public class Deck
{
    private Stack<Card> Cards { get; }

	public Deck(CardLibrary cardLibrary, int numberOfCards)
	{
		var cardTypes = cardLibrary.Cards;
		int nFullDecks = (int)Math.Floor((double)numberOfCards / cardTypes.Count);
		int nCardsInIncompleteDeck = numberOfCards % cardTypes.Count;

		Random random = new Random();

		Cards = new Stack<Card>(
			Enumerable.Concat(
				// Full decks
				Enumerable.Range(1, nFullDecks).SelectMany(i => cardTypes.Select(c => c.CreateCard())),
				// Last, incomplete deck
				cardTypes.OrderBy(c => random.Next()).Take(nCardsInIncompleteDeck).Select(c => c.CreateCard()))
			.OrderBy(c => random.Next()));
	}

	public Card Give(Hand hand)
	{
		var card = Cards.Pop();
		hand.Give(card);
		return card;
	}
}