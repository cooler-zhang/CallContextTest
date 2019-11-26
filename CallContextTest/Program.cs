using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CallContextTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------------TestCallContext-------------------------------");

            var test1 = new TestCallContext();
            Console.WriteLine("-----------------------------Test1 Logical Data(Master Thread Set)-----------------------------");
            test1.Set("Hello");
            Console.WriteLine("Sync Value:" + test1.Get());
            Console.WriteLine("Async Value:" + test1.GetAsync().Result);

            test1.Set("Hello Hello");

            Console.WriteLine();

            var task1 = Task.Run(() =>
            {
                Console.WriteLine("-----------------------------Test2 Logical Data(Thread Set)-----------------------------");
                TestCallContext test2 = new TestCallContext();
                test2.Set("Hello");
                Console.WriteLine("Sync Value:" + test2.Get());
                Console.WriteLine("Async Value:" + test2.GetAsync().Result);

                Console.WriteLine("Sync Value 1:" + test1.Get());

                test1.Set("Hello2");

                Console.WriteLine("Sync Value 2:" + test1.Get());
            });

            Task.WaitAll(task1);

            Console.WriteLine("Sync Value 3:" + test1.Get());

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("-----------------------TestCallContext2-------------------------------");

            var test3 = new TestCallContext2();
            Console.WriteLine("-----------------------------Test3 Data (Master Thread Set)-----------------------------");
            test3.Set("Hello");
            Console.WriteLine("Sync Value:" + test3.Get());
            Console.WriteLine("Async Value:" + test3.GetAsync().Result);

            Console.WriteLine();

            var task2 = Task.Run(() =>
            {
                Console.WriteLine("-----------------------------Test4 Logical Data (Thread Set)-----------------------------");
                var test4 = new TestCallContext();
                test4.Set("Hello");
                Console.WriteLine("Sync Value:" + test4.Get());
                var t1 = test4.GetAsync();
                var t2 = test4.GetAsync();
                var t3 = test4.GetAsync();
                var t4 = test4.GetAsync();
                var t5 = test4.GetAsync();

                Task.WaitAll(t1, t2, t3, t4, t5);
                Console.WriteLine("Async Value1:" + t1.Result);
                Console.WriteLine("Async Value2:" + t2.Result);
                Console.WriteLine("Async Value3:" + t3.Result);
                Console.WriteLine("Async Value4:" + t4.Result);
                Console.WriteLine("Async Value5:" + t5.Result);
            });

            Task.WaitAll(task2);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();


            Console.WriteLine("-----------------------TestCallContext3-------------------------------");

            var test5 = new TestCallContext2();
            Console.WriteLine("-----------------------------Test3-----------------------------");
            test5.Set("Hello");
            Console.WriteLine("Sync Value:" + test5.Get());
            Console.WriteLine("Async Value:" + test5.GetAsync().Result);

            Console.WriteLine();

            var task3 = Task.Run(() =>
            {
                Console.WriteLine("-----------------------------Test4-----------------------------");
                var test6 = new TestCallContext();
                test6.SetAsync("Hello");
                Console.WriteLine("Sync Value:" + test6.GetAsync().Result);
                var t1 = test6.GetAsync();
                var t2 = test6.GetAsync();
                var t3 = test6.GetAsync();
                var t4 = test6.GetAsync();
                var t5 = test6.GetAsync();

                Task.WaitAll(t1, t2, t3, t4, t5);
                Console.WriteLine("Async Value1:" + t1.Result);
                Console.WriteLine("Async Value2:" + t2.Result);
                Console.WriteLine("Async Value3:" + t3.Result);
                Console.WriteLine("Async Value4:" + t4.Result);
                Console.WriteLine("Async Value5:" + t5.Result);
            });

            Task.WaitAll(task3);

            Console.ReadLine();
        }
    }

    public class TestCallContext
    {
        public string Key;

        public TestCallContext()
        {
            Key = Guid.NewGuid().ToString();
        }

        public void Set(string value)
        {
            Console.WriteLine("ThreadId1:" + Thread.CurrentThread.ManagedThreadId);
            CallContext.LogicalSetData(Key, value);
        }
        public Task SetAsync(string value)
        {
            return Task.Run(() =>
            {
                Console.WriteLine("ThreadId1:" + Thread.CurrentThread.ManagedThreadId);
                CallContext.LogicalSetData(Key, value);
            });
        }

        public string Get()
        {
            return CallContext.LogicalGetData(Key)?.ToString();
        }

        public Task<string> GetAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("ThreadId1:" + Thread.CurrentThread.ManagedThreadId);
                return CallContext.LogicalGetData(Key)?.ToString();
            });
        }
    }

    public class TestCallContext2
    {
        public const string Key = "Key";

        public void Set(string value)
        {
            Console.WriteLine("ThreadId2:" + Thread.CurrentThread.ManagedThreadId);
            CallContext.SetData(Key, value);
        }

        public string Get()
        {
            return CallContext.GetData(Key)?.ToString();
        }

        public Task<string> GetAsync()
        {
            return Task.Run(() =>
            {
                Console.WriteLine("ThreadId2:" + Thread.CurrentThread.ManagedThreadId);
                return CallContext.GetData(Key)?.ToString();
            });
        }
    }
}
