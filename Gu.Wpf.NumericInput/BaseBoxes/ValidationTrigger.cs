namespace Gu.Wpf.NumericInput
{
    public enum ValidationTrigger
    {
        /// <summary> Update validation whenever the target property changes </summary>
        PropertyChanged,
        
        /// <summary> Update validation only when target element loses focus, or when Binding deactivates </summary>
        LostFocus,
        
        /// <summary> Update validation only by explicit call to BaseBox.UpdateValidation() </summary>
        Explicit
    }
}