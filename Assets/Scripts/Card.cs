public class Card
{
    public CardDefinition Type { get; }

	internal Card(CardDefinition type)
	{
		Type = type;
	}
}