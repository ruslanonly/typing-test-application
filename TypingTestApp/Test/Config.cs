namespace TypingTestApp
{
    public enum WordAmount
    {
        Ten = 10,
        Twenty = 20,
        Thirty = 30,
        Fifty = 50,
        SeventyFive = 75
    }
    public enum WordGroup
    {
        English,
        Russian
    }

    class Config
    {
        static public WordGroup wordGroup;
        static public WordAmount wordAmount;
        
        static public void InitConfig()
        {
            wordGroup = WordGroup.English;
            wordAmount = WordAmount.Thirty;
        }
    }
}
