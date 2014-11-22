Gu.Wpf.NumericInput
===================
Controls for numeric input in WPF.
Features:
- Realtime validation of input.
- Validates as you type even with UpdateSourceTrigger=LostFocus
- Validates:
  - CanParse
  - Min & Max
  - Regex
  - Exceptions
  - DataError
- No template parts for easy styling.
- Uses WPF validation
- Uses standard .net parsing of strings.
- Uses explicit culture.
- Extends the wpf textbox

Sample:
    
    <numericInput:DoubleBox Value="{Binding DoubleValue}" />
    
Advanced sample:

    <numericInput:DoubleBox x:Name="TweakedBox"
                        AllowSpinners="{Binding AllowSpinners}"
                        Increment="{Binding Increment}"                        
                        Decimals="{Binding Decimals}"
                        IsReadOnly="{Binding IsReadOnly}"
                        MaxValue="{Binding Max}"
                        MinValue="{Binding Min}"
                        Suffix="{Binding Suffix}"
                        Culture="{Binding Culture}"
                        Value="{Binding DoubleValue,
                                        UpdateSourceTrigger=LostFocus}" />



