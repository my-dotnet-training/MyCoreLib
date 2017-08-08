namespace MyCoreLib.Common.Model.BinaryTree
{
    public interface IBinaryTreeNote
    {
        int Index { get; set; }
        IBinaryTreeNote Farther { get; set; }
        IBinaryTreeNote ChildRight { get; set; }
        IBinaryTreeNote ChildLeft { get; set; }
    }

    public class BaseBinaryTreeEntity<T> : IBinaryTreeNote
    {
        public int Index { get; set; }
        public T Value { get; set; }
        public IBinaryTreeNote Farther { get; set; }
        public IBinaryTreeNote ChildRight { get; set; }
        public IBinaryTreeNote ChildLeft { get; set; }
    }
}
