using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Business.Common.System.Processes;
using MahApps.Metro.Controls;
using Simple.ImageResizer;

namespace SocialImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Class Initialization

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion Class Initialization

        #region Dependency Properties

        #region ProgressLabel Dependency Property

        public String ProgressLabel
        {
            get
            {
                return (String)GetValue(ProgressLabelProperty);
            }
            set
            {
                try
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        SetValue(ProgressLabelProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(ProgressLabelProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata ProgressLabelMetaData = new FrameworkPropertyMetadata("Executing..."
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(ProgressLabel_PropertyChanged)
            , new CoerceValueCallback(ProgressLabel_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty ProgressLabelProperty =
            DependencyProperty.Register("ProgressLabel", typeof(String), typeof(MainWindow), ProgressLabelMetaData, new ValidateValueCallback(ProgressLabel_Validate));

        private static void ProgressLabel_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object ProgressLabel_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency ProgressLabel value is reevaluated. The return value is the
            //latest value set to the dependency ProgressLabel
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool ProgressLabel_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion ProgressLabel Dependency Property

        #region IsProcessRunning Dependency Property

        public Boolean IsProcessRunning
        {
            get
            {
                return (Boolean)GetValue(IsProcessRunningProperty);
            }
            set
            {
                try
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        SetValue(IsProcessRunningProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(IsProcessRunningProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata IsProcessRunningMetaData = new FrameworkPropertyMetadata(false
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(IsProcessRunning_PropertyChanged)
            , new CoerceValueCallback(IsProcessRunning_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty IsProcessRunningProperty =
            DependencyProperty.Register("IsProcessRunning", typeof(Boolean), typeof(MainWindow), IsProcessRunningMetaData, new ValidateValueCallback(IsProcessRunning_Validate));

        private static void IsProcessRunning_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object IsProcessRunning_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency IsProcessRunning value is reevaluated. The return value is the
            //latest value set to the dependency IsProcessRunning
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool IsProcessRunning_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion IsProcessRunning Dependency Property

        #region SourceDirectory Dependency Property

        public String SourceDirectory
        {
            get
            {
                return (String)GetValue(SourceDirectoryProperty);
            }
            set
            {
                try
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        SetValue(SourceDirectoryProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(SourceDirectoryProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata SourceDirectoryMetaData = new FrameworkPropertyMetadata(""
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(SourceDirectory_PropertyChanged)
            , new CoerceValueCallback(SourceDirectory_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty SourceDirectoryProperty =
            DependencyProperty.Register("SourceDirectory", typeof(String), typeof(MainWindow), SourceDirectoryMetaData, new ValidateValueCallback(SourceDirectory_Validate));

        private static void SourceDirectory_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object SourceDirectory_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency SourceDirectory value is reevaluated. The return value is the
            //latest value set to the dependency SourceDirectory
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool SourceDirectory_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion SourceDirectory Dependency Property

        #region TargetDirectory Dependency Property

        public String TargetDirectory
        {
            get
            {
                return (String)GetValue(TargetDirectoryProperty);
            }
            set
            {
                try
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        SetValue(TargetDirectoryProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(TargetDirectoryProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata TargetDirectoryMetaData = new FrameworkPropertyMetadata(""
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(TargetDirectory_PropertyChanged)
            , new CoerceValueCallback(TargetDirectory_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty TargetDirectoryProperty =
            DependencyProperty.Register("TargetDirectory", typeof(String), typeof(MainWindow), TargetDirectoryMetaData, new ValidateValueCallback(TargetDirectory_Validate));

        private static void TargetDirectory_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object TargetDirectory_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency TargetDirectory value is reevaluated. The return value is the
            //latest value set to the dependency TargetDirectory
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool TargetDirectory_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion TargetDirectory Dependency Property

        #region PercentComplete Dependency Property

        public Double PercentComplete
        {
            get
            {
                return (Double)GetValue(PercentCompleteProperty);
            }
            set
            {
                try
                {
                    if (this.Dispatcher.CheckAccess())
                    {
                        SetValue(PercentCompleteProperty, value);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Send, (SendOrPostCallback)delegate
                        {
                            SetValue(PercentCompleteProperty, value);
                        }, null);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private static FrameworkPropertyMetadata PercentCompleteMetaData = new FrameworkPropertyMetadata(0D
            , FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal
            , new PropertyChangedCallback(PercentComplete_PropertyChanged)
            , new CoerceValueCallback(PercentComplete_CoerceValue)
            , false
            , System.Windows.Data.UpdateSourceTrigger.PropertyChanged);

        public static readonly DependencyProperty PercentCompleteProperty =
            DependencyProperty.Register("PercentComplete", typeof(Double), typeof(MainWindow), PercentCompleteMetaData, new ValidateValueCallback(PercentComplete_Validate));

        private static void PercentComplete_PropertyChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            //To be called whenever the DP is changed.
            //MessageBox.Show(string.Format("Property changed is fired : OldValue {0} NewValue : {1}", e.OldValue, e.NewValue));
        }

        private static object PercentComplete_CoerceValue(DependencyObject dobj, object Value)
        {
            //called whenever dependency PercentComplete value is reevaluated. The return value is the
            //latest value set to the dependency PercentComplete
            //MessageBox.Show(string.Format("CoerceValue is fired : Value {0}", Value));
            return Value;
        }

        private static bool PercentComplete_Validate(object Value)
        {
            //Custom validation block which takes in the value of DP
            //Returns true / false based on success / failure of the validation
            //MessageBox.Show(string.Format("DataValidation is Fired : Value {0}", Value));
            return true;
        }

        #endregion PercentComplete Dependency Property

        #endregion Dependency Properties

        #region Fields and Properties

        private string _sourceDirectoryPath;

        private string _targetDirectoryPath;

        private string _sourceExtension;

        private string _targetExtension;

        private bool _isCancelling;

        #endregion Fields and Properties

        //private void btnOpenTargetDirectory_Click(object sender, RoutedEventArgs e)
        //{
        //    var FilePath = String.Empty;
        //    if (IoDialogUtility.ShowFolderBrowserDialog(out FilePath, null, Environment.SpecialFolder.MyPictures))
        //    {
        //        TargetDirectory = FilePath;
        //    }
        //}

        private void btnGeneratePhotos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sourceDirectory = SourceBrowser.SelectedDirectory;
                var targetDirectory = TargetBrowser.SelectedDirectory;
                if (!Directory.Exists(sourceDirectory) || !Directory.Exists(targetDirectory))
                {
                    SourceBrowser.SelectedDirectory = sourceDirectory;
                    TargetBrowser.SelectedDirectory = targetDirectory;
                    MessageBox.Show("The source or target directory no longer exists!");
                    return;
                }
                var sizeString = GetFileSize();
                //var imageEncoding = GetImageQuality();
                var imageQuality = GetImageQuality();
                var renameToDate = ChkRenameToDate.IsChecked.HasValue && ChkRenameToDate.IsChecked.Value;

                SetStatusBarRunningOn();
                new Thread(() =>
                {
                    try
                    {
                        GenerateOutput(sourceDirectory, targetDirectory, sizeString, imageQuality, renameToDate);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        SetStatusBarRunningOff();
                    }
                    SetStatusBarRunningOff();
                }) { IsBackground = true }.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPreviewPhotos_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnAutoNameDirectory_Click(object sender, RoutedEventArgs e)
        {
            if (SourceBrowser.DirectoryExists)
            {
                TargetBrowser.SelectedDirectory = System.IO.Path.Combine(SourceBrowser.SelectedDirectory, OpenSocialSiteCheckbox.Text);
            }
        }

        private void GenerateOutput(String sourceDirectory, String targetDirectory, String sizeString, ImageEncoding imageEncoding, Boolean renameToDate)
        {
            var files = Directory.EnumerateFiles(sourceDirectory, "*.jpg", SearchOption.TopDirectoryOnly).ToList();
            //var sizeString = GetFileSize();

            var queueSize = Int32.Parse(sizeString) > 1024 ? 1: 4;
            var fileCount = (Double)files.Count;
            var currentCount = 0D;
            //var percentDone = 0D;

            var tasks = new Queue<Task>();
            foreach (var file in files)
            {
                //var targetDirectory = TargetBrowser.SelectedDirectory;
                Task t = Task.Run(() =>
                {
                    var inputFile = file;
                    var inputFileInfo = new FileInfo(inputFile);
                    var inputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile.ToUpperInvariant());
                    var extension = Path.GetExtension(inputFile);
                    var creationTime = inputFileInfo.CreationTime;
                    var outputFile = String.Empty;
                    if (renameToDate)
                    {
                        outputFile = Path.Combine(targetDirectory, String.Format(@"{0}{1}.{2}.{3}px_wide{4}", creationTime.ToString(@"yyyy-MM-dd_HHmmss"), creationTime.Millisecond.ToString("0000"), inputFileNameWithoutExtension, sizeString, extension));
                    }
                    else
                    {
                        outputFile = Path.Combine(targetDirectory, String.Format(@"{0}.{1}px_wide{2}", inputFileNameWithoutExtension, sizeString, extension));
                    }
                    if (!File.Exists(outputFile))
                    {
                        using (var resizer = new ImageResizer(inputFile))
                        {
                            File.WriteAllBytes(outputFile, resizer.Resize(Int32.Parse(sizeString), imageEncoding));
                        }
                        currentCount++;
                        SetPercentComplete(currentCount / fileCount);
                        SetProgressLabel(String.Format(@"{0} of {1} Complete", (int)currentCount, (int)fileCount));
                    }
                });
                tasks.Enqueue(t);

                if (tasks.Count == queueSize)
                {
                    for (int i = 0; i < queueSize; i++)
                    {
                        Task.WaitAll(tasks.ToArray());
                        tasks.Clear();
                    }
                }
            }
            if (tasks.Count > 0) Task.WaitAll(tasks.ToArray());
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
            {
                if (TargetBrowser.DirectoryExists && ChkOpenWhenDone.IsChecked.HasValue && ChkOpenWhenDone.IsChecked.Value)
                {
                    TargetBrowser.SelectedDirectory.OpenFileWithDefaultApplication();
                }
            }, null);
        }

        //private void GenerateOutput(String sourceDirectory, String targetDirectory, String sizeString, int imageQuality, Boolean renameToDate)
        //{
        //    var files = Directory.EnumerateFiles(sourceDirectory, "*.jpg", SearchOption.TopDirectoryOnly).ToList();
        //    //var sizeString = GetFileSize();

        //    var queueSize = 4;
        //    var fileCount = (Double)files.Count;
        //    var currentCount = 0D;
        //    //var percentDone = 0D;

        //    var tasks = new Queue<Task>();
        //    foreach (var file in files)
        //    {
        //        //var targetDirectory = TargetBrowser.SelectedDirectory;
        //        Task t = Task.Run(() =>
        //        {
        //            var inputFile = file;
        //            var inputFileInfo = new FileInfo(inputFile);
        //            var inputFileNameWithoutExtension = Path.GetFileNameWithoutExtension(inputFile.ToUpperInvariant());
        //            var extension = Path.GetExtension(inputFile);
        //            var creationTime = inputFileInfo.CreationTime;
        //            var outputFile = String.Empty;
        //            if (renameToDate)
        //            {
        //                outputFile = Path.Combine(targetDirectory, String.Format(@"{0}{1}.{2}.{3}px_wide{4}", creationTime.ToString(@"yyyy-MM-dd_HHmmss"), creationTime.Millisecond.ToString("0000"), inputFileNameWithoutExtension, sizeString, extension));
        //            }
        //            else
        //            {
        //                outputFile = Path.Combine(targetDirectory, String.Format(@"{0}.{1}px_wide{2}", inputFileNameWithoutExtension, sizeString, extension));
        //            }
        //            if (!File.Exists(outputFile))
        //            {
        //                //var resizer = new ImageResizer(inputFile);
        //                //File.WriteAllBytes(outputFile, resizer.Resize(Int32.Parse(sizeString), imageEncoding));
        //                var handler = new ImageHandler();
        //                handler.Save(inputFile, Int32.Parse(sizeString), Int32.Parse(sizeString), imageQuality, outputFile);
        //                currentCount++;
        //                SetPercentComplete(currentCount / fileCount);
        //                SetProgressLabel(String.Format(@"{0} of {1} Complete", (int)currentCount, (int)fileCount));
        //            }
        //        });
        //        tasks.Enqueue(t);

        //        if (tasks.Count == queueSize)
        //        {
        //            for (int i = 0; i < queueSize; i++)
        //            {
        //                Task.WaitAll(tasks.ToArray());
        //                tasks.Clear();
        //            }
        //        }
        //    }
        //    if (tasks.Count > 0) Task.WaitAll(tasks.ToArray());
        //    Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
        //    {
        //        if (TargetBrowser.DirectoryExists && ChkOpenWhenDone.IsChecked.HasValue && ChkOpenWhenDone.IsChecked.Value)
        //        {
        //            TargetBrowser.SelectedDirectory.OpenFileWithDefaultApplication();
        //        }
        //    }, null);
        //}

        private String GetFileSize()
        {
            foreach (var child in this.StackFileSizes.Children)
            {
                if (child.GetType() == typeof(RadioButton))
                {
                    var radio = (RadioButton)child;
                    if (radio.IsChecked.HasValue && radio.IsChecked.Value)
                    {
                        return radio.Tag as String;
                    }
                }
            }
            return "1024";
        }

        private ImageEncoding GetImageQuality()
        {
            foreach (var child in this.ImageQualityStackPanel.Children)
            {
                if (child.GetType() == typeof(RadioButton))
                {
                    var radio = (RadioButton)child;
                    if (radio.IsChecked.HasValue && radio.IsChecked.Value)
                    {
                        switch (radio.Tag as String)
                        {
                            case "90":
                                return ImageEncoding.Jpg90;
                            case "95":
                                return ImageEncoding.Jpg90;
                            case "100":
                                return ImageEncoding.Jpg90;
                            default:
                                return ImageEncoding.Jpg;
                        }
                    }
                }
            }
            return ImageEncoding.Jpg90;
        }

        //private int GetImageQuality()
        //{
        //    foreach (var child in this.ImageQualityStackPanel.Children)
        //    {
        //        if (child.GetType() == typeof(RadioButton))
        //        {
        //            var radio = (RadioButton)child;
        //            if (radio.IsChecked.HasValue && radio.IsChecked.Value)
        //            {
        //                switch (radio.Tag as String)
        //                {
        //                    case "90":
        //                        return 90;
        //                    case "95":
        //                        return 95;
        //                    case "100":
        //                        return 100;
        //                    default:
        //                        return 100;
        //                }
        //            }
        //        }
        //    }
        //    return 100;
        //}

        #region Set StatusBar methods

        public void SetStatusBarRunningOn(String LabelContent = "Running...")
        {
            try
            {
                if (this.Dispatcher.CheckAccess())
                {
                    MainGrid.IsEnabled = false;
                    ProgressLabel = LabelContent;
                    CoerceValue(ProgressLabelProperty);
                    IsProcessRunning = true;
                }
                else
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        MainGrid.IsEnabled = false;
                        ProgressLabel = LabelContent;
                        CoerceValue(ProgressLabelProperty);
                        IsProcessRunning = true;
                    }, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetStatusBarRunningOff()
        {
            try
            {
                if (this.Dispatcher.CheckAccess())
                {
                    MainGrid.IsEnabled = true;
                    ProgressLabel = String.Empty;
                    IsProcessRunning = false;
                }
                else
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        MainGrid.IsEnabled = true;
                        ProgressLabel = String.Empty;
                        IsProcessRunning = false;
                    }, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetProgressLabel(String LabelContent)
        {
            try
            {
                if (this.Dispatcher.CheckAccess())
                {
                    ProgressLabel = LabelContent;
                }
                else
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        ProgressLabel = LabelContent;
                    }, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SetPercentComplete(Double value)
        {
            try
            {
                if (this.Dispatcher.CheckAccess())
                {
                    PercentComplete = value * 100;
                    CoerceValue(PercentCompleteProperty);
                }
                else
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Background, (SendOrPostCallback)delegate
                    {
                        PercentComplete = value * 100;
                        CoerceValue(PercentCompleteProperty);
                    }, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion Set StatusBar methods

        private void BtnOpenSocialSite_Click(object sender, RoutedEventArgs e)
        {
            var selectedValue = OpenSocialSiteCheckbox.Text;
            switch (selectedValue)
            {
                case "Facebook":
                    ProcessExt.OpenFileWithDefaultApplication("http://www.facebook.com");
                    break;
                case "Twitter":
                    ProcessExt.OpenFileWithDefaultApplication("http://www.twitter.com");
                    break;
                case "Shutterfly":
                    ProcessExt.OpenFileWithDefaultApplication("http://www.shutterfly.com");
                    break;
            }
        }
    }
}