using System.Text;

namespace BowlingChallenge
{
    public class Game
    {
        public Frame[] Frames { get; set; }

        internal static void PrintTitle()
        {
            FramePrinter.PrintTitle();
        }

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

        public void Roll(int pinsKnockedDown)
        {
            if (IsValidRoll(pinsKnockedDown))
            {
                numPinsLeftStanding = ProcessRoll(pinsKnockedDown);
                Printer.PrintFrames();
            }
            else
            {
                Console.WriteLine($"There are only {numPinsLeftStanding} pins standing. Please try again.");
            }
        }

        private bool IsValidRoll(int pinsKnockedDown)
        {
            return pinsKnockedDown >= 0 && pinsKnockedDown <= numPinsLeftStanding;
        }

        /// <summary>
        ///  Returns the number of pins remainining
        ///  If the roll is a strike or spare, returns 10
        ///  Otherwise, return 10 - pinsKnockedDown
        /// </summary>
        /// <param name="pinsKnockedDown"></param>
        private int ProcessRoll(int pinsKnockedDown)
        {
            for (int i = 0; i < Frames.Length; i++)
            {
                var currentFrame = Frames[i];
                if (currentFrame.IsComplete())
                {
                    continue;
                }

                // If our frame doesn't have a value for Roll1, populate it and await next roll
                if (!currentFrame.Roll1.HasValue)
                {
                    currentFrame.Roll1 = pinsKnockedDown;
                    return currentFrame.IsStrikeFrame() ? 10 : 10 - pinsKnockedDown;
                }
                else
                {
                    // if the 1st roll wasn't a strike, set the 2nd roll
                    if (currentFrame.Roll1 != 10)
                    {
                        if (!currentFrame.Roll2.HasValue)
                        {
                            currentFrame.Roll2 = pinsKnockedDown;
                            // if 2nd roll didn't result in a spare, it's an open frame we can complete the frame score
                            if (currentFrame.IsOpenFrame())
                            {
                                currentFrame.CalculateFrameScore();
                            }
                            // otherwise we've just rolled a spare, time to wait for the next roll
                            return 10;
                        }
                        // we previously got the spare, have to add current roll to the spare's bonus1
                        // note that we don't break out of the process because we must still add the current roll
                        // to the next frame's roll1
                        else
                        {
                            currentFrame.Bonus1 = pinsKnockedDown;
                            currentFrame.CalculateFrameScore();
                        }
                    }
                    // else, 1st roll was a strike - we have to set it's bonus
                    // Note: If we're already on the last frame, we may be setting roll3
                    else
                    {
                        HandleStrike(currentFrame, pinsKnockedDown);
                    }
                }
            }
            
            return -1;
        }

        private void HandleStrike(Frame currentFrame, int pinsKnockedDown)
        {
            if (currentFrame.Bonus1 == null)
            {
                currentFrame.Bonus1 = pinsKnockedDown;
                //if (currentFrame.FrameNumber == 10)
                //{
                //    Console.WriteLine("On the last frame!");
                //    IsProcessingLastFrame = true;
                //    return pinsKnockedDown == 10 ? 10 : 10 - pinsKnockedDown;
                //}
            }
            else
            {
                currentFrame.Bonus2 = pinsKnockedDown;
                currentFrame.CalculateFrameScore();
            }
        }
    }
    
}
