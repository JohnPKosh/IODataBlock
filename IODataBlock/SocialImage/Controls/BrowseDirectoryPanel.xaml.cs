using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Business.Wpf.Dialogs;

namespace SocialImage.Controls
{
    /// <summary>
    /// Interaction logic for BrowseDirectoryPanel.xaml
    /// </summary>
    public partial class BrowseDirectoryPanel : UserControl
    {
        #region Class Initialization

        public BrowseDirectoryPanel()
        {
            InitializeComponent();
            CoerceValue(DirectoryExistsProperty);
            CoerceValue(ParentDirectoryExistsProperty);
        }

        #endregion Class Initialization

        #region SelectedDirectory Dependency Property

        public string SelectedDirectory
        {
            get
            {
                return (string)GetValue(SelectedDirectoryProperty);
            }
            set
            {
                try
                {
                    if (Dispatcher.CheckAccess())
                    {
                        SetValue(SelectedDirectoryProperty, value);
                        CoerceValue(DirectoryExistsProperty);
                        //if (!String.IsNullOrWhiteSpace(value) && Directory.Exists(value)) DirectoryExists = true;
                        //else DirectoryExists = false;
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(SelectedDirectoryProperty, value);
                            //if (!String.IsNullOrWhiteSpace(value) && Directory.Exists(value)) DirectoryExists = true;
                            //else DirectoryExists = false;
                            CoerceValue(DirectoryExistsProperty);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata SelectedDirectoryMetaData = new FrameworkPropertyMetadata(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(SelectedDirectory_PropertyChanged)
            , new CoerceValueCallback(SelectedDirectory_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty SelectedDirectoryProperty =
            DependencyProperty.Register("SelectedDirectory", typeof(string), typeof(BrowseDirectoryPanel), SelectedDirectoryMetaData, new ValidateValueCallback(SelectedDirectory_Validate));

        private static void SelectedDirectory_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
            //var newVal = e.NewValue as string;

            dobj.CoerceValue(DirectoryExistsProperty);
            dobj.CoerceValue(ParentDirectoryExistsProperty);

            //if (!String.IsNullOrWhiteSpace(newVal) && Directory.Exists(newVal)) SetValue(DirectoryExistsProperty, value);
            //else DirectoryExists = false;
        }

        private static object SelectedDirectory_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency SelectedDirectory value is reevaluated. The return value is the
            //latest value set to the dependency SelectedDirectory
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool SelectedDirectory_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion SelectedDirectory Dependency Property

        #region LabelText Dependency Property

        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                try
                {
                    if (Dispatcher.CheckAccess())
                    {
                        SetValue(LabelTextProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(LabelTextProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata LabelTextMetaData = new FrameworkPropertyMetadata(""
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(LabelText_PropertyChanged)
            , new CoerceValueCallback(LabelText_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(BrowseDirectoryPanel), LabelTextMetaData, new ValidateValueCallback(LabelText_Validate));

        private static void LabelText_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object LabelText_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency LabelText value is reevaluated. The return value is the
            //latest value set to the dependency LabelText
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool LabelText_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion LabelText Dependency Property

        #region ButtonText Dependency Property

        public string ButtonText
        {
            get
            {
                return (string)GetValue(ButtonTextProperty);
            }
            set
            {
                try
                {
                    if (Dispatcher.CheckAccess())
                    {
                        SetValue(ButtonTextProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(ButtonTextProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata ButtonTextMetaData = new FrameworkPropertyMetadata("Browse"
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(ButtonText_PropertyChanged)
            , new CoerceValueCallback(ButtonText_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register("ButtonText", typeof(string), typeof(BrowseDirectoryPanel), ButtonTextMetaData, new ValidateValueCallback(ButtonText_Validate));

        private static void ButtonText_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object ButtonText_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency ButtonText value is reevaluated. The return value is the
            //latest value set to the dependency ButtonText
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool ButtonText_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion ButtonText Dependency Property

        #region DirectoryExists Dependency Property

        public bool DirectoryExists
        {
            get
            {
                return (bool)GetValue(DirectoryExistsProperty);
            }
            set
            {
                try
                {
                    if (Dispatcher.CheckAccess())
                    {
                        SetValue(DirectoryExistsProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(DirectoryExistsProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata DirectoryExistsMetaData = new FrameworkPropertyMetadata(false
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(DirectoryExists_PropertyChanged)
            , new CoerceValueCallback(DirectoryExists_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty DirectoryExistsProperty =
            DependencyProperty.Register("DirectoryExists", typeof(bool), typeof(BrowseDirectoryPanel), DirectoryExistsMetaData, new ValidateValueCallback(DirectoryExists_Validate));

        private static void DirectoryExists_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object DirectoryExists_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency DirectoryExists value is reevaluated. The return value is the
            //latest value set to the dependency DirectoryExists
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));

            var d = (BrowseDirectoryPanel)dobj;
            var current = Value;
            if (!string.IsNullOrWhiteSpace(d.SelectedDirectory) && Directory.Exists(d.SelectedDirectory)) current = true;
            else current = false;
            return current;
        }

        private static bool DirectoryExists_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion DirectoryExists Dependency Property
        
        #region AddFolderButtonText Dependency Property

        public string AddFolderButtonText
        {
            get
            {
                return (string)GetValue(AddFolderButtonTextProperty);
            }
            set
            {
                try
                {
                    if (Dispatcher.CheckAccess())
                    {
                        SetValue(AddFolderButtonTextProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(AddFolderButtonTextProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata AddFolderButtonTextMetaData = new FrameworkPropertyMetadata("New Folder"
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(AddFolderButtonText_PropertyChanged)
            , new CoerceValueCallback(AddFolderButtonText_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty AddFolderButtonTextProperty =
            DependencyProperty.Register("AddFolderButtonText", typeof(string), typeof(BrowseDirectoryPanel), AddFolderButtonTextMetaData, new ValidateValueCallback(AddFolderButtonText_Validate));

        private static void AddFolderButtonText_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object AddFolderButtonText_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency AddFolderButtonText value is reevaluated. The return value is the
            //latest value set to the dependency AddFolderButtonText
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool AddFolderButtonText_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion AddFolderButtonText Dependency Property

        #region ParentDirectoryExists Dependency Property

        public bool ParentDirectoryExists
        {
            get
            {
                return (bool)GetValue(ParentDirectoryExistsProperty);
            }
            set
            {
                try
                {
                    if (Dispatcher.CheckAccess())
                    {
                        SetValue(ParentDirectoryExistsProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(ParentDirectoryExistsProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata ParentDirectoryExistsMetaData = new FrameworkPropertyMetadata(false
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(ParentDirectoryExists_PropertyChanged)
            , new CoerceValueCallback(ParentDirectoryExists_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty ParentDirectoryExistsProperty =
            DependencyProperty.Register("ParentDirectoryExists", typeof(bool), typeof(BrowseDirectoryPanel), ParentDirectoryExistsMetaData, new ValidateValueCallback(ParentDirectoryExists_Validate));

        private static void ParentDirectoryExists_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object ParentDirectoryExists_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency ParentDirectoryExists value is reevaluated. The return value is the
            //latest value set to the dependency ParentDirectoryExists
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));

            var d = (BrowseDirectoryPanel)dobj;
            var current = Value;
            if (!string.IsNullOrWhiteSpace(d.SelectedDirectory) && new DirectoryInfo(d.SelectedDirectory).Parent.Exists && new DirectoryInfo(d.SelectedDirectory).Exists) current = false;
            else current = true;
            return current;
        }

        private static bool ParentDirectoryExists_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion ParentDirectoryExists Dependency Property

        private void BtnBrowseDirectory_Click(object sender, RoutedEventArgs e)
        {
            var FilePath = string.Empty;
            if (IoDialogUtility.ShowMyPicturesBrowserDialog(out FilePath, SelectedDirectory))
            {
                SelectedDirectory = FilePath;
                //CoerceValue(SelectedDirectoryProperty);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var t = (TextBox) sender;
            var bindingExpression = t.GetBindingExpression(TextBox.TextProperty);
            if (bindingExpression != null)
                bindingExpression.UpdateSource();
        }

        private void BtnCreateDirectory_Click(object sender, RoutedEventArgs e)
        {
            if (!DirectoryExists && !string.IsNullOrWhiteSpace(SelectedDirectory) && ParentDirectoryExists)
            {
                Directory.CreateDirectory(SelectedDirectory);
                CoerceValue(DirectoryExistsProperty);
                CoerceValue(ParentDirectoryExistsProperty);
            }
        }
    }
}