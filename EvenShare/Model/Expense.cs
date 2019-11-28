using SQLite;

namespace EvenShare
{
    public class Expense
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string Member { get; set; }
        public int Amount { get; set; }
        public string Timestamp { get; set; }
    }
}
