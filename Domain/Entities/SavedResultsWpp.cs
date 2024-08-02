namespace Domain.Entities
{
    public class SavedResultsWpp
    {

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string IsActive { get; set; }
        public string Json { get; set; }
        public string Token { get; set; }
    }
}
