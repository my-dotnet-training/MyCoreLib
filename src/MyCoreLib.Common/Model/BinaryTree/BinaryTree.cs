using System;
using System.Collections.Generic;

namespace MyCoreLib.Common.Model.BinaryTree
{
    public class BinaryTree<T> : BaseClassIndexable<T>, IBinaryTree
    {
        List<IBinaryTreeNote> NoteList { get; set; }

        public bool AddNote(IBinaryTreeNote note)
        {
            throw new NotImplementedException();
        }
        public bool RemoveNote(int index)
        {
            throw new NotImplementedException();
        }
        public bool RemoveNote(IBinaryTreeNote note)
        {
            throw new NotImplementedException();
        }
        public override T CallByIndex(int index)
        {
            var _note = NoteList.Find(e => { return e.Index == index; });
            return _note == null ? default(T) : (T)_note.Value;
        }
        public override void SetByIndex(int index, T value)
        {
            throw new NotImplementedException();
        }

        public override T CallByKey(string key)
        {
            throw new NotImplementedException();
        }
        public override void SetByKey(string key, T value)
        {
            throw new NotImplementedException();
        }
    }
}
