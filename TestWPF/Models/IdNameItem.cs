namespace TestWPF.Models
{
    public class IdNameItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() => Name;
    }
}
