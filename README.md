# Gu.Wpf.NumericInput
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.NumericInput.svg)](https://www.nuget.org/packages/Gu.Wpf.NumericInput/)
[![Build status](https://ci.appveyor.com/api/projects/status/a92oxrywc9nv7f21?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-numericinput)
[![Join the chat at https://gitter.im/JohanLarsson/Gu.Wpf.NumericInput](https://badges.gitter.im/JohanLarsson/Gu.Wpf.NumericInput.svg)](https://gitter.im/JohanLarsson/Gu.Wpf.NumericInput?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Textboxes for numeric input in WPF.
- DoubleBox
- IntBox
- DecimalBox
- FloatBox
- ShortBox
- Easy to add more

## Contents

- [Simple sample](#simple-sample)
- [Sample showing some of the properties](#sample-showing-some-of-the-properties)
- [Features:](#features)
    - [Validation](#validation)
- [Properties](#properties)
    - [Culture](#culture)
    - [ValidationTrigger](#validationtrigger)
    - [CanValueBeNull](#canvaluebenull)
    - [StringFormat](#stringformat)
    - [DecimalDigits](#decimaldigits)
    - [NumberStyles](#numberstyles)
    - [RegexPattern](#regexpattern)
    - [MaxValue](#maxvalue)
    - [MinValue](#minvalue)
- [Spinners](#spinners)
    - [IncrementCommand and DecrementCommand](#incrementcommand-and-decrementcommand)
    - [Increment](#increment)
    - [AllowSpinners](#allowspinners)
- [Attached properties](#attached-properties)
    - [Gu.Wpf.NumericInput.NumericBox](#numericbox)
        - [NumericBox.Culture](#numericboxculture)
        - [NumericBox.ValidationTrigger](#numericboxvalidationtrigger)
        - [NumericBox.CanValueBeNull](#numericboxcanvaluebenull)
        - [NumericBox.NumberStyles](#numericboxnumberstyles)
        - [NumericBox.StringFormat](#numericboxstringformat)
        - [NumericBox.DecimalDigits](#numericboxdecimaldigits)
        - [NumericBox.AllowSpinners](#numericboxallowspinners)
    - [Gu.Wpf.NumericInput.Select.TextBox](#guwpfnumericinputselecttextbox)
        - [TextBox.SelectAllOnGotKeyboardFocus](#textboxselectallongotkeyboardfocus)
        - [TextBox.SelectAllOnClick](#textboxselectallonclick)
        - [TextBox.SelectAllOnDoubleClick](#textboxselectallondoubleclick)
        - [TextBox.MoveFocusOnEnter](#textboxmovefocusonenter)
    - [Gu.Wpf.NumericInput.Touch.TextBox](#guwpfnumericinputtouchtextbox)
        - [TextBox.ShowTouchKeyboardOnTouchEnter](#textboxshowtouchkeyboardontouchenter)
- [Style and Template keys](#style-and-template-keys)
    
### Simple sample
The `Text`property is used internally and will throw if you bind to it.

Bind the `Value`property of the boxes like this:
```
<numericInput:DoubleBox Value="{Binding DoubleValue}" />
```
### Sample showing some of the properties
```
<numeric:SpinnerDecorator>
    <numeric:DoubleBox Value="{Binding Value,
                                       ValidatesOnNotifyDataErrors=True}" 
                       CanValueBeNull="{Binding CanValueBeNull}"
					   ValidationTrigger="PropertyChanged"
                       MaxValue="10"
                       MinValue="-10"
                       NumberStyles="AllowDecimalPoint,AllowLeadingSign"
                       RegexPattern="\1\d+(\.2\d+)"
                       StringFormat="N2"
					   AllowSpinners="True"
					   Increment="{Binding Increment}"/>
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

![render](http://i.imgur.com/ru5TESv.gif)

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
###### NumericBox.Culture
Controls what Culture to use.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `BaseBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.Culture="sv-se">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

###### NumericBox.ValidationTrigger
Controls when validation is performed.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `BaseBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.ValidationTrigger="PropertyChanged">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

###### NumericBox.CanValueBeNull
Controls if empty means null and is a legal value.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `BaseBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.CanValueBeNull="True">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

###### NumericBox.NumberStyles
Controls if empty means null and is a legal value.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `BaseBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.NumberStyles="AllowDecimalPoint, AllowLeadingSign">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

###### NumericBox.StringFormat
Controls how many decimal digits are shown.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `BaseBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.StringFormat="F2">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

###### NumericBox.DecimalDigits
Controls how many decimal digits are shown.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `DecimalDigitsBox<T>`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.DecimalDigits="-2">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

###### NumericBox.AllowSpinners
Allows spinners on all child NumericBoxes.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `BaseBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:numeric="http://gu.se/NumericInput">
    <Grid numeric:NumericBox.AllowSpinners="True">
		...
	    <numeric:DoubleBox .../>
	    <numeric:DoubleBox .../>
	    ...
	</Grid>
```

##### Gu.Wpf.NumericInput.Select.TextBox
###### TextBox.SelectAllOnGotKeyboardFocus
Selects all text in a textbox whgen it gets keyboard focus.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `TextBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:select="http://gu.se/Select">
    <Grid select:TextBox.SelectAllOnGotKeyboardFocus="True">
		...
	    <numeric:DoubleBox .../>
	    <TextBox .../>
	    ...
	</Grid>
```

###### TextBox.SelectAllOnClick
Selects all text in a textbox on click.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `TextBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:select="http://gu.se/Select">
    <Grid select:TextBox.SelectAllOnClick="True">
		...
	    <numeric:DoubleBox .../>
	    <TextBox .../>
	    ...
	</Grid>
```

###### TextBox.SelectAllOnDoubleClick

Selects all text in a textbox on doubleclick.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `TextBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:select="http://gu.se/Select">
    <Grid select:TextBox.SelectAllOnDoubleClick="True">
		...
	    <numeric:DoubleBox .../>
	    <TextBox .../>
	    ...
	</Grid>
```

###### TextBox.MoveFocusOnEnter

Captures enter key and cycles focus to next control.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `TextBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:select="http://gu.se/Select">
    <Grid select:TextBox.MoveFocusOnEnter="True"
	      KeyboardNavigation.TabNavigation="Cycle">
		...
	    <numeric:DoubleBox .../>
	    <TextBox .../>
	    ...
	</Grid>
```

##### Gu.Wpf.NumericInput.Touch.TextBox
###### TextBox.ShowTouchKeyboardOnTouchEnter
Shows onscreen keyboard on touch enter for all controls inheriting from `TextBox`.
Inherits so setting it on a panel or window sets it for all child controls inheriting from `TextBox`.

Sample:

```xaml
<UserControl x:Class="Gu.Wpf.NumericInput.Demo.DemoView"
             ...
             xmlns:touch="http://gu.se/Touch">
    <Grid touch:TextBox.ShowTouchKeyboardOnTouchEnter="True">
		...
	    <numeric:DoubleBox .../>
	    <TextBox .../>
	    ...
	</Grid>
```

### Style and Template keys
- `NumericBox.BaseBoxStyleKey`
- `NumericBox.SpinnersTemplateKey`
- `NumericBox.SpinnerPathStyleKey`
- `NumericBox.SpinnerButtonStyleKey`
