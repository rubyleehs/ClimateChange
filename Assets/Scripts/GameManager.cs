using System.Linq;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject BoardPrefab;
    public TextMeshProUGUI I_moneyTextMesh, I_economyTextMesh, I_environmentInpactTextMesh;

    private GameObject _root;
    private GameObject _boardObject;
    private GameObject _handObject;

    public Board Board { get; private set; }
    public Deck Deck { get; private set; }
    public Hand Hand { get; private set; }
    public CardLibrary CardLibrary { get; private set; }

    private CardLibrary _cardLibrary;

    private static int money, economicalPoints, environmentalPoints;
    public static TextMeshProUGUI moneyTextMesh, economyTextMesh, environmentTextMesh;

    private static int _money;
    public static int Money
    {
        get => _money;
        set
        {
            _money = value;
            moneyTextMesh.text = "";
            if (value != 0) moneyTextMesh.text += value;
        }
    }

    private static int _economicPoints;
    public static int EconomicPoints
    {
        get => _economicPoints;
        set
        {
            _economicPoints = value;
            economyTextMesh.text = "";
            if (value != 0) economyTextMesh.text += value;
        }
    }

    private static int _environmentalPoints;
    public static int EnvironmentalPoints
    {
        get => _environmentalPoints;
        set
        {
            _environmentalPoints = value;
            environmentTextMesh.text = "";
            if (value != 0) environmentTextMesh.text += value;
        }
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        Instance = this;

        _root = gameObject;

        CardLibrary = new CardLibrary("Cards");

        _boardObject = Instantiate(BoardPrefab, _root.transform);
        Board = _boardObject.GetComponent<Board>();

        _handObject = new GameObject("Hand");
        _handObject.transform.SetParent(_root.transform);
        Hand = _handObject.AddComponent<Hand>();
        Hand.Init(Camera.main);
        Hand.transform.SetPositionAndRotation(new Vector3(0, 20f, 0), Quaternion.Euler(45, 45, 0));
        Hand.transform.localScale = new Vector2(2.5f, 2.5f);

        moneyTextMesh = I_moneyTextMesh;
        economyTextMesh = I_economyTextMesh;
        environmentTextMesh = I_environmentInpactTextMesh;

        Money = 100;
        EconomicPoints = 101;
        EnvironmentalPoints = 102;
    }
}
