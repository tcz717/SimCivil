// Copyright (c) 2017 TPDT
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// SimCivil - SimCivil.IntegrationTest - LogViewer.xaml.cs
// Create Date: 2019/05/05
// Update Date: 2019/06/19

using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Extensions.Logging;

namespace SimCivil.IntegrationTest
{
    /// <summary>
    /// LogViewer.xaml 的交互逻辑
    /// </summary>
    public partial class LogViewer : UserControl
    {
        public WpfLogCollection LogCollection => (WpfLogCollection) DataContext;

        public LogViewer()
        {
            InitializeComponent();
        }

        private void LogViewer_OnLoaded(object sender, RoutedEventArgs e) { }

        private void ScrollViewer_OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer) sender;
            if (Math.Abs(e.ExtentHeightChange)                              > 0.01 &&
                scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset < 10)
                scrollViewer.ScrollToEnd();
        }
    }

    public static class WpfLogExtension
    {
        public static ILoggingBuilder AddLogViewer(this ILoggingBuilder builder, LogViewer logViewer)
            => builder.AddProvider(new WpfLogProvider(logViewer));
    }

    public class WpfLogProvider : ILoggerProvider
    {
        public LogViewer LogViewer { get; }

        public WpfLogProvider(LogViewer logViewer)
        {
            LogViewer = logViewer;
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose() { }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.Logging.ILogger" />.</returns>
        public ILogger CreateLogger(string categoryName) => new WpfLogger(LogViewer, categoryName);
    }

    public class WpfLogCollection : ObservableCollection<WpfLogEntry> { }

    public class WpfLogEntry
    {
        public LogLevel  LogLevel  { get; set; }
        public Exception Exception { get; set; }
        public string    Category  { get; set; }
        public string    Content   { get; }
        public DateTime  TimeStamp { get; set; }

        /// <summary>
        ///   初始化 <see cref="T:System.Object" /> 类的新实例。
        /// </summary>
        public WpfLogEntry(LogLevel logLevel, string category, string content)
        {
            LogLevel  = logLevel;
            Category  = category;
            Content   = content;
            TimeStamp = DateTime.Now;
        }
    }

    public class WpfLogger : ILogger
    {
        public LogViewer LogViewer { get; }
        public string    Category  { get; }

        public WpfLogger(LogViewer logViewer, string category)
        {
            LogViewer = logViewer;
            Category  = category;
        }

        /// <summary>Writes a log entry.</summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public void Log<TState>(
            LogLevel                        logLevel,
            EventId                         eventId,
            TState                          state,
            Exception                       exception,
            Func<TState, Exception, string> formatter)
        {
            LogViewer.Dispatcher.InvokeAsync(
                () => LogViewer.LogCollection.Add(
                    new WpfLogEntry(logLevel, Category, state.ToString() + exception)));
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns><c>true</c> if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        /// <summary>Begins a logical operation scope.</summary>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable BeginScope<TState>(TState state) => null;
    }
}