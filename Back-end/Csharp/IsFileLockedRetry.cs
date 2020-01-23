using System.IO;
using System.Threading;

public class IsFileLockedRetry
{
	public void CheckFile()
	{   
		try
		{
			var fileLocked = Retry(() => IsFileLocked("C:\\Disc.dat"), TimeSpan.FromSeconds(10), 5);

			if (!fileLocked)
			{
				//other logic here
			}
			else
			{
				throw new Exception("File is locked");
			}
		}
		catch (Exception ex)
		{
			throw new Exception("An error occured");
		}
	}

	private bool IsFileLocked(string filePath)
	{
		FileStream stream = null;

		try
		{
			stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
		}
		catch (IOException e)
		{
			//the file is unavailable because it is:
			//still being written to
			//or being processed by another thread
			//or does not exist (has already been processed)
			throw e;
		}
		finally
		{
			if (stream != null)
				stream.Close();
		}

		//file is not locked
		return false;
	}

	private T Retry<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = 3)
	{
		var exceptions = new List<Exception>();

		for (int attempted = 0; attempted < maxAttemptCount; attempted++)
		{
			try
			{
				if (attempted > 0)
					Thread.Sleep(retryInterval);

				return action();
			}
			catch (Exception ex)
			{
				exceptions.Add(ex);
			}
		}

		throw new AggregateException(exceptions);
	}
}
