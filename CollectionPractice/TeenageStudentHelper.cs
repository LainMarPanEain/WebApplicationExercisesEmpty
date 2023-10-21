using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionPractice
{
    public class TeenageStudentHelper
    {
        public void TeenagerStudent()
        {
            Student[] student = {
                                new Student(){Id = 1, Name = "su", Age = 20},
                                new Student(){Id = 2, Name = "nway", Age = 21},
                                new Student(){Id = 3, Name = "yoon", Age = 25},
                                new Student(){Id = 4, Name = "amy", Age = 18},
                                new Student(){Id = 5, Name = "bean", Age = 16},
                                new Student(){Id = 6, Name = "zayar", Age = 19}
            };
            var teenageStudents = student.Where(s=>s.Age >12 && s.Age < 20).ToList();//var accept any value(array or list) that is assigned to it
            foreach(var s in  teenageStudents)// same with foreach(Student s in  teenageStudents)
                Console.WriteLine(s.Name);

            Student s1 = student.Where(s=> s.Id == 4).FirstOrDefault();//can also do with .SingleOrDefault
            Console.WriteLine(s1.Name);
        }
    }
}
