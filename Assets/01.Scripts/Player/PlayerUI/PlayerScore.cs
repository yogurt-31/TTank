using Unity.Netcode;
using TMPro;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;

public enum SkillType
{
    MOVE_SPEED,
    ATK_SIZE,
    CANNON_UP,
}
public class PlayerScore : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI atkSizeText;
    [SerializeField] private TextMeshProUGUI cannonText;

    private int playerScore;
    private int moveSpeed;
    private int cannonCnt;
    private int atkSize;

    private PlayerCannon projectileLauncher;

    private void Awake()
    {
        projectileLauncher = GetComponent<PlayerCannon>();
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (playerScore <= 0) return; 
        if (moveSpeed >= 50 || atkSize >= 50) return;

        if(Input.GetKeyDown(KeyCode.Alpha1) && moveSpeed < 50)
        {
            LevelUpSkillServerRpc(SkillType.MOVE_SPEED);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2) && atkSize < 10)
        {
            LevelUpSkillServerRpc(SkillType.ATK_SIZE);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3) && cannonCnt < 7)
        {
            LevelUpSkillServerRpc(SkillType.CANNON_UP);
        }
    }

    public float CalculateMoveSpeed(float moveSpeed) => moveSpeed + this.moveSpeed * 0.25f;

    public Vector3 CalculateAtkSize()
    {
        float bulletSize = 0.5f + atkSize * 0.05f;
        return new Vector3(bulletSize, bulletSize);
    }

    [ClientRpc]
    public void ChangeCannonClientRpc()
    {
        projectileLauncher.CreateCannon();
    }

    public void ScoreUp()
    {
        playerScore++;
        Debug.Log(playerScore);
        ScoreUpClientRpc(playerScore);
    }

    [ClientRpc]
    private void ScoreUpClientRpc(int newScore, ClientRpcParams rpcParams = default)
    {
        playerScore = newScore;
        UpdateScorePoint();
    }

    [ServerRpc]
    public void LevelUpSkillServerRpc(SkillType skillType)
    {
        playerScore--;
        ScoreUpClientRpc(playerScore);
        switch (skillType)
        {
            case SkillType.ATK_SIZE:
                atkSize++;
                break;
            case SkillType.MOVE_SPEED:
                moveSpeed++;
                break;
            case SkillType.CANNON_UP:
                cannonCnt++;
                ChangeCannonClientRpc();
                break;
        }
        UpdateClientSkillsClientRpc(atkSize, moveSpeed, cannonCnt);
    }

    [ClientRpc]
    private void UpdateClientSkillsClientRpc(int newAtkSize, int newMoveSpeed, int newCannonCnt)
    {
        atkSize = newAtkSize;
        moveSpeed = newMoveSpeed;
        cannonCnt = newCannonCnt;
        UpdateScorePoint();
    }
    private void UpdateScorePoint()
    {
        playerScoreText.text = playerScore + " Point";
        moveSpeedText.text = "+" + moveSpeed;
        atkSizeText.text = "+" + atkSize;
        cannonText.text = "+" + cannonCnt;
    }
}