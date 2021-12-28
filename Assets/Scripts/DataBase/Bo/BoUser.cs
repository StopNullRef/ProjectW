﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectW.DB
{
    /// <summary>
    /// 유저의 모든 Bo 데이터를 포함하는 데이터 셋
    /// </summary>
    [Serializable]
    public class BoUser
    {
        public BoAccount boAccount;
        public BoStage boStage;
        public BoCharacter boCharacter;
        public List<BoItem> boItems;
    }
}
