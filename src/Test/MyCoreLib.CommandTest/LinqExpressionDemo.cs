using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MyCoreLib.CommandTest
{
    public class LinqExpressionDemo
    {
        public void Demo()
        {
            //simple express:  2 + 3
            Expression e = Expression.Add(Expression.Constant(2), Expression.Constant(3));
            //lambda express: () => 2 + 3;
            LambdaExpression l = Expression.Lambda(e, null);
            //compile to delegate.
            Delegate d = l.Compile();
            //convert to 
            Func<int> f = (Func<int>)d;

            Console.WriteLine(e);
            Console.WriteLine(l);
            Console.WriteLine(d);
            Console.WriteLine(f());


            ParameterExpression p = Expression.Parameter(typeof(int), "x");
            //expression: x + 3
            Expression e2 = Expression.Add(p, Expression.Constant(3));
            //Lambda: x => x + 3;
            LambdaExpression l2 = Expression.Lambda(e2, new ParameterExpression[] { p });
            Delegate d2 = l2.Compile();
            Func<int, int> f2 = (Func<int, int>)d2;

            Console.WriteLine(e2);
            Console.WriteLine(l2);
            Console.WriteLine(d2);
            Console.WriteLine(f2(3));

        }

        private void Demo2()
        {
            //Expression<Func<int, int>> lambda = n =>
            //{
            //    int result = 0;
            //    for (int j = n; j >= 0; j--)
            //    {
            //        result += j;
            //    }
            //    return result;
            //};

            //变量表达式
            ParameterExpression i = Expression.Variable(typeof(int), "i");
            //变量表达式
            ParameterExpression sum = Expression.Variable(typeof(int), "sum");
            //跳出循环标志
            LabelTarget label = Expression.Label(typeof(int));
            //块表达式
            BlockExpression block =
                Expression.Block(
                    //添加局部变量
                    new[] { sum },
                    //为sum赋初值 sum=1
                    //Assign表示赋值运算符
                    Expression.Assign(sum, Expression.Constant(1, typeof(int))),
                    //loop循环
                    Expression.Loop(
                        //如果为true 然后求和，否则跳出循环
                        Expression.IfThenElse(
                        //如果i>=0
                        Expression.GreaterThanOrEqual(i, Expression.Constant(0, typeof(int))),
                                  //sum=sum+i;i++;
                                  Expression.AddAssign(sum, Expression.PostDecrementAssign(i)),
                            //跳出循环
                            Expression.Break(label, sum)
                        ), label
                     )  // Loop ends
                 );
            int resutl = Expression.Lambda<Func<int, int>>(block, i).Compile()(100);
            Console.WriteLine(resutl);
            Console.Read();
        }
    }
}
