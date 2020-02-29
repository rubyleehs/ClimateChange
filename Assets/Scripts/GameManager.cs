using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject BoardPrefab;

    private GameObject _root;
    private GameObject _boardObject;
    private GameObject _handObject;

    private Board _board;
    private Hand _hand;

    private CardLibrary _cardLibrary;

    public static float money, economy, enviromentalImpact;

    void Awake()
    {
        _root = gameObject;

        _cardLibrary = new CardLibrary("Cards");

        _boardObject = Instantiate(BoardPrefab, _root.transform);
        _board = _boardObject.GetComponent<Board>();

        _handObject = new GameObject("Hand");
        _handObject.transform.SetParent(_root.transform);
        _hand = _handObject.AddComponent<Hand>();
        _hand.Init();
        foreach (var cardType in _cardLibrary.Cards) _hand.GiveFrom(cardType);
    }
}
