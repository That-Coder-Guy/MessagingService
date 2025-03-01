using Client.ViewModels;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using Utilities.Enumerations;
using Utilities.EventArguments;
using Utilities.ViewModels;
using WebSocketCommunication;
using WpfScreenHelper;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields
        private List<Control> _pages;

        private ClientWebSocket _connection;

        private ApplicationState _state = ApplicationState.Connect;
        #endregion

        #region Methods
        public MainWindow()
        {
            InitializeComponent();

            _pages = [ConnectionPage, LoginPage];

            _connection = new ClientWebSocket("ws://localhost:8080/messager/");
            SetState(ApplicationState.Start);
            _connection.Connect(5000);
        }

        private void OnStateChanged(object? sender, StateChangedEventArgs args)
        {
            SetState(args.NewState);
        }

        private void SetState(ApplicationState state)
        {
            Debug.Print($"Set to state: {state}");
            IViewModel viewModel;
            switch (state)
            {
                case ApplicationState.Start:
                case ApplicationState.Connect:
                    viewModel = new ConnectionPageViewModel(_connection);
                    viewModel.StateChanged += OnStateChanged;
                    ConnectionPage.DataContext = viewModel;
                    SetCurrentPage(ConnectionPage);
                    break;
                case ApplicationState.Login:
                    //viewModel = new ConnectionPageViewModel(_connection);
                    //viewModel.StateChanged += OnStateChanged;
                    //LoginPage.DataContext = viewModel;
                    SetCurrentPage(LoginPage);
                    break;
            }
            _state = state;
        }

        private void SetCurrentPage(Control page)
        {
            Dispatcher.Invoke(() =>
            {
                _pages.ForEach(page => page.Visibility = Visibility.Hidden);
                page.Visibility = Visibility.Visible;
            });

        }

        /// <summary>
        /// Handles mouse press events over the window title bar.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnTitlebarMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Handles mouse move events over the window title bar.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnTitlebarMouseMove(object sender, MouseEventArgs e)
        {
            if (WindowState == WindowState.Maximized && e.LeftButton == MouseButtonState.Pressed)
            {
                WindowState = WindowState.Normal;
                if (sender is Border titlebar)
                {
                    // Pre restore operations
                    Point taskbarRelativePosition = e.GetPosition(titlebar);
                    Point windowRelavitePosition = e.GetPosition(null);
                    Screen screen = Screen.FromPoint(PointToScreen(taskbarRelativePosition));

                    // Restore window to normal size
                    WindowState = WindowState.Normal;

                    // Post restore operations
                    Left = screen.Bounds.Left + windowRelavitePosition.X - RestoreBounds.Width * taskbarRelativePosition.X / SystemParameters.WorkArea.Width;
                    Top = screen.Bounds.Top + windowRelavitePosition.Y - taskbarRelativePosition.Y;
                    DragMove();
                }
            }
        }

        /// <summary>
        /// Handles click events for the window title bar minimize button.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Handles click events for the window title bar maximize button.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnMaximizeClick(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        /// <summary>
        /// Handles click events for the window title bar close button.
        /// </summary>
        /// <param name="sender">The object signaling the event.</param>
        /// <param name="e">Information about the event.</param>
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        #region Native Interop Code
        /// <summary>
        /// MINMAXINFO structure is used by the Windows operating system to define the maximum, minimum,
        /// and default sizes and positions for a window. This structure is part of the interop between WPF
        /// and Windows native APIs for controlling window sizing behaviors.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            /// <summary>
            /// Reserved field. Not used by the application, but must be present to maintain
            /// the correct structure size for Windows API calls.
            /// </summary>
            public POINT ptReserved;

            /// <summary>
            /// Specifies the maximum size of the window when it is maximized. This defines
            /// the width and height the window will occupy when maximized.
            /// </summary>
            public POINT ptMaxSize;

            /// <summary>
            /// Specifies the upper-left corner of the window when it is maximized. This point
            /// determines the position where the maximized window will be placed.
            /// </summary>
            public POINT ptMaxPosition;

            /// <summary>
            /// Specifies the minimum tracking size of the window. This is the smallest size
            /// the window can be resized to when dragging the window edges.
            /// </summary>
            public POINT ptMinTrackSize;

            /// <summary>
            /// Specifies the maximum tracking size of the window. This is the largest size
            /// the window can be resized to when dragging the window edges.
            /// </summary>
            public POINT ptMaxTrackSize;
        }

        /// <summary>
        /// POINT structure defines the x and y coordinates of a point in a two-dimensional
        /// space. This is often used for window positioning and sizing calculations in conjunction
        /// with Windows API functions.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            /// The horizontal coordinate (X-axis) of the point.
            /// </summary>
            public int x;

            /// <summary>
            /// The vertical coordinate (Y-axis) of the point.
            /// </summary>
            public int y;

            /// <summary>
            /// Initializes a new instance of the POINT structure with the specified X and Y coordinates.
            /// </summary>
            /// <param name="x">The horizontal coordinate.</param>
            /// <param name="y">The vertical coordinate.</param>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Called when the window source is initialized. This override is used to hook into
        /// the window's message loop to capture native Windows messages, specifically for handling
        /// the `WM_GETMINMAXINFO` message to control window sizing behavior.
        /// </summary>
        /// <param name="e">Provides data for the event.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr handle = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(handle)?.AddHook(new HwndSourceHook(WindowProc));
        }

        /// <summary>
        /// Processes window messages, specifically handling the `WM_GETMINMAXINFO` message (0x0024),
        /// which allows the application to control the window's minimum, maximum, and default sizes.
        /// </summary>
        /// <param name="windowHandle">A handle to the window receiving the message.</param>
        /// <param name="message">The Windows message being processed (e.g., 0x0024).</param>
        /// <param name="widthParam">Additional message-specific information (width parameter).</param>
        /// <param name="lengthParam">Additional message-specific information (height parameter).</param>
        /// <param name="handled">Indicates whether the message was handled.</param>
        /// <returns>A pointer to the result of the message processing, or IntPtr.Zero if no result is required.</returns>
        private IntPtr WindowProc(IntPtr windowHandle, int message, IntPtr widthParam, IntPtr lengthParam, ref bool handled)
        {
            if (message == 0x0024)  // WM_GETMINMAXINFO
            {
                WmGetMinMaxInfo(windowHandle, lengthParam);
                handled = true;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// Property that calculates the DPI scaling factor for the current window.
        /// This factor accounts for high DPI settings, adjusting window size and layout to ensure
        /// the window scales correctly on monitors with non-default DPI settings.
        /// </summary>
        private double DpiScalingFactor
        {
            get
            {
                PresentationSource source = PresentationSource.FromVisual(this);
                return source != null ? source.CompositionTarget.TransformToDevice.M11 : 1.0;
            }
        }

        /// <summary>
        /// Handles the `WM_GETMINMAXINFO` message to adjust the window's maximized size and position.
        /// This method ensures the window is correctly sized based on the screen's working area and
        /// DPI scaling, preventing content from being cut off when the window is maximized.
        /// </summary>
        /// <param name="windowHandle">A handle to the window receiving the message.</param>
        /// <param name="lengthParam">A pointer to the MINMAXINFO structure to be adjusted.</param>
        private void WmGetMinMaxInfo(IntPtr windowHandle, IntPtr lengthParam)
        {
            // Get the monitor information
            HwndSource source = HwndSource.FromHwnd(windowHandle);

            // Get the MINMAXINFO struct
            if (Marshal.PtrToStructure(lengthParam, typeof(MINMAXINFO)) is MINMAXINFO information)
            {
                // Adjust the maximized size and position to fit the working area of the monitor
                information.ptMaxPosition.x = 0;
                information.ptMaxPosition.y = 0;
                information.ptMaxSize.x = (int)(SystemParameters.WorkArea.Width * DpiScalingFactor);
                information.ptMaxSize.y = (int)(SystemParameters.WorkArea.Height * DpiScalingFactor);
                information.ptMinTrackSize.x = (int)(MinWidth * DpiScalingFactor); // Min width
                information.ptMinTrackSize.y = (int)(MinHeight * DpiScalingFactor); // Min height

                Marshal.StructureToPtr(information, lengthParam, true);
            }
        }
        #endregion
    }
}