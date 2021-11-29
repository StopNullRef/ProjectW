using System;

namespace ProjectW.SD
{
    [Serializable]
    public class SDCharacter : StaticData
    {
        /// <summary>
        /// ĳ������ �̸�
        /// </summary>
        public string name;

        /// <summary>
        /// ĳ������ ���� Ÿ��
        /// </summary>
        public Define.Actor.AttackType atkType;

        /// <summary>
        /// �̵� �ӷ�
        /// </summary>
        public float moveSpeed;

        /// <summary>
        /// ���� �� ���鿡 �������� ��
        /// </summary>
        public float jumpForce;

        /// <summary>
        /// �Ϲ� ���� ����
        /// </summary>
        public float atkRange;

        /// <summary>
        /// �Ϲ� ���� ���� (���� ������)
        /// </summary>
        public float atkInterval;

        /// <summary>
        /// ?
        /// </summary>
        public int growthStatRef;

        /// <summary>
        /// ���ҽ�(prefabs) ���
        /// </summary>
        public string resourcePath;
    }
}
