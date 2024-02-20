namespace Eisenhower_CMD {
    public class Tasks {
        public string Name;
        public string Description;
        public int CreationDate;
        public int DueDate;
        public Users UserAcc;
        public eImportance Importance;
    }
    public enum eImportance {
        TWO, 
        THREE, 
        FIVE,
        SEVEN
    }

}