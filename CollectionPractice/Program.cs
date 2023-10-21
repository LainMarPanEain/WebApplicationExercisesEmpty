// See https://aka.ms/new-console-template for more information
using CollectionPractice;
using System.Collections;

Console.WriteLine("Hello, World!");
Console.WriteLine("========================");
ArrayList items = new ArrayList();
items.Add(1);
items.Add("Two");
items.Add(3);
items.Add(4.5);
items.Add("hello");
foreach (var i in items)
    Console.WriteLine(i);
Console.WriteLine("========================");
items.Remove(1);
items.RemoveAt(0);
items.Insert(0, "me");
foreach (var i in items)
    Console.WriteLine(i);
Console.WriteLine("========================");
items.Reverse();
foreach (var i in items)
    Console.WriteLine(i);


Console.WriteLine("==========students==============");
ArrayList students = new ArrayList();
students.Add("aye aye");
students.Add("nway nway");
students.Add("su su");
students.Sort();
foreach (string i in students)
    Console.WriteLine(i);

Console.WriteLine("==========Non-generic demo==============");
Utility u= new Utility();
u.GetMsgs();
u.GetMarks();
u.GetStudents();
u.GetOrderItem();
u.GetWords();

Console.WriteLine("==========Teenage Student with LINQ==============");
TeenageStudentHelper teenageStudentHelper = new TeenageStudentHelper();
teenageStudentHelper.TeenagerStudent();