
namespace Agario
{
    public static class Program
    {
        public static void Main()
        {
            using (Game game = new Game(1000, 1000))
            {
                game.Run();
            }
        }
    }
}