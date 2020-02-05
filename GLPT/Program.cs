namespace GLPT
{
    class Program
    {
        static void Main(string[] args)
        {
            var scene = new Scene();
            var engine = new Engine(1280, 720, scene);
            engine.Initialize();
        }
    }
}