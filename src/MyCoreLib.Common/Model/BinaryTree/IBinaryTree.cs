
namespace MyCoreLib.Common.Model.BinaryTree
{
    public interface IBinaryTree
    {
        bool AddNote(IBinaryTreeNote note);
        bool RemoveNote(int index);
        bool RemoveNote(IBinaryTreeNote note);
    }
}
