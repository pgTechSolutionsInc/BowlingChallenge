using System.Text;

namespace BowlingChallenge
{
    public class Game
    {
        public Frame[] Frames { get; set; }
        private FramePrinter Printer { get; set; }
        public int TotalScore() => Frames.Aggregate(0, (total, frame) => total + (frame?.FrameScore ?? 0));
        public bool IsGameOver { get; set; } = false;
        public bool IsProcessingLastFrame { get; set; } = false; 
        
        private int numPinsLeftStanding = 10;

        public Game()
        {
            // extra frame to account for the 10th frame special case
            Frames = new Frame[10];
            for (int i = 0; i < Frames.Length; i++)
            {
                Frames[i] = new Frame(i + 1);
            }
            Printer = new FramePrinter(Frames);
        }

        private int GetRandomPinsToHit(int maxNumberOfPinsToHit = 10)
        {
            return new Random().Next(maxNumberOfPinsToHit);
        }
        public void Roll(int pinsKnockedDown)
        {
            if (IsValidRoll(pinsKnockedDown))
            {
                //var pinsKnockedDown = GetRandomPinsToHit(numPinsLeftStanding);
                Console.WriteLine($"Rolled: {pinsKnockedDown}.");
                numPinsLeftStanding = ProcessRoll(pinsKnockedDown);
                Printer.PrintFrames();
            }
            else
            {
                Console.WriteLine($"Only {numPinsLeftStanding} pins can be knocked down. Try again.");
            }
        }

        private bool IsValidRoll(int pinsKnockedDown)
        {
            return pinsKnockedDown >= 0 && pinsKnockedDown <= numPinsLeftStanding;
        }

        /// <summary>
        ///  Returns the number of pins remainining
        /// </summary>
        /// <param name="pinsKnockedDown"></param>
        private int ProcessRoll(int pinsKnockedDown)
        {
            if (!IsProcessingLastFrame)
            {
                for (int i = 0; i < Frames.Length; i++)
                {
                    var currentFrame = Frames[i];
                    if (currentFrame.IsComplete())
                    {
                        continue;
                    }
                    if (!currentFrame.Roll1.HasValue)
                    {
                        currentFrame.Roll1 = pinsKnockedDown;
                        if (pinsKnockedDown == 10) Console.WriteLine("STRIKE!");
                        return pinsKnockedDown == 10 ? 10 : 10 - pinsKnockedDown;
                    }
                    else
                    {
                        // if the 1st roll wasn't a strike, set the 2nd roll
                        if (currentFrame.Roll1 != 10)
                        {
                            if (!currentFrame.Roll2.HasValue)
                            {
                                currentFrame.Roll2 = pinsKnockedDown;
                                // if 2nd roll didn't result in a spare, we can complete the frame score
                                if (currentFrame.Roll2 != 10 - currentFrame.Roll1)
                                {
                                    // This is an open frame. We can compute score and move on to next roll.
                                    currentFrame.CalculateFrameScore();
                                }
                                else { Console.WriteLine("SPARE!"); }
                                return 10;
                            }
                            // already got the spare, have to add roll to the spare's bonus1
                            else
                            {
                                currentFrame.Bonus1 = pinsKnockedDown;
                                currentFrame.CalculateFrameScore();
                            }
                        }
                        // else, 1st roll was a strike - we have to set it's bonus
                        // Note: If we're already on the last frame,

                        else
                        {

                            if (currentFrame.Bonus1 == null)
                            {
                                currentFrame.Bonus1 = pinsKnockedDown;
                                if (currentFrame.FrameNumber == 10)
                                {
                                    Console.WriteLine("On the last frame!");
                                    IsProcessingLastFrame = true;
                                    return pinsKnockedDown == 10 ? 10 : 10 - pinsKnockedDown;
                                }
                            }
                            else
                            {
                                currentFrame.Bonus2 = pinsKnockedDown;
                                currentFrame.CalculateFrameScore();
                            }
                        }
                    }
                }
            }
            else
            {
                Frames[9].Bonus2 = pinsKnockedDown;
                Frames[9].CalculateFrameScore();
                IsGameOver = true;
                return -1;
            }
            return -1;
        }
    }
    
}
