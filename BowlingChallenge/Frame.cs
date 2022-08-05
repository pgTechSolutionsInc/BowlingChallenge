namespace BowlingChallenge
{
    public class Frame
    {
        public Frame(int frameNumber)
        {
            FrameNumber = frameNumber;
        }
        public int FrameNumber { get; set; }
        public int? Roll1 { get; set; }
        public int? Roll2 { get; set; }
        /// <summary>
        /// only used for 10th frame printing
        /// </summary>
        public int? Roll3 { get; set; }

        /// <summary>
        /// tracks one roll after picking up a spare or strike
        /// </summary>
        public int? Bonus1 { get; set; }
        /// <summary>
        /// tracks two rolls after picking up a strike
        /// </summary>
        public int? Bonus2 { get; set; }
        public int? FrameScore { get; set; }
        public int RemainingPins() => IsStrikeFrame() ? 10 : 10 - (Roll1 ?? 0);
        public bool IsComplete()
        {
            if (Roll1.HasValue)
            {
                if (IsStrikeFrame()) return Bonus1.HasValue && Bonus2.HasValue;
                if (Roll2.HasValue)
                {
                    if (IsSpareFrame()) return Bonus1.HasValue;
                    if (IsOpenFrame()) return true;
                }
            }
            return false;
        }
        public bool IsStrikeFrame() => Roll1 == 10;
        public bool IsSpareFrame() => !IsStrikeFrame() && Roll1 + Roll2 == 10;
        public bool IsOpenFrame() => !IsStrikeFrame() && !IsSpareFrame();
        public bool IsLastFrame() => FrameNumber == 10;
        public void CalculateFrameScore()
        {
            if (IsComplete())
            {
                if (IsOpenFrame()) FrameScore = Roll1 + Roll2;
                if (IsSpareFrame()) FrameScore = 10 + Bonus1;
                if (IsStrikeFrame()) FrameScore = 10 + Bonus1 + Bonus2;
            }
        }
    }
}