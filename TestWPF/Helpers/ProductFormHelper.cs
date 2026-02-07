using System.Collections.Generic;
using System.Globalization;
using System.Windows.Controls;
using TestWPF.Models;

namespace TestWPF.Helpers
{
    public static class ProductFormHelper
    {
        public static void FillCombo(ComboBox combo, List<IdNameItem> items)
        {
            var list = new List<IdNameItem> { new IdNameItem { Id = 0, Name = "— не выбрано —" } };
            list.AddRange(items);
            combo.ItemsSource = list;
            combo.DisplayMemberPath = "Name";
            combo.SelectedValuePath = "Id";
            combo.SelectedIndex = 0;
        }

        public static int? Id(object? val) => val is int id && id > 0 ? id : null;

        public static (bool ok, string article, string name, string desc, decimal price, int qty, int discount, int? sId, int? mId, int? cId, int? uId, string? err) ReadForm(
            TextBox art, TextBox name, TextBox desc, TextBox price, TextBox qty, TextBox discount,
            ComboBox sup, ComboBox man, ComboBox cat, ComboBox unit)
        {
            var article = art.Text?.Trim() ?? "";
            var nameVal = name.Text?.Trim() ?? "";
            var descVal = desc.Text?.Trim() ?? "";
            if (string.IsNullOrWhiteSpace(article)) return (false, "", "", "", 0, 0, 0, null, null, null, null, "Введите артикул.");
            if (string.IsNullOrWhiteSpace(nameVal)) return (false, "", "", "", 0, 0, 0, null, null, null, null, "Введите наименование.");
            if (!decimal.TryParse(price.Text?.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var p) || p < 0) return (false, "", "", "", 0, 0, 0, null, null, null, null, "Некорректная цена.");
            if (!int.TryParse(qty.Text, out var q) || q < 0) return (false, "", "", "", 0, 0, 0, null, null, null, null, "Некорректное количество.");
            if (!int.TryParse(discount.Text, out var d) || d < 0 || d > 100) return (false, "", "", "", 0, 0, 0, null, null, null, null, "Скидка 0–100.");
            return (true, article, nameVal, descVal, p, q, d, Id(sup.SelectedValue), Id(man.SelectedValue), Id(cat.SelectedValue), Id(unit.SelectedValue), null);
        }
    }
}
