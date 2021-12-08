using ProjectW.DB;
using UnityEngine;

namespace ProjectW.Dummy
{
    // ScriptableOjbect란?
    // 유니티에서 지원하는 데이터 또는 정적 메서드(툴 같은 기능)만을 갖는 클래스
    // 인스턴스가 불가능함
    // -> 현재 DB가 없으므로 해당 클래스가 DB라고 생각하고 사용하면 됨

    // !!! 원래 ScriptableObject의 용도는 고정된 데이터(기획 데이터)등을 저장하기 위한 용도
    // -> 유저데이터 처럼  수정이 발생하는 데이터를 저장하기에는 적합하지 않음
    [CreateAssetMenu(menuName = "ProjectW/UserData")]
    public class UserDataSO : ScriptableObject
    {
        public DtoAccount dtoAccount;
        public DtoStage dtoStage;
        public DtoCharacter dtoCharacter;
    }
}
