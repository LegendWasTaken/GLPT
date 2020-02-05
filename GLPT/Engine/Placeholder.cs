namespace GLPT
{
    public class Placeholder
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Placeholder(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}