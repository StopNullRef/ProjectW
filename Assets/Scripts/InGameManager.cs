﻿using ProjectW.Define;
using ProjectW.Resource;
using ProjectW.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectW
{
    /// <summary>
    /// 인게임 내 객체들을 관리할 클래스
    /// 스테이지 전환 시 처리작업 등을 수행 (해당 스테이지에 필요한 리소스 로드 및 인스턴스 생성)
    /// </summary>
    public class InGameManager : Singleton<InGameManager>
    {
        /// <summary>
        /// 스테이지 전환 작업이 완료되어 인게임을 시작할 준비가 되었는지
        /// </summary>
        private bool isReady;

        /// <summary>
        /// 현재 스테이지 인스턴스를 들고 있을 필드
        /// </summary>
        private GameObject currentStage;

        /// <summary>
        /// 스테이지 전환 시 필요한 리소스를 불러오고 리소스를 통해 인스턴스를 생성,
        /// 생성한 인스턴스에 필요한 데이터를 바인딩 작업
        /// -> 해당 메서드를 호출하는 시점은 로딩 씬이 활성화되어있는 상태
        /// 씬 전환 시에 다음 씬에 필요한 스테이지 리소스를 불러오는 작업
        /// </summary>
        /// <returns></returns>
        public IEnumerator ChangeStage()
        {
            isReady = false;

            // 외부(서버)에서 새로 불러올 스테이지 정보를 받은 상태
            // 스테이지 정보(스테이지에 대한 기획데이터)를 이용하여 스테이지 객체를 생성
            var sdStage = GameManager.User.boStage.sdStage;

            var resourceManager = ResourceManager.Instance;

            // 현재 스테이지 객체가 이미 존재하는지
            if(currentStage != null)
            {
                // 존재한다면 새로운 스테이지를 로드할 것이므로 파괴
                Destroy(currentStage);
            }
            // 새로 불러올 스테이지 객체를 생성하여 현재 스테이지를 나타내는 필드에 대입
            currentStage = Instantiate(resourceManager.LoadObject(sdStage.resourcePath));

            // 여기서 한가지 문제점이 발생
            // 현재 ChangeStage 메서드가 호출되고 있는 시점은 씬이 2개인 상태 (로딩, 인게임)
            // 이 때 객체를 생성하면 활성화되어있는 씬에 객체가 생성됨 (로딩 씬이 활성화, 인게임은 비활성화)
            // 그럼 현재 활성화 되어있는 로딩씬에 생성된 객체가 귀속됨
            // -> 로딩이 끝나면 로딩씬은 언로드 되고 인게임이 활성화되므로, 결과적으로 생성한 맵을 확인할 수가 없음

            // 그럼 어떻게 해결?
            // 생성한 객체를 로딩씬에서 인게임씬으로 이동
            SceneManager.MoveGameObjectToScene(currentStage,SceneManager.GetSceneByName(SceneType.Ingame.ToString()));

            // 위의 과정을 통해 스테이지 객체만 로딩 과정에서 미리 만들어두고
            // 그외 캐릭터나 몬스터 (액터)들은 씬이 완전히 전환된 후에 따로 생성


            yield return null;
        }

    }
}
