using System.Text.RegularExpressions;

namespace BowlingChallenge
{
    public class FramePrinter
    {
        private Frame[] Frames;
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

        internal void PrintFrames()
        {
            PrintHeaders();
            PrintFrameRolls();
            PrintFrameScores();
            PrintFrameFooters();
            Console.WriteLine();
        }
        private void PrintHeaders()
        {
            Console.WriteLine($" {new string('_', 8 * 10)}");
            for (var i = 0; i < Frames.Length; i++)
            {
                Console.Write($"|___{Frames[i].FrameNumber}___");
            }
            // Print 10th Frame Header
            //Console.Write($"|-----10-----")
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
                Console.Write($"| {scoreToDisplay,3}   ");
            }
            Console.Write(" |\n");
        }
        private void PrintFrameFooters()
        {
            for (var i = 0; i < Frames.Length; i++)
            {
                Console.Write("|_______");
            }
            Console.Write("_|");
        }
    }
}
