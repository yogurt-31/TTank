using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;

    public event Action OnFireEvent;

    private Rigidbody2D rigidCompo;
    private PlayerScore playerScore;

    private void Awake()
    {
        rigidCompo = GetComponent<Rigidbody2D>();
        playerScore = GetComponent<PlayerScore>();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 vec = new Vector2(x, y).normalized;
        rigidCompo.velocity = vec * playerScore.CalculateMoveSpeed(moveSpeed);

        if(Input.GetMouseButtonDown(0))
        {
            OnFireEvent?.Invoke();
        }
    }
}
