using ProjectW.DB;
using System.Collections.Generic;
using System.Linq;


namespace ProjectW.Network
{
    /// <summary>
    /// 로그인 시 필요한 데이터들을 서버에 요청하는 작업을 하는 클래스
    /// </summary>
    public class LoginHandler
    {
        /// <summary>
        /// 리스폰스 핸들러를 통해 데이터를 받아 처리
        /// </summary>
        public ResponseHandler<DtoAccount> accountHandler;
        public ResponseHandler<DtoStage> stageHandler;
        public ResponseHandler<DtoCharacter> characterHandler;


        public void Connect()
        {
            ServerManager.Sever.Login(0,accountHandler);
        }

        public LoginHandler()
        {
            accountHandler = new ResponseHandler<DtoAccount>(GetAccountSuccess, OnFailed);
            stageHandler = new ResponseHandler<DtoStage>(GetStageSuccess, OnFailed);
            characterHandler = new ResponseHandler<DtoCharacter>(GetCharacterSuccess, OnFailed);
        }

        /// <summary>
        /// 계정 정보 요청 성공 시 실행할 메서드
        /// </summary>
        /// <param name="dtoAccount">서버에서 보내준 계정 정보</param>
        public void GetAccountSuccess(DtoAccount dtoAccount)
        {
            // 서버에서 받은 dto 데이터를 bo 데이터로 변환 후
            // 게임매니저의 모든 bo 데이터 관리 객체가 들고 있게 한다.
            GameManager.User.boAccount = new BoAccount(dtoAccount);

            // 다음으로 스테이지 정보를 서버에 요청
            ServerManager.Sever.GetStage(0, stageHandler);


        }

        /// <summary>
        /// 스테이지 정보 요청 성공 시 실행할 메서드
        /// </summary>
        /// <param name="dtoStage"></param>
        public void GetStageSuccess(DtoStage dtoStage)
        {
            ServerManager.Sever.GetCharacter(1, characterHandler);

            GameManager.User.boStage = new BoStage(dtoStage);
        }

        /// <summary>
        /// 캐릭터 정보 요청 성공 시 실행할 메서드
        /// </summary>
        /// <param name="dtoCharacter"></param>
        public void GetCharacterSuccess(DtoCharacter dtoCharacter)
        {
            GameManager.User.boCharacter = new BoCharacter(dtoCharacter);
        }

        /// <summary>
        /// 서버에 특정 요청 실패 시 실행될 메서드
        /// </summary>
        /// <param name="dtoError"></param>
        public void OnFailed(DtoBase dtoError)
        {

        }

    }
}
