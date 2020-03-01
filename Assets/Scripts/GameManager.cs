using System.Linq;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject BoardPrefab;
    public TextMeshProUGUI I_moneyTextMesh, I_economyTextMesh, I_environmentInpactTextMesh;

    private GameObject _root;
    private GameObject _boardObject;
    private GameObject _handObject;

    private Board _board;
    private Hand _hand;

    private CardLibrary _cardLibrary;

    private static int money, economicalPoints, environmentalPoints;
    public static TextMeshProUGUI moneyTextMesh, economyTextMesh, environmentTextMesh;

    public static int Money
    {
        get => money;
        set
        {
            money = value;
            moneyTextMesh.text = "";
            if (value != 0) moneyTextMesh.text += value;
        }
    }

    public static int EconomicalPoints
    {
        get => economicalPoints;
        set
        {
            economicalPoints = value;
            economyTextMesh.text = "";
            if (value != 0) economyTextMesh.text += value;
        }
    }

    public static int EnvironmentalPoints
    {
        get => environmentalPoints;
        set
        {
            environmentalPoints = value;
            environmentTextMesh.text = "";
            if (value != 0) environmentTextMesh.text += value;
        }
    }

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

        moneyTextMesh = I_moneyTextMesh;
        economyTextMesh = I_economyTextMesh;
        environmentTextMesh = I_environmentInpactTextMesh;

        Money = 100;
        EconomicalPoints = 101;
        EnvironmentalPoints = 102;
    }
}
