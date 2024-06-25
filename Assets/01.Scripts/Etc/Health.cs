using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class Health : NetworkBehaviour
{
    [SerializeField] private LevelExp expPrefab;

    public NetworkVariable<int> currentHealth;
    public int maxHealth;

    public event Action OnDieEvent;
    public event Action OnHealthChangedEvent;

    public bool isDead;

    private void Awake()
    {
        currentHealth = new NetworkVariable<int>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(-1);
    }

    public override void OnNetworkSpawn()
    {
        //���ǻ���2 . NetworkVariable�� ������ �ǵ帱 �� �ִ�. Ŭ��� ���� ������ �ޱ⸸
        if (IsClient)
        {
            currentHealth.OnValueChanged += HandleHealthValueChanged;
        }

        if (IsServer == false) return;
        currentHealth.Value = maxHealth; //ó�� ���۽� �ִ�ü������ �־��ش�.
    }

    public override void OnNetworkDespawn()
    {
        for (int i = 0; i < 4; ++i)
        {
            Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle;
        }

        if (IsClient)
        {
            currentHealth.OnValueChanged -= HandleHealthValueChanged;
        }
    }

    public float GetNormalizedHealth()
    {
        return (float)currentHealth.Value / maxHealth;
    }

    private void HandleHealthValueChanged(int previousValue, int newValue)
    {
        OnHealthChangedEvent?.Invoke();

        int delta = newValue - previousValue;
        int value = Mathf.Abs(delta);

        if (value == maxHealth) return;// ó�� ����� �� ü�� ä���� ��
    }

    //�̳༮�� ������ �����ϴ� �ż����
    private void ModifyHealth(int value)
    {
        if (isDead) return;

        currentHealth.Value = Mathf.Clamp(currentHealth.Value + value, 0, maxHealth);

        if (currentHealth.Value == 0)
        {
            OnDieEvent?.Invoke();
            LevelExp newCoin = Instantiate(expPrefab, transform.position, Quaternion.identity);

            newCoin.NetworkObject.Spawn();
            isDead = true;
        }
    }

    public void TakeDamage(int damageValue)
    {
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healValue)
    {
        ModifyHealth(healValue);
    }
}
