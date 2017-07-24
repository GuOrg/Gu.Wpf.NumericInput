# Gu.Wpf.NumericInput
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/v/Gu.Wpf.NumericInput.svg)](https://www.nuget.org/packages/Gu.Wpf.NumericInput/)
[![Build status](https://ci.appveyor.com/api/projects/status/e0nwl8lha4nsoxq7/branch/master?svg=true)](https://ci.appveyor.com/project/JohanLarsson/gu-wpf-numericinput/branch/master)
[![Join the chat at https://gitter.im/JohanLarsson/Gu.Wpf.NumericInput](https://badges.gitter.im/JohanLarsson/Gu.Wpf.NumericInput.svg)](https://gitter.im/JohanLarsson/Gu.Wpf.NumericInput?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Textboxes for numeric input in WPF.
- DoubleBox
- IntBox
- DecimalBox
- FloatBox
- ShortBox
- Easy to add more

# Contents

- [1. Samples](#1-samples)
  - [1.1. Simple sample](#11-simple-sample)
  - [1.2. Sample showing some of the properties](#12-sample-showing-some-of-the-properties)
- [2. Features:](#2-features)
- [3. Validation](#3-validation)
  - [3.1. ValidationTrigger](#31-validationtrigger)
  - [3.2. CanValueBeNull](#32-canvaluebenull)
  - [3.3. MinValue](#33-minvalue)
  - [3.4. MaxValue](#34-maxvalue)
  - [3.5. NumberStyles](#35-numberstyles)
  - [3.6. Culture](#36-culture)
  - [3.7. RegexPattern](#37-regexpattern)
- [4. Formatting](#4-formatting)
  - [4.1. Culture](#41-culture)
  - [4.1. NumberStyles](#41-numberstyles)
  - [4.2 StringFormat](#42-stringformat)
  - [4.3 DecimalDigits](#43-decimaldigits)
- [5. Spinners](#5-spinners)
  - [5.1. Increment](#51-increment)
  - [5.2. AllowSpinners](#52-allowspinners)
  - [5.3. IncrementCommand and DecrementCommand](#53-incrementcommand-and-decrementcommand)
- [5. Attached properties](#5-attached-properties)
  - [5.1. NumericBox](#51-numericbox)
    - [5.1.1. NumericBox.Culture](#511-numericboxculture)
    - [5.1.2. NumericBox.ValidationTrigger](#512-numericboxvalidationtrigger)
    - [5.1.3. NumericBox.CanValueBeNull](#513-numericboxcanvaluebenull)
    - [5.1.4. NumericBox.NumberStyles](#514-numericboxnumberstyles)
    - [5.1.5. NumericBox.StringFormat](#515-numericboxstringformat)
    - [5.1.6. NumericBox.DecimalDigits](#516-numericboxdecimaldigits)
    - [5.1.7. NumericBox.AllowSpinners](#517-numericboxallowspinners)
  - [5.2. Gu.Wpf.NumericInput.Select.TextBox](#52-guwpfnumericinputselecttextbox)
    - [5.2.1. TextBox.SelectAllOnGotKeyboardFocus](#521-textboxselectallongotkeyboardfocus)
    - [5.2.2. TextBox.SelectAllOnClick](#522-textboxselectallonclick)
    - [5.2.3. TextBox.SelectAllOnDoubleClick](#523-textboxselectallondoubleclick)
    - [5.2.4. TextBox.MoveFocusOnEnter](#524-textboxmovefocusonenter)
  - [5.3 Gu.Wpf.NumericInput.Touch.TextBox](#53-guwpfnumericinputtouchtextbox)
    - [5.3.1. TextBox.ShowTouchKeyboardOnTouchEnter](#531-textboxshowtouchkeyboardontouchenter)
- [6. Style and Template keys](#6-style-and-template-keys)

# 1. Samples
    
## 1.1. Simple sample
The `Text`property is used internally and will throw if you bind to it.

Bind the `Value`property of the boxes like this:
```xaml
<numericInput:DoubleBox Value="{Binding DoubleValue}" />
```

## 1.2. Sample showing some of the properties
```xaml
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

# 2. Features:
- Validation
  - Validates as you type even with UpdateSourceTrigger=LostFocus for the binding. Configurable via `ValidationTrigger`
  - Uses vanilla WPF validation
  - If there is a validation error no value is sent to the viewmodel.
- Works with WPF focus.
- Formatting using formatted view and edit view.
- Compatible with WPF undo/redo.
- SpinnerDecorator for up/down buttons.

![render](http://i.imgur.com/ru5TESv.gif)

# 3. Validation

## 3.1. ValidationTrigger
Control when validation is performed, the default is `LostFocus` to be consistent with vanilla WPF `TextBox`
Setting `ValidationTrigger="PropertyChanged"` validates as you type even if the binding has `UpdateSourceTrigger=LostFocus.`
Available as inheriting attached property: `NumericBox.ValidationTrigger`

## 3.2. CanValueBeNull
Sets if empty textbox sends null to the binding source and is a legal value. Useful when binding to a `double?` for example.
Available as inheriting attached property: `NumericBox.CanValueBeNull`

## 3.3. MinValue
The minimum value. The default is null meaning that no validation on min is performed.
When using spinners clicking decrease truncates to min if it would decrement below min.
If user enters a value less than `MinValue` the validation returns an `IsLessThanValidationResult`
With the content as a string `Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__`
The message is localized using `box.Culture`.

## 3.4. MaxValue
The maximum value. The default is null meaning that no validation on max is performed.
When using spinners clicking increase truncates to max if it would increment above max.
If user enters a value greater than `MaxValue` the validation returns an `IsGreaterThanValidationResult`
With the content as a string `Properties.Resources.Please_enter_a_value_less_than_or_equal_to__0__`
The message is localized using `box.Culture`.

## 3.5. NumberStyles
The `NumberStyles` used when parsing and formatting the value.
Ex if you want to allow leading sign or thousands.

## 3.6. Culture
The `IFormatProvider` used when parsing and formatting the value.
The default value for culture is `Thread.CurrentThread.CurrentUICulture`
Available as inheriting attached property: `NumericBox.Culture`

## 3.7. RegexPattern
A regex pattern used when validating the input.

# 4. Formatting

If `StringFormat`or `DecimalDigits` are specified a TextBlock with the formatted text is overlaid when not focused.

## 4.1. Culture
The `IFormatProvider` used when parsing and formatting the value.
The default value for culture is `Thread.CurrentThread.CurrentUICulture`
Available as inheriting attached property: `NumericBox.Culture`

## 4.1. NumberStyles
The `NumberStyles` used when parsing and formatting the value.

```xaml
<numeric:DoubleBox NumberStyles="AllowDecimalPoint, AllowLeadingSign"
                   Value="{Binding Value}" />
```

## 4.2 StringFormat
The string format used in the formatted view.

Available as inheriting attached property: `NumericBox.StringFormat`

```xaml
<numeric:DoubleBox Culture="sv-se"
                   StringFormat="#,0.00"
                   Value="{Binding Value}" />
```

## 4.3 DecimalDigits
`DecimalDigits="3"` sets stringformat to `F3` which means the value will always have three digits.

`DecimalDigits="-3"` sets stringformat to `0.###` which means the value will be rendered with up to three digits.

If the user eneters more digits they are used in the Value binding. The formatted view will round the value.

Available as inheriting attached property: `NumericBox.DecimalDigits`

# 5. Spinners
If you want to add up/down buttons you wrap the box in a spinnerdecorator liken this:

```xaml
<numeric:SpinnerDecorator>
    <numeric:DoubleBox AllowSpinners="True"
                       Increment="10"
                       Value="{Binding Value}" />
</numeric:SpinnerDecorator>
```

The spinner decorator derives from `Control` so it can be templated for easy styling.

## 5.1. Increment
How big change each increment/decrement is. Checked for overflow.
If the value is 9.5 and `Increment="1"`and `Max="10"` one click on increment will set the value to 10. 

## 5.2. AllowSpinners
Controls if spinners are visible assuming the control is wrapped in a `SpinnerDecorator`

## 5.3. SpinUpdateMode
Controls how the `IncreaseCommand` and the `DecreaseCommand`behaves.
The default is `AsBinding` meaning the value updates using the `UpdateSourceTrigger` specified in the binding. Default is lostfocus.
If set to `PropertyChanged` the binding source will be updated at each click even if the binding has `UpdateSourceTrigger = LostFocus`
This property inherits so it can be set for example on the `Window` ex: `numericInput:NumericBox.SpinUpdateMode = "PropertyChanged"`

## 5.4. IncrementCommand and DecrementCommand
The boxes exposes two command for incrementing and decrementing the current value with `Increment` clicking changes the text so it is undoable.



# 5. Attached properties
## 5.1. NumericBox
### 5.1.1. NumericBox.Culture
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

### 5.1.2. NumericBox.ValidationTrigger
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

### 5.1.3. NumericBox.CanValueBeNull
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

### 5.1.4. NumericBox.NumberStyles
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

### 5.1.5. NumericBox.StringFormat
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

### 5.1.6. NumericBox.DecimalDigits
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

### 5.1.7. NumericBox.AllowSpinners
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

## 5.2. Gu.Wpf.NumericInput.Select.TextBox
### 5.2.1. TextBox.SelectAllOnGotKeyboardFocus
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

### 5.2.2. TextBox.SelectAllOnClick
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

### 5.2.3. TextBox.SelectAllOnDoubleClick

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

### 5.2.4. TextBox.MoveFocusOnEnter

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

## 5.3 Gu.Wpf.NumericInput.Touch.TextBox
### 5.3.1. TextBox.ShowTouchKeyboardOnTouchEnter
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

# 6. Style and Template keys
- `NumericBox.BaseBoxStyleKey`
- `NumericBox.SpinnersTemplateKey`
- `NumericBox.SpinnerPathStyleKey`
- `NumericBox.SpinnerButtonStyleKey`
