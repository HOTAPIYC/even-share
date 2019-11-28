using SQLite;

namespace EvenShare
{
    public class Member
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public int PersonalTotal { get; set; }
        public int DiffToEvenShare { get; set; }
    }
}
