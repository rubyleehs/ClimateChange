using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardLibrary
{
    private List<CardDefinition> _cards { get; }
	public IReadOnlyCollection<CardDefinition> Cards => _cards.AsReadOnly();

	public CardLibrary(string path)
	{
		_cards = Resources.LoadAll<CardDefinition>(path).ToList();
	}

	public void AddCard(CardDefinition cardDefinition)
		=> _cards.Add(cardDefinition);
	public void AddCards(IEnumerable<CardDefinition> cardDefinitions)
		=> _cards.AddRange(cardDefinitions);
}