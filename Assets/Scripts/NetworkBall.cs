using FishNet.Object;
using UnityEngine;

public class NetworkBall : NetworkBehaviour
{
    [SerializeField] private float speed = 7f;
    private Vector3 _velocity;
    private int _left, _right;
    private float _serveAt;

    public override void OnStartServer()
    {
        base.OnStartServer();
        Reset(1);
    }

    private void Update()
    {
        if (!base.IsServer) return;

        if (_velocity == Vector3.zero)
        {
            if (Time.time >= _serveAt) Serve();
            return;
        }

        Vector3 p = transform.position + _velocity * Time.deltaTime;

        // Duvar Çarpışmaları[cite: 3]
        float zLimit = Court.HalfHeight - Court.BallRadius;
        if (Mathf.Abs(p.z) > zLimit) { p.z = Mathf.Sign(p.z) * zLimit; _velocity.z = -_velocity.z; }

        foreach (PaddleController paddle in FindObjectsByType<PaddleController>(FindObjectsSortMode.None))
        {
            Vector3 pp = paddle.transform.position;
            bool towards = (pp.x < 0 && _velocity.x < 0) || (pp.x > 0 && _velocity.x > 0);
            if (!towards) continue;

            if (Mathf.Abs(p.x - pp.x) < 0.5f && Mathf.Abs(p.z - pp.z) < Court.PaddleHalfLength + Court.BallRadius)
            {
                _velocity.x = -_velocity.x;
                _velocity.z += (p.z - pp.z) * 3f;
                _velocity = _velocity.normalized * speed;
                p.x = pp.x + (pp.x < 0 ? 0.5f : -0.5f);
            }
        }
        transform.position = p;

        if (p.x < -Court.HalfWidth - 0.5f) { _right++; Debug.Log($"SAĞ Gol! Skor: {_left} - {_right}"); Reset(0); }
        else if (p.x > Court.HalfWidth + 0.5f) { _left++; Debug.Log($"SOL Gol! Skor: {_left} - {_right}"); Reset(1); }
    }

    private void Reset(int towards)
    {
        transform.position = new Vector3(0, Court.BallY, 0);
        _velocity = Vector3.zero;
        _serveAt = Time.time + 1f;
    }

    private void Serve()
    {
        float dx = Random.value > 0.5f ? -1f : 1f;
        float dz = Random.Range(-0.6f, 0.6f);
        _velocity = new Vector3(dx, 0, dz).normalized * speed;
    }
}