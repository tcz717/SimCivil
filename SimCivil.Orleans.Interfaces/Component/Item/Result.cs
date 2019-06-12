﻿// Copyright (c) 2019 TPDT
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
// SimCivil - SimCivil.Orleans.Interfaces - Result.cs
// Update Date: 2019/6/12

namespace SimCivil.Orleans.Interfaces.Component
{
    public enum ErrorCode
    {
        Success = 0,
        ItemNotFound,
        InvalidOperation,
    }

    public interface IResult
    {
        ErrorCode Err { get; }

        string ErrMsg { get; }
    }



    public struct Result : IResult
    {
        public ErrorCode Err { get; set; }

        public string ErrMsg { get; set; }
    }

    public struct Result<T>
    {
        public ErrorCode Err { get; set; }

        public string ErrMsg { get; set; }

        public T Value { get; set; }
    }
}
