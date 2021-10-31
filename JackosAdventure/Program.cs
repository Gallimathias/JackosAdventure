namespace JackosAdventure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var game = new AdventureGame();
            game.Run();
        }
    }
}
