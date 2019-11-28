using SQLite;

namespace EvenShare
{
    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Members { get; set; }
        public string Timestamp { get; set; }
    }
}
