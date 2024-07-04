using BackEnd.Util;
using Cysharp.Threading.Tasks;
using Model;
using UnityEngine;
// 뒤끝 SDK namespace 추가

namespace BackEnd
{
    public class BackendDataManager : Singleton<BackendDataManager>
    {
        public static TestModel userData;

        private string gameDataRowInDate = string.Empty;

        public void GameDataInsert()
        {
            if (userData == null)
            {
                userData = new TestModel();
            }

            Debug.Log("데이터를 초기화합니다.");
            userData.level = 1;
            userData.atk = 3.5f;
            userData.info = "친추는 언제나 환영입니다.";

            userData.equipment.Add("전사의 투구");
            userData.equipment.Add("강철 갑옷");
            userData.equipment.Add("헤르메스의 군화");

            userData.inventory.Add("빨간포션", 1);
            userData.inventory.Add("하얀포션", 1);
            userData.inventory.Add("파란포션", 1);

            Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
            Param param = new Param();
            param.Add("level", userData.level);
            param.Add("atk", userData.atk);
            param.Add("info", userData.info);
            param.Add("equipment", userData.equipment);
            param.Add("inventory", userData.inventory);


            Debug.Log("게임 정보 데이터 삽입을 요청합니다.");
            var bro = Backend.GameData.Insert(TableNames.Test, param);

            if (bro.IsSuccess())
            {
                Debug.Log("게임 정보 데이터 삽입에 성공했습니다. : " + bro);

                //삽입한 게임 정보의 고유값입니다.  
                gameDataRowInDate = bro.GetInDate();
            }
            else
            {
                Debug.LogError("게임 정보 데이터 삽입에 실패했습니다. : " + bro);
            }
        }

        public void GameDataGet()
        {
            Debug.Log("게임 정보 조회 함수를 호출합니다.");
            var bro = Backend.GameData.GetMyData(TableNames.Test, new Where());
            if (bro.IsSuccess())
            {
                Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


                LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

                // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
                if (gameDataJson.Count <= 0)
                {
                    Debug.LogWarning("데이터가 존재하지 않습니다.");
                }
                else
                {
                    gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임 정보의 고유값입니다.  

                    userData = new TestModel();

                    userData.level = int.Parse(gameDataJson[0]["level"].ToString());
                    userData.atk = float.Parse(gameDataJson[0]["atk"].ToString());
                    userData.info = gameDataJson[0]["info"].ToString();

                    foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                    {
                        userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                    }

                    foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                    {
                        userData.equipment.Add(equip.ToString());
                    }

                    Debug.Log(userData.ToString());
                }
            }
            else
            {
                Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
            }
        }

        public void LevelUp()
        {
            Debug.Log("레벨을 1 증가시킵니다.");
            userData.level += 1;
            userData.atk += 3.5f;
            userData.info = "내용을 변경합니다.";
        }

        // 게임 정보 수정하기
        public async UniTask<BackendReturnObject> GameDataUpdate()
        {
            BackendReturnObject bro = null;
            if (userData == null)
            {
                Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
                return bro;
            }

            var param = userData.GetParam();

            Debug.Log("업데이트 시작");

            if (string.IsNullOrEmpty(gameDataRowInDate))
            {
                Debug.Log("내 제일 최신 게임 정보 데이터 수정을 요청합니다.");

                bro = await UniTask.RunOnThreadPool(() => Backend.GameData.Update(TableNames.Test, new Where(), param));
            }
            else
            {
                Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");
                
                bro = await UniTask.RunOnThreadPool(() => Backend.GameData.UpdateV2(TableNames.Test, gameDataRowInDate, Backend.UserInDate, param));
            }

            await UniTask.Delay(5000);
            return bro;
        }
    }
}