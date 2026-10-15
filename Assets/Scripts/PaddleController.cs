using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PaddleController : NetworkBehaviour
{
    [SerializeField] private float speed = 8f;

    public readonly SyncVar<int> Side = new SyncVar<int>(2);
    public static List<PaddleController> All = new List<PaddleController>();
    public static PaddleController Local;

    private void Awake()
    {
        Side.OnChange += OnSideChanged;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (!All.Contains(this)) All.Add(this);

        int assignedSide = 2;
        bool leftTaken = false;
        bool rightTaken = false;

        foreach (PaddleController p in All)
        {
            if (p != this)
            {
                if (p.Side.Value == 0) leftTaken = true;
                if (p.Side.Value == 1) rightTaken = true;
            }
        }

        if (!leftTaken) assignedSide = 0;
        else if (!rightTaken) assignedSide = 1;

        Side.Value = assignedSide;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        All.Remove(this);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!All.Contains(this)) All.Add(this);

        if (base.IsOwner) Local = this;

        Place();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        All.Remove(this);
    }

    private void OnSideChanged(int prev, int next, bool asServer)
    {
        Place();
    }

    private void Place()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null) return;

        if (Side.Value == 2)
        {
            mr.enabled = false;
            return;
        }

        mr.enabled = true;
        float xPos = Side.Value == 0 ? -Court.PaddleX : Court.PaddleX;
        transform.position = new Vector3(xPos, 0.5f, 0);

        Color c = Side.Value == 0 ? new Color(0.35f, 0.65f, 1f) : new Color(1f, 0.60f, 0.25f);
        Court.Paint(gameObject, c);
    }

    private void Update()
    {
        if (!base.IsOwner) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float dir = 0f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) dir += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) dir -= 1f;

        if (kb.spaceKey.wasPressedThisFrame)
        {
            RequestServe();
        }

        transform.position += Vector3.forward * dir * speed * Time.deltaTime;
        Court.ClampPaddle(transform);
    }

    [ServerRpc]
    private void RequestServe()
    {
        GameState gs = FindObjectOfType<GameState>();
        NetworkBall nb = FindObjectOfType<NetworkBall>();

        if (gs == null || nb == null) return;

        if (Side.Value == gs.ServingSide.Value && !nb.InPlay.Value)
        {
            nb.Serve(1 - Side.Value);
        }
    }

    [ObserversRpc]
    public void AnnounceGoal(int left, int right)
    {
        Debug.Log($"Goal! {left} : {right}");
    }

    [TargetRpc]
    public void YouServe(NetworkConnection target)
    {
        Debug.Log("YOU SERVE");
    }
}