namespace FavoriteLocations.Models
{
    public class PickerItem<T>
    {
        public string Text { get; set; }
        public T Value { get; set; }

        public static PickerItem<T> Create(string text, T value)
        {
            return new PickerItem<T> { Text = text, Value = value };
        }
    }
}