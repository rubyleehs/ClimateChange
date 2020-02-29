using System.Linq;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject BoardPrefab;

    private GameObject _root;
    private GameObject _boardObject;
    private GameObject _handObject;

    private Board _board;
    private Hand _hand;

    private CardLibrary _cardLibrary;

    public static int money, economy, environmentImpact;
    public TextMeshProUGUI moneyTextMesh, economyTextMesh, environmentInpactTextMesh;

    void Awake()
    {
        _root = gameObject;

        _cardLibrary = new CardLibrary("Cards");

        _boardObject = Instantiate(BoardPrefab, _root.transform);
        _board = _boardObject.GetComponent<Board>();

        _handObject = new GameObject("Hand");
        _handObject.transform.SetParent(_root.transform);
        _hand = _handObject.AddComponent<Hand>();
        _hand.Init(Camera.main);
        _hand.transform.SetPositionAndRotation(new Vector3(0, 17.5f, 0), Quaternion.Euler(45, 45, 0));
        _hand.transform.localScale = new Vector2(2.5f, 2.5f);
        foreach (var cardType in _cardLibrary.Cards)
        {
            var card = _hand.GiveFrom(cardType);
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
        }

        SetMoney(100);
        SetEconomy(101);
        SetEnvironmentImpact(102);
    }

    public void SetMoney(int value)
    {
        money = value;
        moneyTextMesh.text = "" + value;
    }

    public void SetEconomy(int value)
    {
        economy = value;
        economyTextMesh.text = "" + value;
    }

    public void SetEnvironmentImpact(int value)
    {
        environmentImpact = value;
        environmentInpactTextMesh.text = "" + value;
    }
}
