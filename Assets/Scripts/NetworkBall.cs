using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class NetworkBall : NetworkBehaviour
{
    public static NetworkBall Instance;
    public readonly SyncVar<bool> InPlay = new SyncVar<bool>();
    private Vector3 velocity;

    private void Awake()
    {
        Instance = this;
        InPlay.OnChange += OnPlayChanged;
    }

    private void OnPlayChanged(bool prev, bool next, bool asServer)
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr != null)
            mr.material.color = next ? Color.white : Color.gray;
    }

    public void Serve(int towardSide)
    {
        if (!base.IsServerInitialized) return;

        InPlay.Value = true;
        float vx = towardSide == 0 ? -5f : 5f;
        velocity = new Vector3(vx, 0, Random.Range(-2f, 2f));

        GameState.Announce($"Serve towards Side {towardSide}");
    }

    private void Update()
    {
        if (!base.IsServerInitialized) return;
        if (!InPlay.Value) return;

        Vector3 pos = transform.position;
        pos += velocity * Time.deltaTime;

        if (pos.z > Court.HalfHeight - Court.BallRadius)
        {
            pos.z = Court.HalfHeight - Court.BallRadius;
            velocity.z = -velocity.z;
        }
        else if (pos.z < -Court.HalfHeight + Court.BallRadius)
        {
            pos.z = -Court.HalfHeight + Court.BallRadius;
            velocity.z = -velocity.z;
        }

        foreach (PaddleController p in PaddleController.All)
        {
            if (p.Side.Value == 2) continue;

            if (Mathf.Abs(pos.x - p.transform.position.x) < Court.BallRadius + 0.5f &&
                Mathf.Abs(pos.z - p.transform.position.z) < Court.BallRadius + Court.PaddleHalfLength)
            {
                velocity.x = -velocity.x;
                pos.x = p.transform.position.x + (pos.x > p.transform.position.x ? 1 : -1) * (Court.BallRadius + 0.5f);
                break;
            }
        }

        if (pos.x > Court.HalfWidth)
        {
            GameState.Instance.Goal(0);
            ResetBall();
            return;
        }
        else if (pos.x < -Court.HalfWidth)
        {
            GameState.Instance.Goal(1);
            ResetBall();
            return;
        }

        transform.position = pos;
    }

    private void ResetBall()
    {
        InPlay.Value = false;
        transform.position = new Vector3(0, Court.BallY, 0);
        velocity = Vector3.zero;
    }
}