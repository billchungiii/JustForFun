using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp36
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in GetMembers("", 0, Gender.F))
            {
                Console.WriteLine($" {item.Name} -- {item.Age} -- {item.Gender} ");
            }
            Console.ReadLine();
        }

        private static List<Member> GetMembers(string conditionName, int conditionAge, Gender conditionGender)
        {
            var members = new List<Member>
            {
                new Member { Name = "AX", Age = 11, Gender = Gender.M },
                new Member { Name ="AA", Age =12,  Gender = Gender.M},
                new Member { Name = "B", Age = 11, Gender = Gender.M },
                new Member { Name ="AB", Age =10,  Gender = Gender.M},

            };

            var conditions = CreateConditions(conditionName, conditionAge, conditionGender);             
            return Request(members, conditions, conditionName, conditionAge, conditionGender).ToList();

        }

        private static List<Condition> CreateConditions(string conditionName, int conditionAge, Gender conditionGender)
        {
            return new List<Condition>
            {
                new Condition( (x, y, z ) => !string.IsNullOrWhiteSpace(x), (x) => x.Name.Contains(conditionName)),
                new Condition( (x, y, z ) => y != 0, (x) => x.Age < conditionAge),
                new Condition( (x, y, z ) => z != Gender.None, (x) => x.Gender < conditionGender),
            };


        }

        private static IEnumerable<Member> Request(IEnumerable <Member> members, List<Condition> conditions, string conditionName, int conditionAge, Gender conditionGender)
        {
            
            if (conditions.Count > 0 )
            {
                if (conditions[0].Source(conditionName, conditionAge, conditionGender))
                {
                    members = members.Where(conditions[0].Target);                     
                }
                conditions.RemoveAt(0);
                return Request(members, conditions, conditionName,conditionAge , conditionGender);
            }
            return members;            
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

