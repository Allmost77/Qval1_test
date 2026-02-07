namespace TestWPF.Models
{
    /// <summary>
    /// Элемент для комбобокса: показываем Name, в БД отправляем Id.
    /// </summary>
    public class IdNameItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public override string ToString() => Name;
    }
}
