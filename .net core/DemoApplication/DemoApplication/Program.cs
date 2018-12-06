namespace DemoApplication
{
    using Demo.Contract.Models;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Student s = new Student() { FirstName = "Shailendra Patil", LastName = "Patil", Address = "Pune", Age = 25 };
            Console.Read();
        }
    }
}
