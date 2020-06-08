void Main()
{
    var fileLocked = Retry(() => IsFileLocked("C:\\Test.txt"), TimeSpan.FromSeconds(10), 5);
	
    fileLocked.Dump("Locked");
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

