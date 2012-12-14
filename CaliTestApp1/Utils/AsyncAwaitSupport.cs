using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace CaliTestApp1.Utils
{
    public static class AsyncAwaitSupport
    {
        public static void Hook()
        {
            ActionMessage.InvokeAction = context =>
            {
                var values = MessageBinder.DetermineParameters(context, context.Method.GetParameters());
                var returnValue = context.Method.Invoke(context.Target, values);

                var task = returnValue as Task;
                if (task != null)
                {
                    returnValue = new TaskResult(task);
                }

                var result = returnValue as IResult;
                if (result != null)
                {
                    returnValue = new[] { result };
                }

                var enumerable = returnValue as IEnumerable<IResult>;
                if (enumerable != null)
                {
                    Coroutine.BeginExecute(enumerable.GetEnumerator(), context);
                    return;
                }

                var enumerator = returnValue as IEnumerator<IResult>;
                if (enumerator != null)
                {
                    Coroutine.BeginExecute(enumerator, context);
                    return;
                }
            };
        }

        private class TaskResult : IResult
        {
            readonly Task _task;
            public TaskResult(Task task)
            {
                if (task == null) throw new ArgumentNullException("task");
                _task = task;
            }

            public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

            public void Execute(ActionExecutionContext context)
            {
                _task.ContinueWith(t => Completed(this, new ResultCompletionEventArgs
                    {
                        WasCancelled = t.IsCanceled,
                        Error = t.Exception
                    }));
            }
        }

    }
}
