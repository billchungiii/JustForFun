using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp37
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in GetMembers("A", 12, Gender.F))
            {
                Console.WriteLine($" {item.Name} -- {item.Age} -- {item.Gender} ");
            }
            Console.ReadLine();
        }

        public static List<Member> GetMembers(string conditionName, int conditionAge, Gender conditionGender)
        {
            var members = new List<Member>
            {
                new Member { Name = "AX", Age = 11, Gender = Gender.M },
                new Member { Name ="AA", Age =12,  Gender = Gender.M},
                new Member { Name = "B", Age = 11, Gender = Gender.F},
                new Member { Name ="AB", Age =10,  Gender = Gender.M},

            };

            return members.Where(CreateCondition(conditionName, conditionAge, conditionGender)).ToList();
        }

        public static Func<Member, bool> CreateCondition(string conditionName, int conditionAge, Gender conditionGender)
        {


            List<Condition> conditions = new List<Condition>
            {
                new Condition( (x, y, z ) => !string.IsNullOrWhiteSpace(x), (x) => x.Name.Contains(conditionName)),
                new Condition( (x, y, z ) => y != 0, (x) => x.Age < conditionAge),
                new Condition( (x, y, z ) => z != Gender.None, (x) => x.Gender < conditionGender),
            };

            Func<Member, bool> result = null;

            foreach (var func in conditions)
            {
                if (func.Source(conditionName, conditionAge, conditionGender))
                {
                    result = result.XCombine(func.Target);
                }
            }

            if (result == null)
            {
                result = new Func<Member, bool>((x) => false);
            }
            return result;
        }
    }

    public class Condition
    {

        public Func<string, int, Gender, bool> Source { get; }
        public Func<Member, bool> Target { get; }

        public Condition(Func<string, int, Gender, bool> source, Func<Member, bool> target)
        {
            Source = source;
            Target = target;
        }

    }

    public static class MyExtensions
    {
        public static Func<T, bool> XCombine<T>(this Func<T, bool> source, Func<T, bool> target)
        {
            if (source == null)
            {
                return target;
            }
            return x => source(x) && target(x);
        }
    }



    public class Member
    {
        public string Name;
        public int Age;
        public Gender Gender;
    }
    public enum Gender
    {
        None,
        M,
        F
    }

}
