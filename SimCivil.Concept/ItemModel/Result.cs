using System;
using System.Collections.Generic;
using System.Text;

namespace SimCivil.Concept.ItemModel
{
    public enum ErrorCode
    {
        Success = 0,
        ItemNotFound,
        InvalidOperation,
    }

    public struct Result
    {
        public ErrorCode Err;
        public string ErrMsg;
    }

    public struct Result<T>
    {
        public ErrorCode Err;
        public string ErrMsg;
        public T value;
    }
}
