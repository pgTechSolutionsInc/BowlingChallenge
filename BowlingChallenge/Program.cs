// See https://aka.ms/new-console-template for more information
using BowlingChallenge;

Console.WriteLine("Welcome To Bowling");
Console.WriteLine(@"
  __  __  __  __
  )(__)(__)(__)(
 /  )(__)(__)(  \
 | /  )(__)(  \ |
 | | /  )(  \ | |
 '-| | /  \ | |-'
   '-| |  | |-'
     '-|  |-' .--.
       '--'  / .  \
             \'.  /
              '--'");
Console.WriteLine("\n");
Console.WriteLine("Press 'x' at any time to exit.");

var game = new Game();
while (!game.IsGameOver)
{
    Console.Write("How many pins to knock down?\t");
    var userInput = Console.ReadLine();
    if (userInput == "x")
    {
        Console.WriteLine("Exiting bowling...");
        break;
    }
    else
    {
        int.TryParse(userInput, out int pinsToKnockDown);
        game.Roll(pinsToKnockDown);
    }
}
Console.WriteLine("GAME OVER!");
Console.WriteLine($"Final Score: {game.TotalScore()}");
