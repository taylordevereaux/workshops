#if __WASM__
using Uno.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TodoApp.Shared.Controls
{
    /// 📚 <see cref="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/progress"/>
    public partial class NativeProgress : Control
    {
        /// 🛈 base("progress") would create a <progress> and </progress> HTML element.
        public NativeProgress()  : base ("progress") // 🎯 Instantiate the correct HTML element.
        {
            MinHeight = 20;
            HorizontalAlignment = HorizontalAlignment.Stretch;

            UpdateAttributes();
        }

        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            (dependencyObject as NativeProgress)?.UpdateAttributes();
        }

        private static void OnMinChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            (dependencyObject as NativeProgress)?.UpdateAttributes();
        }

        private static void OnMaxChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            (dependencyObject as NativeProgress)?.UpdateAttributes();
        }

        private void UpdateAttributes()
        {
            // The minimum value is always 0 and the min attribute is not allowed for progress elements
            // https://developer.mozilla.org/en-US/docs/Web/HTML/Element/progress#Attributes

            // To override this limitation, we recalculate the value & max based on our minimum.
            var min = Minimum;

            var calculatedValue = Value - min;
            var calculatedMax = Maximum - min;

            // 🛈 Usage: SetAttribute("HtmlAttributeName", "Value");
            SetAttribute("value", calculatedValue.ToString());  // 🎯 Set HTML attribute to calculatedValue
			SetAttribute("max", calculatedMax.ToString());    // 🎯 Set HTML attribute to calculatedMax            
        }
    }
}
#endif