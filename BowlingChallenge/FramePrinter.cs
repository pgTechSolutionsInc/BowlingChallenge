using System.Text.RegularExpressions;

namespace BowlingChallenge
{
    public class FramePrinter
    {
        private readonly Frame[] Frames;

        /// <summary>
        /// Recursively compute the sum of previous frames for display purposes
        /// Begin on the current frame index and add up each previous frame's FrameScore
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <returns>The aggregate of current and all previous frame scores</returns>
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
            Console.WriteLine("Welcome To Paul's Bowling Challenge");
            Console.ForegroundColor = ConsoleColor.Green;
            // Art by Joan Stark
            //https://www.asciiart.eu/sports-and-outdoors/bowling
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
            Console.ForegroundColor= ConsoleColor.Gray;
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
        private static string HyphenizedZeros(string score) => Regex.Replace(score, @"\b0\b", "-");
        private static string FormattedRoll1(Frame frame) => 
            frame.IsStrikeFrame() ? 
            "X" : 
            HyphenizedZeros((frame?.Roll1.ToString() ?? ""));
        private static string FormattedRoll2(Frame frame)
        {
            if (frame.IsSpareFrame()) return "/";
            if (frame.IsLastFrame() && frame.Roll2 == 10) return "X";
            return HyphenizedZeros((frame?.Roll2.ToString() ?? ""));
        }
        private static string FormattedRoll3(Frame frame)
        {
            if (frame.IsOpenFrame() || !frame.IsLastFrame() || !frame.Roll3.HasValue) return " ";
            if (frame.Roll3 + frame.Roll2 == 10) return "/";
            if (frame.Roll3 == 10) return "X";
            return HyphenizedZeros(frame.Roll3.ToString() ?? "");

        }
        private void PrintFrameRolls()
        {
            for (var i = 0; i < Frames.Length; i++)
            {
                var currentFrame = Frames[i];
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{FormattedRoll1(currentFrame),3}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("|");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{ FormattedRoll2(currentFrame),3}");
                Console.ForegroundColor = ConsoleColor.White;
                if (currentFrame.IsLastFrame())
                {
                    Console.Write("|");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"{FormattedRoll3(currentFrame),3}");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
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
                Console.ForegroundColor = ConsoleColor.White;
                if (i == 9)
                {
                    Console.Write("|");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($" {scoreToDisplay,7}   ");
                }
                else
                {
                    Console.Write("|");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write($" {scoreToDisplay,3}   ");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(" |\n");
        }
        private void PrintFrameFooters()
        {
            Console.ForegroundColor = ConsoleColor.White;
            for (var i = 0; i < Frames.Length; i++)
            {
                var underscores = new string('_', i == 9 ? 11 : 7);
                Console.Write($"|{underscores}");
            }
            Console.Write("_|");
        }
    }
}
