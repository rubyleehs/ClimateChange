using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardLibrary
{
    private List<CardDefinition> Cards { get; }

	public CardLibrary(string path)
	{
		Cards = Resources.LoadAll<CardDefinition>(path).ToList();
	}

	public void AddCard(CardDefinition cardDefinition)
		=> Cards.Add(cardDefinition);
	public void AddCards(IEnumerable<CardDefinition> cardDefinitions)
		=> Cards.AddRange(cardDefinitions);
}