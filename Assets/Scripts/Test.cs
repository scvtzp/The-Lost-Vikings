using BackEnd;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        public async void OnPressedTest()
        {
            BackendDataManager.GetInstance.LevelUp(); // [추가] 로컬에 저장된 데이터를 변경

            await BackendDataManager.GetInstance.GameDataUpdate(); //[추가] 서버에 저장된 데이터를 덮어씌기(변경된 부분만)
            Debug.Log("테스트");
        }
    }
}