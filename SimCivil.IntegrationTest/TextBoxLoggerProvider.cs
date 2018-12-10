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
// SimCivil - SimCivil.IntegrationTest - TextBoxLoggerProvider.cs
// Create Date: 2018/09/28
// Update Date: 2018/09/28

using System;
using System.Text;
using System.Windows.Controls;

using Microsoft.Extensions.Logging;

namespace SimCivil.IntegrationTest
{
    public class TextBoxLoggerProvider : ILoggerProvider
    {
        public TextBox TextBox { get; }

        public TextBoxLoggerProvider(TextBox textBox)
        {
            TextBox = textBox;
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose() { }

        /// <summary>
        /// Creates a new <see cref="T:Microsoft.Extensions.Logging.ILogger" /> instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new TextBoxLogger(TextBox, categoryName);
        }
    }
}