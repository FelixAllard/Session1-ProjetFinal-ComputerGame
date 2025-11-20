namespace Questions
{
    public abstract class IQuestion
    {
        public int Id;
        public string Question;

        public string Answer1;
        public string Answer2;
        public string Answer3;
        public string Answer4;
        
        public int Answer; // 0-3
        
        public Coordinate AnswerCoordinate;
        public string Tip;
    }
}