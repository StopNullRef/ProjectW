
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectW.Util
{
    // is a has a 
    // ~이다 ~를 가지고있다

    /// <summary>
    /// 오브젝트 풀링을 사용하는 클래스에서 구현해야 하는 인터페이스
    /// 오브젝트 풀링을 사용할 객체는 해당 인터페이스를 상속받아야만
    /// 오브젝트 풀링을 사용할 수 있음
    /// </summary>
    public interface IPoolableObject
    {
        // 총알은 풀링 기능을 가지고 있다.

        /// <summary>
        /// 오브젝트가 재사용될 수 있음을 나타내는 프로퍼티
        /// 오브젝트 풀에서 꺼내서 사용할 수 있는 상태인지를 나타내는 값
        /// </summary>
        bool CanRecyle { get; set; }
    }
}
