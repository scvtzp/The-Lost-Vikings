using System.Reflection;
using BackEnd;

namespace Model
{
    public class ModelBase
    {
        public Param GetParam()
        {
            Param param = new Param();
            var fields = typeof(TestModel).GetFields(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (var field in fields)
            {
                //todo : 커스텀 딕셔너리(딕셔너리 키/벨류에 클래스가 들어가는) 또는 리액티브프로퍼티 추가 시 여기서 추가 처리 필요. 
                if (false && field.FieldType == typeof(int))
                {
                    
                }

                else
                {
                    var value = field.GetValue(this); // 필드의 값을 가져옴
                    param.Add(field.Name, value);
                }
            }
            
            return param;
        }

        public Param GetParam(string[] paramName)
        {
            var param = GetParam();
            var returnParam = new Param();
            
            foreach (var name in paramName)
            {
                if (param.Contains(name))
                    returnParam.Add(name, param[name]);
            }

            return returnParam;
        }
    }
}