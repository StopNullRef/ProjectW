using ProjectW.Define;
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
        /// ĳ������ �Ϲ� ���� Ÿ��
        /// </summary>
        public Actor.AttackType atkType;
        /// <summary>
        /// �̵� �ӵ�
        /// </summary>
        public float moveSpeed;
        /// <summary>
        /// ������ ���鿡 �������� ��
        /// </summary>
        public float jumpForce;
        /// <summary>
        /// �Ϲ� ���� ����
        /// </summary>
        public float atkRange;
        /// <summary>
        /// �Ϲ� ���� ���� (��Ÿ��)
        /// </summary>
        public float atkInterval;
        /// <summary>
        /// ���彺�� ���̺� �ε��� ����
        /// </summary>
        public int growthStatRef;
        /// <summary>
        /// ���ҽ� ���
        /// </summary>
        public string resourcePath;
    }
}
