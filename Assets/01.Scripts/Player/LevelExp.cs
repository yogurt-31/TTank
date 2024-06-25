using Unity.Netcode;

public class LevelExp : NetworkBehaviour
{
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerScore>(out PlayerScore score))
        {
            if (!IsServer) return;
            score.ScoreUp();
            Destroy(gameObject);
        }
    }
}
