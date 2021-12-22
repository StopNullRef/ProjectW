/// <summary>
/// ������Ʈ���� ���Ǵ� ����� ������ ���� ����
/// </summary>
namespace ProjectW.Define
{
    /// <summary>
    /// ���ӿ��� ���Ǵ� �� ����
    /// </summary>
    public enum SceneType { Title, Ingame, Loading, }

    /// <summary>
    /// Ÿ��Ʋ ������ ���������� ������ �۾��� ����
    /// </summary>
    public enum IntroPhase
    {
        Start, //����
        ApplicationSetting, // �� ����
        Server, // ���� ���� �� �ʱ�ȭ
        StaticData, // ��ȹ ������ �ε� (������ ������)
        UserData, // ���� ������ �ε� (�������� ������)
        Resource, // ���ҽ� �ε�
        UI, // UI �Ŵ��� �ʱ�ȭ
        Comepelte, // �Ϸ�
    }

    public class Camera
    {
        public enum View { Standard,Front}
        public const float RotSpeed = 3f;
        public const string CamPosPath = "Prefabs/CamPos";
    }

    public class Input
    {
        public const string AxisX = "Horizontal"; // ĳ���� �¿� �̵��� ��� (a,d �� ��밡���ϰ�)
        public const string AxisZ = "Vertical"; // ĳ���� �յ� �̵��� ��� (w,s �� ���)
        public const string MouseX = "Mouse X"; // ���콺 ����(�¿�) �̵��� ���� �ప
        public const string MouseY = "Mouse Y"; // ���콺 ���� �̵��� ���� �ప
        public const string FrontCam = "Fire3"; // ī�޶� ĳ���� �Ĺ�(3��Ī)���� ���� ī�޶�� ����(leftctrl�� ���)
        public const string Jump = "Jump"; // ĳ���� ���� (spaceBar�� ���)
        public const string MouseLeft = "Fire1"; // ���콺 ���� Ŭ��
        public const string MouseRight = "Fire2"; // ���콺 ������ Ŭ��
    }

    public class Actor
    {
        /// <summary>
        /// ������ Ÿ��
        /// </summary>
        public enum Type { Character, Monster, }

        /// <summary>
        /// ������ ����
        /// </summary>
        public enum State { Idle, Walk, Jump, Attack, Dead, }

        /// <summary>
        /// ������ �Ϲݰ��� Ÿ��
        /// </summary>
        public enum AttackType { Normal, Projectile, }
    }

    public class Monster
    {
        /// <summary>
        /// �ּ� ���� ���� ��
        /// </summary>
        public const int MinSpawnCnt = 1;
        /// <summary>
        /// �ִ� ���� ���� ��
        /// </summary>
        public const int MaxSpawnCnt = 5;
        /// <summary>
        /// ���� ���������� �ּ� �ð�
        /// </summary>
        public const float MinSpawnTime = 10f;
        /// <summary>
        /// ���� ���������� �ִ� �ð�
        /// </summary>
        public const float MaxSpawnTime = 20f;
        /// <summary>
        /// ���� �� �ּ� ��� �ð�
        /// </summary>
        public const float MinPatrolWaitTime = 1f;
        /// <summary>
        /// ���� �� �ִ� ��� �ð�
        /// </summary>
        public const float MaxPatrolwaitTime = 3f;
    }

    public class StaticData
    {
        public const string SDPath = "Assets/StaticData";
        public const string SDExcelPath = "Assets/StaticData/Excel";
        public const string SDJsonPath = "Assets/StaticData/Json";
    }

}
