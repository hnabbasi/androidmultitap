using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AndroidMultitap.Helpers
{
    public static class TaskRunner
    {
        public static async Task RunSafe(Task task)
        {
            Exception exception = null;

            try
            {
                await task;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Task Cancelled");
            }
            catch (AggregateException e)
            {
                var ex = e.InnerException;
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                exception = ex;
            }
            catch (Exception e)
            {
                exception = e;
            }

            if (exception != null)
            {
                Debug.WriteLine(exception);
                throw exception;
            }
        }
    }
}
