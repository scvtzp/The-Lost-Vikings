namespace DefaultNamespace
{
    //todo : 삭제
    /// <summary>
    /// 아니 이거 공식으로 기능이 있네?
    /// 똑같은거 같아서 그냥 그거 씀.
    /// 나중에 파일 삭제 요망
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingleTon<T> where T : class, new()
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }
    }
}