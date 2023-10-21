using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionPractice
{
    public class Utility
    {
        //Generic class include <>
        public void GetMsgs()
        {
            List<string> msg = new List<string> {
            "Hi",
            "Hello",
            "Thanks"
            };
            List<string> fruits = new List<string> { "apple", "orange", "banana"};
            msg.AddRange(fruits);

            foreach (string s in msg) { 
                Console.WriteLine(s);  
            }
        }
        public void GetMarks()
        {
            IList<int> marks = new List<int> { 100, 80 };
            marks.Add(98);
            foreach (int i in marks)
            {
                Console.WriteLine(i);
            }
        }

        public void GetStudents()
        {
            IList<Student> students = new List<Student>();//Generic class
            Student s1 = new Student() { 
                Id = 1,
                Name = "Su"
            };
            Student s2 = new Student()
            {
                Id = 2,
                Name = "Mei"
            };
            Student s3 = new Student()
            {
                Id = 3,
                Name = "Nway"
            };
            students.Add(s1);
            students.Add (s2); 
            students.Add(s3);
            foreach(Student s in students)
            {
                Console.WriteLine(s.Id);
                Console.WriteLine(s.Name);
            }
        }
        public void GetOrderItem()
        {
            Stack<int> order = new Stack<int>();
            order.Push(1);
            order.Push(2);
            order.Push(3);
            int count = order.Count;
            for(int i = 0; i < count; i++)
            {
                Console.WriteLine(order.Pop());
            }
        }
        public void GetWords()  //To test LINQ
        {
            string[] words = {"abc", "abcd", "abcde" };
            var longWords = words.Where(w => w.Length > 10);
            foreach(string word in longWords)
                Console.WriteLine(word);

            var firstWord = words.Where(w => w == "abc");
            Console.WriteLine(firstWord);

            var shortWords = words.Where(w => w.Length < 4);
            foreach(string word in shortWords)
                Console.WriteLine(word);
        }
    }
}
