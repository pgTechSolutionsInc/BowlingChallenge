using System.Text;

namespace BowlingChallenge
{
    public class Game
    {
        public Frame[] Frames { get; set; }

        public static void PrintTitle()
        {
            FramePrinter.PrintTitle();
        }

        private FramePrinter Printer { get; set; }
        public bool IsGameOver { get; set; } = false;
        public bool ShouldProcessLastFrameRoll3 { get; set; } = false;
        public int TotalScore() => Frames.Aggregate(0, (total, frame) => total + (frame?.FrameScore ?? 0));

        private int numPinsLeftStanding = 10;

        public Game()
        {
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

        /// <summary>
        /// Ensures the available pins to knockdown is between 0 and the number of remaining pins.
        /// </summary>
        /// <param name="pinsKnockedDown"></param>
        /// <returns></returns>
        private bool IsValidRoll(int pinsKnockedDown)
        {
            return pinsKnockedDown >= 0 && pinsKnockedDown <= numPinsLeftStanding;
        }

        /// <summary>
        /// Main routine to process an incoming roll
        /// </summary>
        /// <returns>The number of remaining pins</returns>
        /// <param name="pinsKnockedDown"></param>
        private int ProcessRoll(int pinsKnockedDown)
        {
            if (ShouldProcessLastFrameRoll3)
            {
                return Process3rdRoll(pinsKnockedDown);
            }
            for (int i = 0; i < Frames.Length; i++)
            {
                var f = Frames[i];
                if (f.IsComplete())
                {
                    continue;
                }

                // If our frame doesn't have a value for Roll1, populate it and await next roll
                if (!f.Roll1.HasValue)
                {
                    f.Roll1 = pinsKnockedDown;
                    return f.RemainingPins();
                }
                else
                {
                    // if 1st roll was a strike - we have to set it's bonus
                    // Note: If we're already on the last frame, we should be setting roll3 after this roll
                    if (f.IsStrikeFrame())
                    {
                        HandleStrike(f, pinsKnockedDown);
                        if (f.IsLastFrame())
                        {
                            ShouldProcessLastFrameRoll3 = true;
                            return f.RemainingPins();
                        }
                    }
                    else
                    {
                        // if the 1st roll wasn't a strike, set the 2nd roll
                        if (!f.Roll2.HasValue)
                        {
                            f.Roll2 = pinsKnockedDown;
                            // if 2nd roll didn't result in a spare, it's an open frame we can complete the frame score
                            if (f.IsOpenFrame())
                            {
                                CalculateFrameScore(f);
                            }
                            // otherwise we've just rolled a spare, time to wait for the next roll
                            // if we're on the last frame, we want the next roll to be roll3
                            if (f.IsLastFrame())
                            {
                                ShouldProcessLastFrameRoll3 = true;
                            }
                            return 10;
                        }
                        // we previously got the spare, have to add current roll to the spare's bonus1
                        // note that we don't break out of the process because we must still add the current roll
                        // to the next frame's roll1
                        else
                        {
                            f.Bonus1 = pinsKnockedDown;
                            if (f.IsLastFrame())
                            {
                                ShouldProcessLastFrameRoll3 = true;
                            }
                            else 
                            { 
                                CalculateFrameScore(f); 
                            }
                        }
                    }
                }
            }
            ShouldProcessLastFrameRoll3 = true;
            return -1;
        }
        
        /// <summary>
        /// On the 10th frame, if it's a strike or spare frame we need to allow a third roll
        /// </summary>
        /// <param name="pinsKnockedDown"></param>
        /// <returns>-1 since this was the last roll of the game</returns>
        private int Process3rdRoll(int pinsKnockedDown)
        {
            var lastFrame = Frames[9];
            if (lastFrame.IsSpareFrame())
            {
                lastFrame.Bonus1 = pinsKnockedDown;
                lastFrame.Roll3 = pinsKnockedDown;
            }
            if (lastFrame.IsStrikeFrame())
            {
                lastFrame.Bonus2 = pinsKnockedDown;
                lastFrame.Roll3 = pinsKnockedDown;
            }
            CalculateFrameScore(lastFrame);
            return -1;
        }

        private void CalculateFrameScore(Frame f)
        {
            f.CalculateFrameScore();
            if (f.IsLastFrame())
            {
                IsGameOver = true;
            }
        }

        private void HandleStrike(Frame f, int pinsKnockedDown)
        {
            if (f.Bonus1 == null)
            {
                f.Bonus1 = pinsKnockedDown;
                if (f.IsLastFrame())
                {
                    f.Roll2 = pinsKnockedDown;
                }
            }
            else
            {
                f.Bonus2 = pinsKnockedDown;
                if (f.IsLastFrame())
                {
                    f.Roll3 = pinsKnockedDown;
                }
                CalculateFrameScore(f);
            }
        }
    }
    
}
