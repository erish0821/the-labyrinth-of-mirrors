using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;       // ī�� �̸�
    public string type;       // ī�� ���� (����, ���, ���� ��)
    public string effect;     // ī�� ȿ��
    [Range(1, 100)] public int quantity;   // ī�� ���� (Inspector���� ���� ����)
    [Range(0, 10)] public int damage;      // ī�� ������ (���� ī�忡 ���)
    [Range(0, 10)] public int heal;        // ȸ���� (ȸ�� ī�忡 ���)
    public TargetType targetType; // TargetType ���
    public Sprite sprite;     // ī�� �̹��� (Unity���� ���� ����)
}

[CreateAssetMenu(fileName = "ItemSO", menuName = "Game/ItemSO")]
public class ItemSO : ScriptableObject
{
    public Item[] items;
}
