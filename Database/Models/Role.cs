namespace Data.Model
{
    public class Role
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string Location { get; set; } = string.Empty;
        
    }

}
