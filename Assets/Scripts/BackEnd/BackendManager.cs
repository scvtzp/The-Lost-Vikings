using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BackEnd
{
    public class BackendManager : MonoBehaviour
    {
        void Start()
        {
            var bro = Backend.Initialize(true); // 뒤끝 초기화

            // 뒤끝 초기화에 대한 응답값
            if (bro.IsSuccess())
            {
                Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
            }
            else
            {
                Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
            }

            Test();
        }

        // 동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가)
        async UniTaskVoid Test()
        {
            Debug.Log("1");
            
            await UniTask.RunOnThreadPool(() =>
            {
                BackendLogin.Instance.CustomLogin("user1", "1234"); // 뒤끝 로그인

                BackendDataManager.GetInstance.GameDataGet(); // 데이터 삽입 함수

                // [추가] 서버에 불러온 데이터가 존재하지 않을 경우, 데이터를 새로 생성하여 삽입
                if (BackendDataManager.userData == null)
                    BackendDataManager.GetInstance.GameDataInsert();

                Debug.Log("테스트를 종료합니다.");
            });
            
            Debug.Log("업데이트 함수 호출");

            var a = await BackendDataManager.GetInstance.GameDataUpdate();
            if (a != null && a.IsSuccess())
                Debug.Log("게임 정보 데이터 수정에 성공했습니다. : " + a);
            else
                Debug.LogError("게임 정보 데이터 수정에 실패했습니다. : " + a);
            
            Debug.Log("업데이트 함수 끝");
        }
    }
}