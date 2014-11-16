namespace Gu.Wpf.NumericControls
{
    using System.ComponentModel;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    [TemplatePart(Name = "PART_Text", Type = typeof(TextBox))]
    public abstract class BaseUpDown : Control
    {
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
            "IsReadOnly",
            typeof(bool),
            typeof(BaseUpDown),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.None,
                OnIsReadOnlyChanged));

        public static readonly DependencyProperty ReadOnlyForegroundProperty = DependencyProperty.Register(
            "ReadOnlyForeground",
            typeof(Color),
            typeof(BaseUpDown),
            new FrameworkPropertyMetadata(
                SystemColors.InactiveCaptionColor,
                FrameworkPropertyMetadataOptions.None,
                OnReadOnlyForegroundChanged));

        public static readonly DependencyProperty ValidateOnInputProperty =
            DependencyProperty.Register(
                "ValidateOnInput",
                typeof(bool),
                typeof(BaseUpDown),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.None,
                    OnValidateOnInputChanged));

        public static readonly DependencyProperty HasFocusProperty =
                DependencyProperty.Register(
                "HasFocus",
                typeof(bool),
                typeof(BaseUpDown),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.None,
                    OnHasFocusChanged));

        public static readonly DependencyProperty SetFocusProperty =
            DependencyProperty.Register(
                "SetFocus",
                typeof(bool),
                typeof(BaseUpDown),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.None,
                    OnSetFocusChanged));

        /// <summary>
        /// Identifies the Suffix property
        /// </summary>
        public static readonly DependencyProperty SuffixProperty = DependencyProperty.Register(
            "Suffix",
            typeof(string),
            typeof(BaseUpDown),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
                {
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        /// <summary>
        /// Identifies the AllowSpinners property
        /// </summary>
        public static readonly DependencyProperty AllowSpinnersProperty = DependencyProperty.Register(
            "AllowSpinners",
            typeof(bool),
            typeof(BaseUpDown),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsArrange)
                {
                    BindsTwoWayByDefault = true,
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

        private static readonly DependencyPropertyKey IncreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "IncreaseCommand",
            typeof(ICommand),
            typeof(BaseUpDown),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the IncreaseCommand property
        /// </summary>
        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "DecreaseCommand",
            typeof(ICommand),
            typeof(BaseUpDown),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the DecreaseCommand property
        /// </summary>
        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

        static BaseUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BaseUpDown),
                new FrameworkPropertyMetadata(typeof(BaseUpDown)));
        }

        protected BaseUpDown()
        {
            this.Init = false;
            IncreaseCommand = new ManualRelayCommand(Increase, CanIncrease);
            DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public bool IsReadOnly
        {
            get
            {
                return (bool)this.GetValue(IsReadOnlyProperty);
            }
            set
            {
                this.SetValue(IsReadOnlyProperty, value);
            }
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public bool ValidateOnInput
        {
            get
            {
                return (bool)this.GetValue(ValidateOnInputProperty);
            }
            set
            {
                this.SetValue(ValidateOnInputProperty, value);
            }
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public Color ReadOnlyForeground
        {
            get
            {
                return (Color)this.GetValue(ReadOnlyForegroundProperty);
            }
            set
            {
                this.SetValue(ReadOnlyForegroundProperty, value);
            }
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public string Suffix
        {
            get
            {
                return (string)this.GetValue(SuffixProperty);
            }
            set
            {
                this.SetValue(SuffixProperty, value);
            }
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public bool HasFocus
        {
            get
            {
                return (bool)this.GetValue(HasFocusProperty);
            }
            set
            {
                this.SetValue(HasFocusProperty, value);
            }
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public bool SetFocus
        {
            get
            {
                return (bool)this.GetValue(SetFocusProperty);
            }
            set
            {
                this.SetValue(SetFocusProperty, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public bool AllowSpinners
        {
            get
            {
                return (bool)this.GetValue(AllowSpinnersProperty);
            }
            set
            {
                this.SetValue(AllowSpinnersProperty, value);
            }
        }

        public bool HasBeenInitialized
        {
            get
            {
                return this.Init;
            }
        }

        protected TextBox TextBox { get; private set; }

        /// <summary>
        /// Increases the value by increment
        /// </summary>
        public ICommand IncreaseCommand
        {
            get
            {
                return (ICommand)GetValue(IncreaseCommandProperty);
            }
            protected set
            {
                SetValue(IncreaseCommandPropertyKey, value);
            }
        }

        /// <summary>
        /// Decreases the value by increment
        /// </summary>
        public ICommand DecreaseCommand
        {
            get
            {
                return (ICommand)GetValue(DecreaseCommandProperty);
            }
            private set
            {
                SetValue(DecreaseCommandPropertyKey, value);
            }
        }

        protected bool Init { get; private set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.TextBox != null)
            {
                //this.TextBox.PreviewKeyDown -= this._textBox_PreviewKeyDown;
                this.TextBox.PreviewTextInput -= this._textBox_PreviewTextInput;
                this.TextBox.LostFocus -= this.Validate;
                this.TextBox.GotFocus -= this._textBox_GotFocus;
                DataObject.RemovePastingHandler(this.TextBox, this.CheckPasteFormat);
            }
            this.TextBox = this.GetTemplateChild("PART_Text") as TextBox;
            if (this.TextBox != null)
            {
                //this.TextBox.PreviewKeyDown += this._textBox_PreviewKeyDown;
                this.TextBox.PreviewTextInput += this._textBox_PreviewTextInput;
                this.TextBox.LostFocus += this.Validate;
                this.TextBox.GotFocus += this._textBox_GotFocus;
                this.TextBox.IsReadOnly = this.IsReadOnly;
                this.TextBox.Foreground = this.IsReadOnly ? new SolidColorBrush(this.ReadOnlyForeground) : this.Foreground;
                // this.TextBox.BorderThickness = this.IsReadOnly ? new Thickness(0) : new Thickness(System.Windows.Forms.SystemInformation.BorderSize.Width);

                DataObject.AddPastingHandler(this.TextBox, this.CheckPasteFormat);
            }

            if (!this.Init)
            {
                this.Init = true;

                this.ConvertValueToText();
                this.CheckSpinners();
            }
        }

        public void CheckPasteFormat(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.Text, true);
            if (!isText || this.IsReadOnly)
            {
                return;
            }

            var text = e.SourceDataObject.GetData(DataFormats.Text) as string;

            bool ok = this.ValidateInput(text);

            if (!ok)
            {
                e.Handled = true;
                e.CancelCommand();
            }
        }

        protected virtual void Validate(object sender, RoutedEventArgs e)
        {
            this.HasFocus = false;
            this.Validate();
        }

        protected void Validate()
        {
            string text = this.TextBox.Text;
            text = text.Trim();
            if (this.Suffix != string.Empty)
                text = text.Replace(this.Suffix, "");
            this.ValidateText(text.Trim());
        }

        /// <summary>
        /// Invoked when IncreaseCommand.Execute() is executed
        /// </summary>
        /// <returns></returns>
        protected abstract void Increase();

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanIncrease()
        {
            if (IsReadOnly)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Invoked when DecreaseCommand.Execute() is executed
        /// </summary>
        /// <returns></returns>
        protected abstract void Decrease();

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanDecrease()
        {
            if (IsReadOnly)
            {
                return false;
            }
            return true;
        }

        protected abstract void ValidateText(string text);

        protected abstract string ValidInput();

        protected abstract bool ValidValue(string text);

        protected abstract void ConvertValueToText();

        protected virtual void CheckSpinners()
        {
            // Not nice to cast like this but want to have ManualRelayCommand as internal
            ((ManualRelayCommand)IncreaseCommand).TryRaiseCanExecuteChanged();
            ((ManualRelayCommand)DecreaseCommand).TryRaiseCanExecuteChanged();
        }

        private static void OnValidateOnInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnReadOnlyForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        
        private static void OnHasFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var baseUpDown = (BaseUpDown)d;
            baseUpDown.CheckSpinners();
        }

        private static void OnSetFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                var obj = d as BaseUpDown;
                if (obj != null)
                {
                    obj.TextBox.Focus();
                }
            }
        }

        private void RepeatButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.TextBox.Focus();
        }

        private void _textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.HasFocus = true;
        }

        private void BaseUpDown_GotFocus(object sender, RoutedEventArgs e)
        {
            this.TextBox.Focus();
        }

        private void _textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //switch (e.Key)
            //{
            //    case Key.Up:
            //        {
            //            this.Increase();
            //            break;
            //        }
            //    case Key.Down:
            //        {
            //            this.Decrease();
            //            break;
            //        }
            //}
        }
       
        private void _textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (this.IsReadOnly)
            {
                e.Handled = true;
            }
            else
            {
                TextBox box = sender as TextBox;

                string text = box.Text;

                text = text.Remove(box.SelectionStart, box.SelectionLength);
                text = text.Insert(box.SelectionStart, e.Text);

                if (!this.ValidateInput(text))
                {
                    e.Handled = true;
                }
            }
        }
        
        private bool ValidateInput(string text)
        {
            bool ok = true;
            if (this.Suffix != "")
                text = text.Replace(this.Suffix, "").Trim();
            else
                text = text.Trim();

            if (!Regex.IsMatch(text, this.ValidInput()))
            {
                ok = false;
            }
            if (ok && this.ValidateOnInput && Regex.IsMatch(text, @"\d") && !this.ValidValue(text))
            {
                ok = false;
            }

            return ok;
        }
    }
}