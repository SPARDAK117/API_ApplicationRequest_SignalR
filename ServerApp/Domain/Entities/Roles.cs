namespace Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<LoginCredential> LoginCredentials { get; set; } = new List<LoginCredential>();
    }
}
