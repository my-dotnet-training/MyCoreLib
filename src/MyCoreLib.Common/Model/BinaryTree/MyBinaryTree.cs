using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCoreLib.Common.Model.BinaryTree
{

    public class MyBinaryTree
    {
        private IBinaryTreeNote _head;
        private string cStr;
        public IBinaryTreeNote Head
        {
            get { return _head; }
            set { _head = value; }
        }
        public MyBinaryTree()
        {
            _head = null;
        }
        /// <summary>
        /// 默认的是以层序序列构造二叉树的
        /// </summary>
        /// <param name="constructStr"></param>
        public MyBinaryTree(string constructStr)
        {
            cStr = constructStr;
            if (cStr[0] == '#')
            {
                _head = null;
                return;//根节点为空，则树无法建立
            }
            _head = new DefaultBinaryTreeNote(cStr[0]);
            Add(_head, 0);
        }
        /// <summary>
        /// 按照给定层序序列给二叉树添加节点
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        private void Add(IBinaryTreeNote parent, int index)
        {
            int leftIndex = 2 * index + 1;
            if (leftIndex < cStr.Length)
            {
                if (cStr[leftIndex] != '#')
                {
                    parent.ChildLeft = new DefaultBinaryTreeNote(cStr[leftIndex]);
                    Add(parent.ChildLeft, leftIndex);
                }
            }
            int rightIndex = 2 * index + 2;
            if (rightIndex < cStr.Length)
            {
                if (cStr[rightIndex] != '#')
                {
                    parent.ChildRight = new DefaultBinaryTreeNote(cStr[rightIndex]);
                    Add(parent.ChildRight, rightIndex);
                }
            }
        }
        /// <summary>
        /// 递归先序遍历
        /// </summary>
        /// <param name="node"></param>
        public void PreOrder(IBinaryTreeNote node)
        {
            if (node != null)
            {
                Console.Write(node);
                PreOrder(node.ChildLeft);
                PreOrder(node.ChildRight);
            }
        }
        /// <summary>
        /// 递归中序遍历
        /// </summary>
        /// <param name="node"></param>
        public void InOrder(IBinaryTreeNote node)
        {
            if (node != null)
            {
                InOrder(node.ChildLeft);
                Console.Write(node);
                InOrder(node.ChildRight);
            }
        }
        /// <summary>
        /// 递归后序遍历
        /// </summary>
        /// <param name="node"></param>
        public void AfterOrder(IBinaryTreeNote node)
        {
            if (node != null)
            {
                AfterOrder(node.ChildLeft);
                AfterOrder(node.ChildRight);
                Console.Write(node);
            }
        }
        /*先序非递归遍历树的原则：
         * 1、先访问当前节点的(node)所有左孩子
         *  在访问某个节点操作完成后立即将该节点入栈。
         * 2、当前访问完一个节点的所有左孩子后，进行出栈操作，
         *  如果出栈节点没有右孩子，则继续出栈，操作。否则按第
         *  1步访问该出栈节点的右孩子节点。
         */
        /// <summary>
        /// 先序遍历的非递归实现
        /// </summary>
        public void PreStackOrder()
        {
            IBinaryTreeNote node = _head;
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            while (node != null || stack.Count > 0)
            {
                while (node != null)//第1步
                {
                    Console.Write(node);//先序的本质就是第一次遇到节点时候访问它
                    stack.Push(node);
                    node = node.ChildLeft;
                }
                if (stack.Count > 0)//第2步
                {
                    node = stack.Pop();
                    node = node.ChildRight;//如果出栈节点没有右孩子的话则继续出栈操作

                }
            }

        }
        /// <summary>
        /// 中序遍历的非递归实现
        /// </summary>
        public void InStackOrder()
        {
            IBinaryTreeNote node = _head;
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.ChildLeft;
                }
                if (stack.Count > 0)
                {
                    node = stack.Pop();
                    Console.Write(node);//中序遍历的本质就是在第二次遇到节点时候访问它
                    node = node.ChildRight;
                }
            }

        }

        /*
         * 后序遍历的非递归实现(比较复杂)。后序遍历的本质就是在第三次遇到节点的时候访问它，
         * 但是向上面那种方式只能在进栈的时候遇到一次和出栈的时候遇到一次
         */       
        /// <summary>
        /// 方法一：利用两个栈。
        /// 1、按照左节孩子的方向将节点和当前节点的右孩子同时入栈，直到没有左孩子为止
        /// 2、两个栈同时出栈，若右栈出栈的是null则访问左栈出栈的节点,
        ///    如果不为null，则左边出栈的退回左栈，右栈入栈个null
        /// 3、对右出栈的右孩子节点重复第1和第2步操作知道两个栈都为空
        /// </summary>
        public void AfterStackOrder()
        {
            Stack<IBinaryTreeNote> lstack = new Stack<IBinaryTreeNote>();//用于存放父节点
            Stack<IBinaryTreeNote> rstack = new Stack<IBinaryTreeNote>();//用于存放右孩子
            IBinaryTreeNote node = _head, right;//right用于存放右栈出栈的节点
            do
            {
                while (node != null)//当node不为空的时候将父节点和右孩子同时入栈
                {
                    right = node.ChildRight;
                    lstack.Push(node);
                    rstack.Push(right);
                    node = node.ChildLeft;//沿着左孩子的方向继续循环
                }
                node = lstack.Pop();//父节点和右孩子同时出栈
                right = rstack.Pop();
                if (right == null)//如果右栈出栈的元素为空则访问左边出栈的元素
                {
                    Console.Write(node);
                }
                else
                {
                    lstack.Push(node);//左边出栈的元素退回栈
                    rstack.Push(null);//右栈补充一个空元素
                }
                node = right;//如果右边出栈的部位空则以上面的规则访问这个右孩子节点
            }
            while (rstack.Count > 0 || rstack.Count > 0);//当左栈或右栈不空的时候继续循环。
        }

        /// <summary>
        /// 方法二：只利用一个栈，利用规律：任何节点如果存在做孩子，那么在后续遍历中，性能更优的单栈非递归算法
        /// 必然紧跟在它的右孩子后面。
        /// 规则：
        /// 1、同上沿着左孩子依次入栈。
        /// 2、如果之前出栈节点是栈顶元素右边孩子或为空则出栈，即访问该节点。
        /// 3、不满足以上条件则将栈顶元素的右孩子入栈
        /// </summary>
        public void AfterStackOrder2()
        {
            IBinaryTreeNote node = _head, pre = _head;
            //pre指针指向“之前出栈节点”，如果为null有问题，这里指向头节点,因为后续遍历中头节点肯定是最后被访问的。
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.ChildLeft;
                }
                if (stack.Count > 0)
                {
                    IBinaryTreeNote temp = stack.Peek().ChildRight;//获取栈顶元素的右孩子，C#的栈因为有了这个方法使得操作简单
                    if (temp == null || temp == pre)//满足规则1
                    {
                        node = stack.Pop();//出栈进行访问
                        Console.Write(node);
                        pre = node;//设置“之前出栈节点”
                        node = null;//防止null再次入栈
                    }
                    else
                    {
                        node = temp;//规则2 继续循环。将栈顶节点的右孩子入栈，重复规则1的操作
                    }
                }
            }

        }
        /// <summary>
        /// 方法三：在特定情况下性能优于方法二
        /// 这种方法是在前面的非递归先序遍历算法的基础上进行修改然后的到后续遍历的逆序。
        /// 这种算法在时间复杂度上与前面两种算法相同，但是空间复杂度上较差，但它在某些
        /// 情况下却是最优秀的：
        /// 1、只是为了得到二叉树的后续遍历序列，那么最后的逆序的操作就可以省掉
        /// </summary>
        public void AfterStackOrder3()
        {
            IBinaryTreeNote node = _head;
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            Stack<IBinaryTreeNote> st = new Stack<IBinaryTreeNote>();//辅助栈将的到的逆序变为正的
            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    st.Push(node);
                    stack.Push(node);
                    node = node.ChildRight;//在先序遍历非递归算法中这里是node.Left及沿着右孩子放学压入栈
                }
                if (stack.Count > 0)
                {
                    node = stack.Pop();
                    node = node.ChildLeft;//在先序遍历非递归算法中这里是node.Right

                }
            }
            while (st.Count > 0)
                Console.Write(st.Pop());
        }
        /// <summary>
        /// 广度优先遍历
        /// </summary>
        public void LevelOrder()
        {
            IBinaryTreeNote node = _head;
            Queue<IBinaryTreeNote> queue = new Queue<IBinaryTreeNote>();
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                node = queue.Dequeue();
                Console.Write(node);
                if (node.ChildLeft != null)
                {
                    queue.Enqueue(node.ChildLeft);

                }
                if (node.ChildRight != null)
                {
                    queue.Enqueue(node.ChildRight);
                }
            }
        }
        
        /// <summary>
        /// 二叉树遍历的应用
        /// 1、计算叶子节点的个数(先序遍历)
        /// 度为2的节点和度为0的节点也就是叶子节点的关系是n0=n2+1;加上可以统计节点的个数所以就可以
        /// 分别统计度为0、度为1和度为2的节点数了。
        /// </summary>
        /// <param name="node"></param>
        /// <param name="count"></param>
        public void CountLeaf(IBinaryTreeNote node, ref int count)
        {
            if (node != null)
            {
                if ((node.ChildLeft == null) && (node.ChildRight == null))
                    count++;
                CountLeaf(node.ChildLeft, ref count);
                CountLeaf(node.ChildRight, ref count);
            }
        }
        /// <summary>
        /// 二叉树遍历的应用
        /// 计算节点数
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Count(IBinaryTreeNote root)
        {
            if (root == null) return 0;
            return Count(root.ChildLeft) + Count(root.ChildRight) + 1;
        }
        /// <summary>
        /// 二叉树遍历的应用
        /// 2、计算树的高度（后序遍历）
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public int Height(IBinaryTreeNote root)
        {
            int a, b;
            if (root == null) return 0;
            a = Height(root.ChildLeft);
            b = Height(root.ChildRight);
            if (a > b) return a + 1; else return b + 1;
        }
        /// <summary>
        /// 二叉树遍历的应用
        /// 3、复制二叉树（后序遍历）
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public IBinaryTreeNote CopyTree(IBinaryTreeNote root)
        {
            IBinaryTreeNote newroot;
            if (root == null)
            {
                newroot = null;
            }
            else
            {
                CopyTree(root.ChildLeft);
                CopyTree(root.ChildRight);
                newroot = root;
            }
            return newroot;
        }
        /// <summary>
        /// 二叉树遍历的应用
        /// 4、建立二叉树饿存储结构（建立二叉树的二叉链表）。上面的复制也是种建立方法
        /// (1)按给定先序序列建立二叉树
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MyBinaryTree CreateByPre(string s)
        {
            MyBinaryTree tree = new MyBinaryTree(s);//先以层序序列初始化个树，再调整
            int _count = 0;
            IBinaryTreeNote node = tree.Head;
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    node.Value = s[_count++];
                    stack.Push(node);
                    node = node.ChildLeft;
                }
                if (stack.Count > 0)//第2步
                {
                    node = stack.Pop();
                    node = node.ChildRight;

                }
            }
            return tree;
        }
        /// <summary>
        /// (2)以中序序列建立二叉树
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static MyBinaryTree CreateByIn(string s)
        {
            MyBinaryTree tree = new MyBinaryTree(s);//先以层序序列初始化个树，再调整
            int _count = 0;
            IBinaryTreeNote node = tree.Head;
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.ChildLeft;
                }
                if (stack.Count > 0)
                {
                    node = stack.Pop();
                    node.Value = s[_count++];
                    node = node.ChildRight;

                }
            }
            return tree;
        }
        public static MyBinaryTree CreateByAfter(string s)
        {
            MyBinaryTree tree = new MyBinaryTree(s);//先以层序序列初始化个树，再调整
            int _count = 0;
            IBinaryTreeNote node = tree.Head;
            IBinaryTreeNote pre = tree.Head; ;
            //pre指针指向“之前出栈节点”，如果为null有问题，这里指向头节点,因为后续遍历中头节点肯定是最后被访问的。
            Stack<IBinaryTreeNote> stack = new Stack<IBinaryTreeNote>();
            while (node != null || stack.Count > 0)
            {
                while (node != null)
                {
                    stack.Push(node);
                    node = node.ChildLeft;
                }
                if (stack.Count > 0)
                {
                    IBinaryTreeNote temp = stack.Peek().ChildRight;//获取栈顶元素的右孩子，C#的栈因为有了这个方法使得操作简单
                    if (temp == null || temp == pre)//满足规则1
                    {
                        node = stack.Pop();//出栈进行访问
                        node.Value = s[_count++];
                        pre = node;//设置“之前出栈节点”
                        node = null;//防止null再次入栈
                    }
                    else
                    {
                        node = temp;//规则2 继续循环。将栈顶节点的右孩子入栈，重复规则1的操作
                    }
                }
            }
            return tree;
        }

    }
}
