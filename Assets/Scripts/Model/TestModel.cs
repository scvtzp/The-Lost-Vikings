using System.Collections.Generic;

namespace Model
{
    public class TestModel : ModelBase
    {
        public int level = 1;
        public float atk = 3.5f;
        public string info = string.Empty;
        public Dictionary<string, int> inventory = new Dictionary<string, int>();
        public List<string> equipment = new List<string>();
        public float dia = 0;
    }
}