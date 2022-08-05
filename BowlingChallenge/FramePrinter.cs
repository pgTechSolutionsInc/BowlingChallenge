using System.Text.RegularExpressions;

namespace BowlingChallenge
{
    public class FramePrinter
    {
        private readonly Frame[] Frames;
        /// <summary>
        /// Recursively compute the sum of previous frames for display purposes
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns></returns>
        private int SumOfPreviousFrameScores(int currentIndex)
        {
            if (currentIndex == 0) return Frames[currentIndex]?.FrameScore ?? 0;
            return (Frames[currentIndex]?.FrameScore ?? 0) + SumOfPreviousFrameScores(currentIndex - 1);
        }
        public FramePrinter(Frame[] frames)
        {
            Frames = frames;
        }
        internal static void PrintTitle()
        {
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
        }
        internal void PrintFrames()
        {
            Console.Clear();
            PrintTitle();
            PrintHeaders();
            PrintFrameRolls();
            PrintFrameScores();
            PrintFrameFooters();
            Console.WriteLine();
        }
        private void PrintHeaders()
        {
            Console.WriteLine($" {new string('_', 84)}");
            for (var i = 0; i < Frames.Length; i++)
            {
                var underscores = new string('_', i == 9 ? 5 : 3);
                Console.Write($"|{underscores}{Frames[i].FrameNumber}{underscores}");
            }
            Console.Write("|\n");
        }
        private void PrintFrameRolls()
        {
            for (var i = 0; i < Frames.Length; i++)
            {
                var roll1 = Frames[i].IsStrikeFrame() ? "X" : (Frames[i]?.Roll1.ToString() ?? "");
                var formattedRoll1 = Regex.Replace(roll1, @"\b0\b", "-");
                var roll2 = Frames[i].IsSpareFrame() ? "/" : (Frames[i]?.Roll2.ToString() ?? "");
                var formattedRoll2 = Regex.Replace(roll2, @"\b0\b", "-");
                Console.Write($"|{formattedRoll1,3}|{formattedRoll2,3}");

                // for 10th frame, lets squeeze in the 3rd roll if necessary
                if (i == 9)
                {
                    if (Frames[i].Roll3.HasValue)
                    {
                        var roll3 = Frames[i].IsStrikeFrame() ? "X" : Frames[i].IsSpareFrame() ? "/" : (Frames[i]?.Roll3.ToString() ?? "");
                        var formattedRoll3 = Regex.Replace(roll3, @"\b0\b", "-");
                        Console.Write($"|{formattedRoll3,3}");
                    }
                    else
                    {
                        Console.Write($"|{' ',3}");
                    }
                }
            }
            Console.Write(" |\n");
        }
        private void PrintFrameScores()
        {
            for (var i = 0; i < Frames.Length; i++)
            {
                var sumOfPreviousFrameScores = SumOfPreviousFrameScores(i);
                var scoreToDisplay = sumOfPreviousFrameScores > 0 && Frames[i].FrameScore.HasValue
                    ? sumOfPreviousFrameScores.ToString()
                    : "";
                if (i == 9)
                {
                    Console.Write($"| {scoreToDisplay,7}   ");
                }
                else
                {
                    Console.Write($"| {scoreToDisplay,3}   ");
                }
            }
            Console.Write(" |\n");
        }
        private void PrintFrameFooters()
        {
            for (var i = 0; i < Frames.Length; i++)
            {
                var underscores = new string('_', i == 9 ? 11 : 7);
                Console.Write($"|{underscores}");
            }
            Console.Write("_|");
        }
    }
}
