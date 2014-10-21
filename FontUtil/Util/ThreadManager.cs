using System.Collections.Generic;
using System.Threading;

namespace FontUtil
{
	public static class ThreadManager
	{
		static List<ThreadHandler> threads = new List<ThreadHandler>();

		class ThreadHandler
		{
			ParameterizedThreadStart threadFunction;
			internal Thread thread;

			public ThreadHandler(ParameterizedThreadStart function, object parameter)
			{
				threadFunction = function;
				thread = new Thread(Stub);
				threads.Add(this);
				thread.Start(parameter);
			}

			private void Stub(object o)
			{
				threadFunction(o);
				threads.Remove(this);
			}
		};

		public static Thread CreateThread(ParameterizedThreadStart function, object parameter)
		{
			return new ThreadHandler(function, parameter).thread;
		}

		public static void DeleteAllThreads()
		{
			while (threads.Count > 0)
			{
				int c = threads.Count - 1;
				threads[c].thread.Abort();
				threads.RemoveAt(c);
			}
		}
	}
}