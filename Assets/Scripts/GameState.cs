using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    public static GameState Instance;

    public readonly SyncVar<int> LeftScore = new SyncVar<int>();
    public readonly SyncVar<int> RightScore = new SyncVar<int>();
    public readonly SyncVar<int> ServingSide = new SyncVar<int>(0);

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }

    public void Goal(int scorerSide)
    {
        if (!base.IsServerInitialized) return;

        if (scorerSide == 0) LeftScore.Value++;
        else RightScore.Value++;

        ServingSide.Value = 1 - scorerSide;
    }
}