using System.Collections.Generic;

public class CardLibrary
{
    private List<CardDefinition> Cards { get; }

	public CardLibrary()
	{
		Cards = new List<CardDefinition>();
	}

	public void AddCard(CardDefinition cardDefinition)
		=> Cards.Add(cardDefinition);
	public void AddCards(IEnumerable<CardDefinition> cardDefinitions)
		=> Cards.AddRange(cardDefinitions);
}