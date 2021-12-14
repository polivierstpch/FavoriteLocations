using System.ComponentModel;
using Xamarin.Forms;

namespace FavoriteLocations.Controls
{
    public class DisablingButton : Button
    {
        private Style _normalStyle;

        public Style DisabledStyle
        {
            get => (Style)GetValue(DisabledStyleProperty);
            set => SetValue(DisabledStyleProperty, value);
        }

        public static readonly BindableProperty DisabledStyleProperty =
            BindableProperty.Create(nameof(DisabledStyle), 
                typeof(Style), 
                typeof(DisablingButton), 
                null,
                BindingMode.TwoWay, 
                null, 
                (bindable, value, newValue) => { });

        public DisablingButton()
        {
            _normalStyle = Style;

            PropertyChanged += ExtendedButton_PropertyChanged;
        }

        private void ExtendedButton_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsEnabled) && DisabledStyle != null)
            {
                if (IsEnabled)
                    Style = _normalStyle;
                else
                    Style = DisabledStyle;
            }
        }
    }
}