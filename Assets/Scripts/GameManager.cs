using System.Linq;
using UnityEngine;
using TMPro;

public enum GameState { Start, TurnStart, Turn, TurnEnd };

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

    public GameState State { get; private set; }

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

        Deck = new Deck(CardLibrary, 80);

        _handObject = new GameObject("Hand");
        _handObject.transform.SetParent(Camera.main.transform);
        Hand = _handObject.AddComponent<Hand>();
        Hand.Init(Camera.main);
        Hand.transform.localPosition = new Vector3(0, -40, 1);
        Hand.transform.localRotation = Quaternion.identity;
        Hand.transform.localScale = new Vector2(2.5f, 2.5f);

        State = GameState.Start;

        moneyTextMesh = I_moneyTextMesh;
        economyTextMesh = I_economyTextMesh;
        environmentTextMesh = I_environmentInpactTextMesh;

        Money = 2500;
        EconomicPoints = 0;
        EnvironmentalPoints = 0;
    }
    
    void Update()
    {
        switch (State)
        {
            case GameState.Start:
                Debug.Log("GAMESTATE: START");
                foreach (var i in Enumerable.Range(1, 6))
                    Deck.Give(Hand);
                State = GameState.Turn;
                break;
            case GameState.TurnStart:
                Debug.Log("GAMESTATE: TURNSTART");
                Board.RaiseTurnStartEvents();
                Deck.Give(Hand);
                State = GameState.Turn;
                break;
            case GameState.Turn:
                //Debug.Log("GAMESTATE: TURN");
                break;
            case GameState.TurnEnd:
                Debug.Log("GAMESTATE: TURNEND");
                Board.RaiseTurnEndEvents();
                State = GameState.TurnStart;
                break;
        }
    }

    public void EndTurn()
    {
        if (State == GameState.Turn)
            State = GameState.TurnEnd;
    }
}
