using UnityEngine;

public class Card : ScriptableObject
{
    public CardDefinition Type { get; }

	internal Card(CardDefinition type)
	{
		Type = type;
	}
}