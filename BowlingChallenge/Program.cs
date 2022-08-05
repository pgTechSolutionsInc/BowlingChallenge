// See https://aka.ms/new-console-template for more information
using BowlingChallenge;

var game = new Game();
Game.PrintTitle();
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
        if (int.TryParse(userInput, out int pinsToKnockDown))
        {
            game.Roll(pinsToKnockDown);
        }
    }
}
Console.WriteLine("GAME OVER!");
Console.WriteLine($"Final Score: {game.TotalScore()}");
