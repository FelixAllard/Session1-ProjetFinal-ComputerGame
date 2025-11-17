namespace Questions
{
    public abstract class IQuestion
    {
        public int Id;
        public string Question;
        public string Answer;
        public Coordinate AnswerCoordinate;
        public string Tip;
    }
}