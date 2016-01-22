# Gu.Wpf.NumericInput
### Textboxes for numeric input in WPF.

[nuget package](https://www.nuget.org/packages/Gu.Wpf.NumericInput/)

[![Join the chat at https://gitter.im/JohanLarsson/Gu.Wpf.NumericInput](https://badges.gitter.im/JohanLarsson/Gu.Wpf.NumericInput.svg)](https://gitter.im/JohanLarsson/Gu.Wpf.NumericInput?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
- DoubleBox
- IntBox
- DecimalBox
- FloatBox
- ShortBox
- Easy to add more

### Simple sample
The `Text`property is used internally and will throw if you bind to it.

Bind the `Value`property of the boxes like this:
```
<numericInput:DoubleBox Value="{Binding DoubleValue}" />
```
### Sample showing some of the properties
```
<numeric:SpinnerDecorator>
    <numeric:DoubleBox AllowSpinners="True"
                        CanValueBeNull="{Binding CanValueBeNull}"
                        Increment="{Binding Increment}"
                        MaxValue="10"
                        MinValue="-10"
                        NumberStyles="AllowDecimalPoint,AllowLeadingSign"
                        RegexPattern="\1\d+(\.2\d+)"
                        StringFormat="N2"
                        Value="{Binding Value,
                                        ValidatesOnNotifyDataErrors=True}" />
</numeric:SpinnerDecorator>
```

### Features:
- Validation
  - Validates as you type even with UpdateSourceTrigger=LostFocus for the binding. Configurable via `ValidationTrigger`
  - Uses vanilla WPF validation
  - If there is a validation error no value is sent to the viewmodel.
- Works with WPF focus.
- Formatting using formatted view and edit view.
- Compatible with WPF undo/redo.
- SpinnerDecorator for up/down buttons.

##### Validation
Validates as you type even if the binding has `UpdateSourceTrigger=LostFocus.`

The boxes has their own culture that defaults to `Thread.CurrentThread.CurrentUICulture`
- CanParse with culture and numberformat.
- Min & Max
- Regex
- DataError

### Properties
##### Culture
The default value for culture is `Thread.CurrentThread.CurrentUICulture`

Available as inheriting attached property: `NumericBox.Culture`

##### ValidationTrigger
The default value for culture is `ValidationTrigger.LostFocus`

Controls when validation is performed.

Available as inheriting attached property: `NumericBox.ValidationTrigger`

##### CanValueBeNull
If false empty textbox means validation error. If true empty textbox gets parsed as null.

Available as inheriting attached property: `NumericBox.CanValueBeNull`

##### StringFormat
The string format used in the formatted view.

Available as inheriting attached property: `NumericBox.StringFormat`

##### DecimalDigits
`DecimalDigits="3"` sets stringformat to `F3` which means the value will always have three digits.

`DecimalDigits="-3"` sets stringformat to `0.###` which means the value will be rendered with up to three digits.

If the user eneters more digits they are used in the Value binding. The formatted view will round the value.

Available as inheriting attached property: `NumericBox.DecimalDigits`

##### NumberStyles
Use the NumberStyles to control validation. Ex if you want to allow leading sign or thousands.

Available as inheriting attached property: `NumericBox.NumberStyles`

##### RegexPattern
Use the RegexPattern to add a Regex pattern that is used in validation.

##### MaxValue
Use max to specify the maximum value when validating. If null max is not checked.

##### MinValue
Use max to specify the minimum value when validating. If null min is not checked.

### Spinners
If you want to add up/down buttons you wrap the box in a spinnerdecorator liken this:

```
<numeric:SpinnerDecorator>
    <numeric:DoubleBox AllowSpinners="True"
                       Increment="10"
                       Value="{Binding Value}" />
</numeric:SpinnerDecorator>
```

The spinner decorator derives from `Control` so it can be templated for easy styling.

##### IncrementCommand and DecrementCommand
The boxes exposes two command for incrementing and decrementing the current value with `Increment` clicking changes the text so it is undoable.

##### Increment
How big change each increment/decrement is. Checked for overflow.
If the value is 9.5 and `Increment="1"`and `Max="10"` one click on increment will set the value to 10. 

##### AllowSpinners
If spinners are visible/enabled.

### Attached properties
##### NumericBox
- `NumericBox.Culture`
- `NumericBox.CanValueBeNull`
- `NumericBox.NumberStyles`
- `NumericBox.StringFormat`
- `NumericBox.DecimalDigits`
- `NumericBox.AllowSpinners`

##### Gu.Wpf.NumericInput.Select.TextBox
- `TextBox.SelectAllOnGotKeyboardFocus`
- `TextBox.SelectAllOnClick`
- `TextBox.SelectAllOnDoubleClick`
- `TextBox.MoveFocusOnEnter`

### Style and Template keys
- `NumericBox.BaseBoxStyleKey`
- `NumericBox.SpinnersTemplateKey`
- `NumericBox.SpinnerPathStyleKey`
- `NumericBox.SpinnerButtonStyleKey`
