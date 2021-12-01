using System;

namespace ProjectW.DB
{
    // 일반적으로 서버에서 받은(통신에 사용되는 데이터) Dto 를 Bo로 변환하여 사용

    // Dto를 직접적으로 인게임 로직에서 사용할 일은 없음
    // -> 통신데이터의 보안을 위한 목적 및 통신에 사용되는 데이터는 통신량을 줄이기 위해 최소한의
    // 데이터만 포함됨

    // Bo는 인게임 로직에서만 사용되고, 통신을 하지 않으므로 직렬화할 필요가 없음
    // -> Dto는 최소한의 데이터만을 가지므로, 클라이언트에서 표현하고자 하는 데이터를 바로
    // 표현할 수 있는 경우가 많이 없다 따라서 Dto를 클라이언트에서 사용하고자 하는 형태로
    // 가공을 거침.. 이때 가공된 데이터를 Bo (Business object)라고 표현
    // -> 반대로 서버에 데이터를 보낼 때도, Bo를 바로 보내는 것이 아닌 Bo -> Dto로 변환하여 보냄


    public class BoAccount
    {
        public string nickname;
        public int gold;

        public BoAccount(DtoAccount dtoAccount)
        {
            nickname = dtoAccount.nickname;
            gold = dtoAccount.gold;
        }
    }
}