using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : NetworkBehaviour
{
    [SerializeField] private float speed = 8f;
    private int side;

    public override void OnStartClient()
    {
        base.OnStartClient();
        side = base.OwnerId == 0 ? 0 : 1;
        transform.position = new Vector3(side == 0 ? -Court.PaddleX : Court.PaddleX, 0.5f, 0);
        Court.Paint(gameObject, side == 0 ? new Color(0.35f, 0.65f, 1f) : new Color(1f, 0.60f, 0.25f));
    }

    private void Update()
    {
        if (!base.IsOwner) return;
        if (!Application.isFocused) return;

        Keyboard kb = Keyboard.current;
        if (kb == null) return;

        float dir = 0f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) dir += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) dir -= 1f;

        transform.position += Vector3.forward * dir * speed * Time.deltaTime;
        Court.ClampPaddle(transform);
    }
}