/// <summary>
/// ������Ʈ���� ���Ǵ� ����� ������ ���� ����
/// </summary>
namespace ProjectW.Define
{
    /// <summary>
    /// ���ӿ��� ���Ǵ� �� ����
    /// </summary>
    public enum SceneType { Title,Ingame,Loading,}

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
}
