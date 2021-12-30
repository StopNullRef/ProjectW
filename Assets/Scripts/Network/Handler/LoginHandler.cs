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
        public ResponseHandler<DtoItem> itemHandler;


        public void Connect()
        {
            ServerManager.Sever.Login(0,accountHandler);
        }

        public LoginHandler()
        {
            accountHandler = new ResponseHandler<DtoAccount>(GetAccountSuccess, OnFailed);
            stageHandler = new ResponseHandler<DtoStage>(GetStageSuccess, OnFailed);
            characterHandler = new ResponseHandler<DtoCharacter>(GetCharacterSuccess, OnFailed);
            itemHandler = new ResponseHandler<DtoItem>(GetItemSuccess, OnFailed);
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
            GameManager.User.boStage = new BoStage(dtoStage);

            ServerManager.Sever.GetItem(0, itemHandler);
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

        /// <summary>
        /// 아이템 요청 성공시 실행할 메서드
        /// </summary>
        /// <param name="dtoItem"></param>
        public void GetItemSuccess(DtoItem dtoItem)
        {
            // 유저 아이템 정보 리스트 할당
            GameManager.User.boItems = new List<BoItem>();
            // 유저 아이템 정보 리스트에 빈번히 접근해야하므로 잠시 참조를 받아둠
            var boItems = GameManager.User.boItems;

            for(int i =0; i< dtoItem.items.Count; i++)
            {
                // 서버에서 받은 아이템 개별 정보를 베이스 데이터 형태로 접근
                var dtoItemElement = dtoItem.items[i];

                // 서버 아이템 정보로 유저 아이템 정보를 만들기 위해 boItem 변수 선언
                BoItem boItem = null;

                // 서버 아이템의 기획 인덱스 값으로 아이템 기획 데이터를 찾음
                var sdItem = GameManager.SD.sdItems.Where(_ => _.index == dtoItemElement.index).SingleOrDefault();

                if(sdItem.itemType == Define.Item.ItemType.Equipment)
                {
                    // Bo 장비 데이터 형태로 생성
                    boItem = new BoEquipment(sdItem);
                    // 위의 담고 있는 boItem 변수가 BoItem 형태이므로 장비 데이터 필드에 접근할수 없다.
                    // 따라서, 장비 데이터를 채워주기 위해 BoEquipment로 캐스팅
                    var boEquipment = boItem as BoEquipment;
                    boEquipment.reinforceValue = dtoItemElement.reinforceValue;
                    boEquipment.isEquip = dtoItemElement.isEquip;
                }
                // 그 외 타입이라면
                else
                {
                    boItem = new BoItem(sdItem);
                }

                boItem.slotIndex = dtoItemElement.slotIndex;
                boItem.amount = dtoItemElement.amount;
                 
                // 생성한 아이템 정보를 유저 아이템 정보 리스트에 넣는다.
                boItems.Add(boItem);
            }

            // 다음으로 캐릭터 정보를 요청한다.
            ServerManager.Sever.GetCharacter(0, characterHandler);
        }

    }
}
