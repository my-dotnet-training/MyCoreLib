using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreLib.Common.Model
{
    public class BinaryTreeNode
    {
        private object _data;
        private BinaryTreeNode _left;
        private BinaryTreeNode _right;
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }
        public BinaryTreeNode Left
        {
            get { return _left; }
            set { _left = value; }
        }
        public BinaryTreeNode Right
        {
            get { return _right; }
            set { _right = value; }
        }
        public BinaryTreeNode(object data)
        {
            this._data = data;
        }
        public override string ToString()
        {
            return _data.ToString();
        }
    }
}
