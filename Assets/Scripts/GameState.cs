using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    public static GameState Instance;

    public readonly SyncVar<int> LeftScore = new SyncVar<int>();
    public readonly SyncVar<int> RightScore = new SyncVar<int>();
    public readonly SyncVar<int> ServingSide = new SyncVar<int>(0);

    public readonly SyncList<string> Feed = new SyncList<string>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectionHud.Panels.Add(() =>
        {
            string text = $"LEFT {LeftScore.Value} : {RightScore.Value} RIGHT\nServe: {ServingSide.Value}\n---\n";
            foreach (string line in Feed)
            {
                text += line + "\n";
            }
            return text;
        });
    }

    public void Goal(int scorerSide)
    {
        if (!base.IsServerInitialized) return;

        if (scorerSide == 0) LeftScore.Value++;
        else RightScore.Value++;

        ServingSide.Value = 1 - scorerSide;

        Announce($"Goal! Side {scorerSide} scored.");

        PaddleController scorer = null;
        PaddleController conceder = null;

        foreach (PaddleController p in PaddleController.All)
        {
            if (p.Side.Value == scorerSide) scorer = p;
            else if (p.Side.Value == 1 - scorerSide) conceder = p;
        }

        if (scorer != null) scorer.AnnounceGoal(LeftScore.Value, RightScore.Value);
        if (conceder != null) conceder.YouServe(conceder.Owner);
    }

    public static void Announce(string line)
    {
        if (Instance == null || !Instance.IsServerInitialized) return;

        Instance.Feed.Add(line);
        if (Instance.Feed.Count > 6)
        {
            Instance.Feed.RemoveAt(0);
        }
    }
}