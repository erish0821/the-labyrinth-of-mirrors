using UnityEngine;

public enum AbilityEffect
{
    None,                     // �ɷ� ����
    AddAttackAndDefense,      // ����̵�: ���ݷ°� ���� +1
    AttackSuccessOnDraw,      // ������: ��ο�� ���� ���� �Ǵ�
    StealthAttackBonus,       // �α�: ���� ���¿��� ���ط� +2
    EvadeAttack,              // ��ũ: 50% Ȯ���� ȸ��
    GiveCardToAlly,           // �ٵ�: �Ʊ����� ī�� ����
    DoubleDamageOnFirstAttack, // �ٹٸ���: ù ���� 2�� ����
    DrawExtraCardOnTurnStart, // �Ƹ�ī��: �� ���� �� 1�� �߰� ��ο�
    MagicAttack,              // ����: ���� �� ���ط� +2
    MultiAttack,              // ������: 2ȸ ���� ����
    HealSelfOrAlly,           // ������: �ϸ��� ü�� ȸ��
    AddDefenseEveryTurn,      // ������: ���� ���� +1
    HealOnAttack              // �ȶ��: �� ���� �� ü�� ȸ��
}




