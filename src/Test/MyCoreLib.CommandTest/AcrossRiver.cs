using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.CommandTest
{
    public class SeqQueue<T>
    {
        // 顺序队列类型定义
        public int Max; // 队列中最大元素个数
        public int F;
        public int R;
        public int[] Value;
    }
    /// <summary>
    /// 一个农夫带着一只狼、一只羊和一棵白菜，身处河的南岸。他要把这些东西全部运到北岸。
    /// 问题是他只有一条船，船小到只能容下他和一件物品，当然，船只有农夫能撑。
    /// 另外，因为狼能吃羊，而羊爱吃白菜，所以农夫不能留下羊和狼或者羊和白菜单独在河的一边，自己离开。
    /// 好在狼属于食肉动物，它不吃白菜。请问农夫该采取什么方案，才能将所有的东西安全运过河呢？ 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AcrossRiver<T>
    {
        SeqQueue<T> PSeqQueue;

        public SeqQueue<T> CreateEmptyQueue(int max)
        {
            PSeqQueue = new SeqQueue<T>()
            {
                Max = max,
                F = 0,
                R = 0
            };
            PSeqQueue.Value = new int[max];
            return PSeqQueue;
        }
        public bool IsEmptyQueue(SeqQueue<T> queue)
        {
            return queue.F == queue.R;
        }

        public void EnQueue(SeqQueue<T> queue, int value)
        {
            if ((queue.R + 1) % queue.Max == queue.F)
                Console.WriteLine("Full queue.");
            else
            {
                queue.Value[queue.R] = value;
                queue.R = (queue.R + 1) % queue.Max;
            }
        }
        public void DeQueue(SeqQueue<T> queue)
        {
            if (queue.F == queue.R)
                Console.WriteLine("Empty queue.");
            else
                queue.F = (queue.F + 1) % queue.Max;
        }
        public int FrontQueue(SeqQueue<T> queue)
        {
            if (queue.F == queue.R)
            {
                Console.WriteLine("Empty queue.");
                return -1;
            }
            else
                return queue.Value[queue.F];
        }
        /// <summary>
        /// 判断农夫的位置
        /// </summary>
        /// <param name="location"></param>
        /// <returns>false 过河，true 没有过河</returns>
        public bool Farmer(int location)
        {
            return (0 != (location & 0x08));
        }
        /// <summary>
        /// 判断狼的位置
        /// </summary>
        /// <param name="location"></param>
        /// <returns>false 过河，true 没有过河</returns>
        public bool Wolf(int location)
        {
            return (0 != (location & 0x04));
        }
        /// <summary>
        /// 判断白菜的位置
        /// </summary>
        /// <param name="location"></param>
        /// <returns>false 过河，true 没有过河</returns>
        public bool Cabbage(int location)
        {
            return (0 != (location & 0x02));
        }
        /// <summary>
        /// 判断羊的位置
        /// </summary>
        /// <param name="location"></param>
        /// <returns>false 过河，true 没有过河</returns>
        public bool Goat(int location)
        {
            return (0 != (location & 0x01));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns>若状态安全则返回true</returns>
        public bool IsSafe(int location)
        {
            // 羊吃白菜
            if ((Goat(location) == Cabbage(location)) && (Goat(location) != Farmer(location)))
                return false;
            // 狼吃羊
            if ((Goat(location) == Wolf(location)) && (Goat(location) != Farmer(location)))
                return false;
            return true;
        }

        public void Across()
        {
            int i, movers, location, newLocation;
            int[] route = new int[16];
            SeqQueue<T> moveTo = CreateEmptyQueue(20);
            EnQueue(moveTo, 0x00);
            for (i = 0; i < 16; i++)
                route[i] = -1;
            route[0] = 0;
            while (!IsEmptyQueue(moveTo) && (route[15] == -1))
            {
                location = FrontQueue(moveTo);
                DeQueue(moveTo);
                for (movers = 1; movers <= 8; movers <<= 1)
                {
                    //考虑各种物品移动
                    if ((0 != (location & 0x08)) == (0 != (location & movers)))
                    {
                        newLocation = location ^ (0x08 | movers); //计算新状态
                        if (IsSafe(newLocation) && (route[newLocation] == -1))
                        //新状态安全且未处理
                        {
                            route[newLocation] = location; //记录新状态的前驱
                            EnQueue(moveTo, newLocation); //新状态入队
                        }
                    }
                }
            }
            //到达最终状态
            if (route[15] != -1)
            {
                Console.WriteLine("The reverse path is : ");
                for (location = 15; location >= 0; location = route[location])
                {
                    Console.WriteLine("The location is : {0},{1}", location, Convert.ToString(location, 2));
                    if (location == 0)
                        break;
                }
            }
            else
                Console.WriteLine("No solution.");
        }
    }
}
