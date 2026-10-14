using FishNet.Object;
using UnityEngine;

public class NetworkBall : NetworkBehaviour
{
    [SerializeField] private float speed = 7f;
    private Vector3 velocityDir;
    private int leftScore, rightScore;
    private float serveTime;

    public override void OnStartServer()
    {
        base.OnStartServer();
        ResetBall(1);
    }

    private void Update()
    {
        if (!base.IsServerInitialized) return;

        if (velocityDir == Vector3.zero)
        {
            if (Time.time >= serveTime) ServeBall();
            return;
        }

        Vector3 pos = transform.position + velocityDir * Time.deltaTime;

        float zLimit = Court.HalfHeight - Court.BallRadius;
        if (Mathf.Abs(pos.z) > zLimit)
        {
            pos.z = Mathf.Sign(pos.z) * zLimit;
            velocityDir.z = -velocityDir.z;
        }

        foreach (PaddleController paddle in Object.FindObjectsByType<PaddleController>(FindObjectsSortMode.None))
        {
            Vector3 paddlePos = paddle.transform.position;
            bool isTowards = (paddlePos.x < 0 && velocityDir.x < 0) || (paddlePos.x > 0 && velocityDir.x > 0);
            if (!isTowards) continue;

            if (Mathf.Abs(pos.x - paddlePos.x) < 0.5f && Mathf.Abs(pos.z - paddlePos.z) < Court.PaddleHalfLength + Court.BallRadius)
            {
                velocityDir.x = -velocityDir.x;
                velocityDir.z += (pos.z - paddlePos.z) * 3f;
                velocityDir = velocityDir.normalized * speed;
                pos.x = paddlePos.x + (paddlePos.x < 0 ? 0.5f : -0.5f);
            }
        }

        transform.position = pos;

        if (pos.x < -Court.HalfWidth - 0.5f) { rightScore++; ResetBall(0); }
        else if (pos.x > Court.HalfWidth + 0.5f) { leftScore++; ResetBall(1); }
    }

    private void ResetBall(int towards)
    {
        transform.position = new Vector3(0, Court.BallY, 0);
        velocityDir = Vector3.zero;
        serveTime = Time.time + 1f;
    }

    private void ServeBall()
    {
        float dx = Random.value > 0.5f ? -1f : 1f;
        float dz = Random.Range(-0.6f, 0.6f);
        velocityDir = new Vector3(dx, 0, dz).normalized * speed;
    }
}