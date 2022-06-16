namespace HenBot.Core.Extensions;

public static class TaskExtensions
{
	public static Task<TNewResult> ContinueWith<TResult, TNewResult>(
		this Task<TResult> task, 
		Func<TResult, TNewResult> continuationDelegate)
		=> task.ContinueWith((resultTask, _) => continuationDelegate(resultTask.Result), 
							 TaskContinuationOptions.OnlyOnRanToCompletion);
	
	public static Task<TNewResult> ContinueWith<TResult, TNewResult>(
		this Task<TResult> task, 
		Func<TResult, Task<TNewResult>> continuationDelegate)
		=> task.ContinueWith((resultTask, _) => continuationDelegate(resultTask.Result).Result, 
							 TaskContinuationOptions.OnlyOnRanToCompletion);
}