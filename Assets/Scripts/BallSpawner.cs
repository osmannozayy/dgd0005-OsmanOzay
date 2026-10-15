using FishNet.Object;
using UnityEngine;

public class BallSpawner : NetworkBehaviour
{
    public GameObject ballPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();

        GameObject spawnedBall = Instantiate(ballPrefab, new Vector3(0, Court.BallY, 0), Quaternion.identity);
        ServerManager.Spawn(spawnedBall);
    }
}