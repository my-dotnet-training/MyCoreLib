using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

namespace MyCoreLib.CommandTest
{
    public class EfDemo
    {
        public void DumpReflection(object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {   //得到object的具体类型的所有属性
                PropertyInfo[] pi = objs[i].GetType().GetTypeInfo().GetProperties();
                for (int j = 0; j < pi.Length; j++)
                {   //得到属性值并且打印
                    Console.WriteLine(pi[j].Name + " : " + pi[j].GetValue(objs[i], null));
                }
            }
        }
        //#############################################
        //
        //  dump Hundreds of object , using reflection linq and lambda and delegate generic.
        //
        //############################################
        public void DumpLambdaLow(object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                //如果使用C# 4.0，直接使用系统提供tuple.
                Tuple<string, Delegate>[] t = GetTuple(objs[i].GetType());

                for (int j = 0; j < t.Length; j++)
                {
                    //使用泛型委托，创建委托实例。
                    Func<object, object> temp = (Func<object, object>)t[j].Item2;
                    Console.WriteLine(t[j].Item1 + " : " + temp(objs[i]));
                }
            }
        }
        private Tuple<string, Delegate>[] GetTuple(Type t)
        {
            Tuple<string, Delegate>[] dRet = (from pi in t.GetTypeInfo().GetProperties()  //遍历每个属性
                                              let o = Expression.Parameter(typeof(object), "o")  //设置Lambda表达式参数
                                              select new Tuple<string, Delegate>(pi.Name, Expression.Lambda(Expression.Convert(Expression.Property(Expression.Convert(o, t), pi), typeof(object)), o).Compile())).ToArray();
            return dRet;
        }

        public void DumpLambdaLow4(object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                MyTuple[] t = GetMyTuple(objs[i].GetType());
                for (int j = 0; j < t.Length; j++)
                {
                    //使用泛型委托，创建委托实例。
                    Func<object, object> temp = (Func<object, object>)t[j].Delegate;
                    Console.WriteLine(t[j].Name + " : " + temp(objs[i]));
                }
            }
        }
        private MyTuple[] GetMyTuple(Type t)
        {
            //创建lambda: (o)=>((t)o).pi;(pi为类型的具体属性)
            MyTuple[] dRet = (from pi in t.GetTypeInfo().GetProperties()  //遍历每个属性
                              let o = Expression.Parameter(typeof(object), "o")  //设置Lambda表达式参数
                              select new MyTuple(pi.Name,
                              Expression.Lambda(
                                  Expression.Convert(
                                      Expression.Property(
                                          Expression.Convert(o, t),
                                      pi),
                                  typeof(object)),
                              o).Compile())).ToArray();
            //返回tuple类。
            return dRet;
        }

        public class MyTuple
        {
            public string Name { get; set; }
            public Delegate Delegate { get; set; }
            public MyTuple(string name, Delegate d)
            {
                this.Name = name;
                this.Delegate = d;
            }
        }
    }
}
