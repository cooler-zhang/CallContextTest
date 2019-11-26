using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace CallContextTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCallContext();
            Console.ReadLine();
        }

        public static void TestCallContext()
        {
            var student = new Student { Id = 1, Name = "Cooler" };

            CallContext.SetData("student1", student);

            Console.WriteLine(((Student)CallContext.GetData("student1")).Name);

            Task task = new Task(() =>
            {
                Console.WriteLine(((Student)CallContext.GetData("student1"))?.Name);
                student.Id = 2;

                Console.WriteLine(((Student)CallContext.GetData("student1"))?.Id);

                CallContext.SetData("student1", new Student { Id = 3, Name = "Cooler" });

                Console.WriteLine(((Student)CallContext.GetData("student1"))?.Id);

                var childTask = new Task(() =>
                {
                    Console.WriteLine(((Student)CallContext.GetData("student1"))?.Id);

                    CallContext.SetData("student1", new Student { Id = 31, Name = "Cooler" });

                    Console.WriteLine(((Student)CallContext.GetData("student1"))?.Id);
                });

                childTask.Start();
                childTask.Wait();

                Console.WriteLine(((Student)CallContext.GetData("student1"))?.Id);
            });

            task.Start();
            task.Wait();

            Console.WriteLine(((Student)CallContext.GetData("student1"))?.Id);

        }


        public static void TestCallContextLogical()
        {
            var student = new Student { Id = 1, Name = "Cooler" };

            CallContext.LogicalSetData("student1", student);

            Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Name);

            Task task = new Task(() =>
            {
                Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Name);
                student.Id = 2;

                Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Id);

                CallContext.LogicalSetData("student1", new Student { Id = 3, Name = "Cooler" });

                Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Id);

                var childTask = new Task(() =>
                {
                    Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Id);

                    CallContext.LogicalSetData("student1", new Student { Id = 31, Name = "Cooler" });

                    Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Id);
                });

                childTask.Start();
                childTask.Wait();

                Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Id);
            });

            task.Start();
            task.Wait();

            Console.WriteLine(((Student)CallContext.LogicalGetData("student1")).Id);

        }
    }

    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
