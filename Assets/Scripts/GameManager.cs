using FishNet.Object;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject ballPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameObject ball = Instantiate(ballPrefab);
        ServerManager.Spawn(ball);
    }
}