using CSharpInDepth.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInDepth._15_AsynchronyWithAsyncAwait
{
    class DissectingAsyncAwait : Study
    {
        public override string StudyName
        {
            get { return "just dissect asyn method"; }
        }

        protected override void PerformStudy()
        {
            var t = SumCharactersDissected("Sumanesh");
            t.Wait();
            Console.WriteLine(t.Result);
        }

        static Task<int> SumCharactersDissected(IEnumerable<char> text)
        {
            DemoStateMachine stateMachine = new DemoStateMachine();
            stateMachine.text = text;
            stateMachine.builder = AsyncTaskMethodBuilder<int>.Create();
            stateMachine.state = -1;
            stateMachine.builder.Start(ref stateMachine);
            return stateMachine.builder.Task;
        }

        private struct DemoStateMachine : IAsyncStateMachine
        {
            public IEnumerable<char> text;

            public int total;
            public char ch;
            public int unicode;
            public IEnumerator<char> iterator;

            public AsyncTaskMethodBuilder<int> builder;
            public int state;
           // private object stack;

            private TaskAwaiter awaiter5;
            private YieldAwaitable.YieldAwaiter awaiter6;


            void IAsyncStateMachine.MoveNext()
            {
                int result = 0;

                try
                {
                    bool flag = true;
                    YieldAwaitable.YieldAwaiter awaiter1;

                    switch (state)
                    {
                        case -3:
                            goto label16;
                        case 0:
                            try
                            {
                                TaskAwaiter awaiter2;
                                if (this.state == 0)
                                {
                                    awaiter2 = this.awaiter5;
                                    this.awaiter5 = new TaskAwaiter();
                                    this.state = -1;
                                }
                                else
                                {
                                    goto label7;
                                }
                            label6:
                                awaiter2.GetResult();
                                TaskAwaiter taskAwaiter = new TaskAwaiter();
                                this.total += this.unicode;
                            label7:
                                if (iterator.MoveNext())
                                {
                                    this.ch = iterator.Current;
                                    this.unicode = this.ch;
                                    awaiter2 = Task.Delay(unicode).GetAwaiter();
                                    if (!awaiter2.IsCompleted)
                                    {
                                        this.state = 0;
                                        this.awaiter5 = awaiter2;
                                        this.builder.AwaitUnsafeOnCompleted(ref awaiter2, ref this);

                                        flag = false;
                                        return;
                                    }
                                    else
                                    {
                                        goto label6;
                                    }
                                }

                            }
                            finally
                            {
                                if(flag && this.iterator != null)
                                {
                                    this.iterator.Dispose();
                                }
                            }
                            awaiter1 = Task.Yield().GetAwaiter();
                            if (!awaiter1.IsCompleted)
                            {
                                this.state = 1;
                                this.awaiter6 = awaiter1;
                                this.builder.AwaitUnsafeOnCompleted(ref awaiter1, ref this);
                                return;
                            }
                            else
                                break;
                        case 1:
                            awaiter1 = this.awaiter6;
                            this.awaiter6 = new YieldAwaitable.YieldAwaiter();
                            this.state = -1;
                            break;
                        default:
                            this.total = 0;
                            this.iterator = this.text.GetEnumerator();
                            goto case 0;
                    }
                    awaiter1.GetResult();
                    YieldAwaitable.YieldAwaiter yieldAwaiter = new YieldAwaitable.YieldAwaiter();
                    result = this.total;
                }
                catch (Exception ex)
                {
                    state = -2;
                    builder.SetException(ex);
                    return;
                }
            label16:
                state = -2;
                builder.SetResult(result);
            }

            void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
            {
                this.builder.SetStateMachine(stateMachine);
            }
        }

        static async Task<int> SumCharactersAsync(IEnumerable<char> text)
        {
            int total = 0;

            foreach (char ch in text)
            {
                int unicode = ch;
                await Task.Delay(unicode);
                total += unicode;
            }

            await Task.Yield();

            return total;
        }
    }
}
