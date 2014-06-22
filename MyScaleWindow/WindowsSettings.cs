using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows;
using System.ComponentModel;

namespace MyScaleWindow.Tools
{
    class WindowsSettings
    {
        #region WindowApplicationSettings Helper Class
        public class WindowApplicationSettings : ApplicationSettingsBase
        {
            private WindowsSettings windowSettings;

#pragma warning disable 618 // "System.Windows.UIElement.PersistId" ist veraltet: "PersistId is an obsolete property and may be removed in a future release.  The value of this property is not defined."
            public WindowApplicationSettings(WindowsSettings windowSettings)
                : base(windowSettings.window.PersistId.ToString())
            {
                this.windowSettings = windowSettings;
            }
#pragma warning restore

            [UserScopedSetting]
            public Rect Location
            {
                get
                {
                    if (this["Location"] != null)
                    {
                        return ((Rect)this["Location"]);
                    }
                    return Rect.Empty;
                }
                set
                {
                    this["Location"] = value;
                }
            }

            [UserScopedSetting]
            public WindowState WindowState
            {
                get
                {
                    if (this["WindowState"] != null)
                    {
                        return (WindowState)this["WindowState"];
                    }
                    return WindowState.Normal;
                }
                set
                {
                    this["WindowState"] = value;
                }
            }

        }
        #endregion

        #region Constructor
        private Window window = null;

        public WindowsSettings(Window window)
        {
            this.window = window;
        }
        #endregion

        #region Attached "Save" Property Implementation
        /// <summary>
        /// Register the "Save" attached property and the "OnSaveInvalidated" callback 
        /// </summary>
        public static readonly DependencyProperty SaveProperty = DependencyProperty.RegisterAttached("Save", typeof(bool), typeof(WindowsSettings), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnSaveInvalidated)));

        public static void SetSave(DependencyObject dependencyObject, bool enabled)
        {
            dependencyObject.SetValue(SaveProperty, enabled);
        }

        /// <summary>
        /// Called when Save is changed on an object.
        /// </summary>
        private static void OnSaveInvalidated(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            Window window = dependencyObject as Window;
            if (window != null)
            {
                if ((bool)e.NewValue)
                {
                    WindowsSettings settings = new WindowsSettings(window);
                    settings.Attach();
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Load the Window Size Location and State from the settings object
        /// </summary>
        protected virtual void LoadWindowState()
        {
            this.Settings.Reload();
            if (this.Settings.Location != Rect.Empty)
            {
                this.window.Left = this.Settings.Location.Left;
                this.window.Top = this.Settings.Location.Top;
                this.window.Width = this.Settings.Location.Width;
                this.window.Height = this.Settings.Location.Height;
            }

            if (this.Settings.WindowState != WindowState.Maximized)
            {
                this.window.WindowState = this.Settings.WindowState;
            }
        }


        /// <summary>
        /// Save the Window Size, Location and State to the settings object
        /// </summary>
        protected virtual void SaveWindowState()
        {
            this.Settings.WindowState = this.window.WindowState;
            this.Settings.Location = this.window.RestoreBounds;
            this.Settings.Save();
        }
        #endregion

        #region Private Methods
        private void Attach()
        {
            if (this.window != null)
            {
                this.window.Closing += new CancelEventHandler(window_Closing);
                this.window.Initialized += new EventHandler(window_Initialized);
                this.window.Loaded += new RoutedEventHandler(window_Loaded);
            }
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Settings.WindowState == WindowState.Maximized)
            {
                this.window.WindowState = this.Settings.WindowState;
            }
        }

        private void window_Initialized(object sender, EventArgs e)
        {
            LoadWindowState();
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            SaveWindowState();
        }
        #endregion

        #region Settings Property Implementation
        private WindowApplicationSettings windowApplicationSettings = null;

        protected virtual WindowApplicationSettings CreateWindowApplicationSettingsInstance()
        {
            return new WindowApplicationSettings(this);
        }

        [Browsable(false)]
        public WindowApplicationSettings Settings
        {
            get
            {
                if (windowApplicationSettings == null)
                {
                    this.windowApplicationSettings = CreateWindowApplicationSettingsInstance();
                }
                return this.windowApplicationSettings;
            }
        }
        #endregion
    }

    class Tools
    {
        /// <summary>
        /// Beep
        /// </summary>
        static public void Beep()
        {
            Console.Beep();
        }

        /// <summary>
        /// Dec2Bin
        /// </summary>
        /// <param name="dezimalzahl"></param>
        /// <param name="bitanzahl"></param>
        /// <returns></returns>
        public static string Dec2Bin(long dezimalzahl, int bitanzahl)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bitanzahl; dezimalzahl = dezimalzahl >> 1, i++)
                sb.Insert(0, dezimalzahl & 1);
            return sb.ToString();
        }

        /// <summary>
        /// BinaryStringToUInt32
        /// </summary>
        /// <param name="binString"></param>
        /// <returns></returns>
        public static UInt32 BinaryStringToUInt32(string binString)
        {
            int Stringlänge = binString.Length;
            UInt32 Rückgabe = 0;
            if (!System.Text.RegularExpressions.Regex.IsMatch(binString, "[01]{" + Stringlänge + "}") || Stringlänge > 31)
                throw new Exception("Ungültige Zeichenfolge");
            else
                for (int i = 0; i < Stringlänge; i++)
                    if (binString[i] == '1')
                        Rückgabe += (UInt32)Math.Pow(2, Stringlänge - 1 - i);
            return Rückgabe;
        }
    }
}
