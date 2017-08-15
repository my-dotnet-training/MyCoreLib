using System;

namespace MyCoreLib.Common.Model.BinaryTree
{
    public interface IBinaryTreeNote
    {
        int Index { get; set; }
        object Value { get; set; }
        IBinaryTreeNote Farther { get; set; }
        IBinaryTreeNote ChildRight { get; set; }
        IBinaryTreeNote ChildLeft { get; set; }
    }

    public abstract class BaseBinaryTreeNote<T> : IBinaryTreeNote
    {
        public int Index { get; set; }
        public T Data { get { return (T)Value; } }
        public object Value { get; set; }

        public IBinaryTreeNote Farther { get; set; }
        public IBinaryTreeNote ChildRight { get; set; }
        public IBinaryTreeNote ChildLeft { get; set; }
        public BaseBinaryTreeNote(T data)
        {
            Value = data;
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class DefaultBinaryTreeNote : BaseBinaryTreeNote<object>
    {
        public DefaultBinaryTreeNote(object data)
            : base(data)
        {
        }
    }
}
