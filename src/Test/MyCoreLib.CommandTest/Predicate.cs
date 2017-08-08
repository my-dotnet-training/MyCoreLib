using System;
using System.Collections.Generic;
using System.Text;

namespace MyCoreLib.CommandTest
{
    public class Predicate
    {
        static void Demo()
        {
            List<Person> list = GetList();
            //绑定查询条件
            Predicate<Person> predicate = new Predicate<Person>(Match);
            List<Person> result = list.FindAll(predicate);
            Console.WriteLine("Person count is :" + result.Count);
            Console.ReadKey();
        }
        //模拟源数据
        static List<Person> GetList()
        {
            var personList = new List<Person>();
            var person1 = new Person(1, "Leslie", 29);
            personList.Add(person1);
            return personList;
        }
        //查询条件
        static bool Match(Person person)
        {
            return person.Age <= 30;
        }
    }

    public class Person
    {
        public Person(int id, string name, int age)
        {
            ID = id;
            Name = name;
            Age = age;
        }
        public int ID
        { get; set; }
        public string Name
        { get; set; }
        public int Age
        { get; set; }
    }
}
