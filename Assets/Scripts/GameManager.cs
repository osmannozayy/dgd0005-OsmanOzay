using FishNet.Object;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public GameObject ballPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnBall();
    }

    private void SpawnBall()
    {
        if (ballPrefab == null)
        {
            Debug.LogError("Error: Ball prefab is not assigned.");
            return;
        }

        GameObject spawnedBall = Instantiate(ballPrefab, new Vector3(0, Court.BallY, 0), Quaternion.identity);
        ServerManager.Spawn(spawnedBall);
    }
}