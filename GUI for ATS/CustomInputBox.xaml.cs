using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace GUI_for_ATS
{
    /// <summary>
    /// Interaktionslogik für CustomInputBox.xaml
    /// </summary>
    public partial class CustomInputBox : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        public string UserInput { get; private set; } = "";
        public bool IsCanceled { get; private set; } = true;

        public CustomInputBox(string message, string title, string defaultValue = "")
        {
            InitializeComponent();
            Loaded += CustomInputBox_Loaded;
            this.Title = title;
            MessageTextBlock.Text = message;
            InputTextBox.Text = defaultValue;
            InputTextBox.Focus();
            InputTextBox.SelectionStart = InputTextBox.Text.Length;
        }

        void CustomInputBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Remove the system menu(close, minimize, maximize)
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            IntPtr style = GetWindowLongPtr(hWnd, GWL_STYLE);
            SetWindowLongPtr(hWnd, GWL_STYLE, new IntPtr(style.ToInt64() & ~WS_SYSMENU));
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput = InputTextBox.Text;
            IsCanceled = false;
            this.DialogResult = true;
            this.Close();
        }

        private void InputTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OkButton_Click(sender, e);
                e.Handled = true;
            }

            if (e.Key == Key.Escape)
            {
                CancelButton_Click(sender, e);
                e.Handled = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            UserInput = "";
            IsCanceled = true;
            this.DialogResult = false;
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Keyboard.ClearFocus();             
        }
    }
}
