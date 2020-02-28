using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : ScriptableObject
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