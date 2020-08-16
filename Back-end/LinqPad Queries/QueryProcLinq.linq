void Main()
{
	var spUpdate = "usp_Proc";
	int result;

    using (SqlConnection dbConnection = new SqlConnection(this.Connection.ConnectionString))
    {
        dbConnection.Open();

        using (SqlCommand dbCommand = dbConnection.CreateCommand())
        {
            dbCommand.CommandText = spUpdate;
            dbCommand.CommandType = CommandType.StoredProcedure;
            dbCommand.Parameters.AddWithValue("@Param1", "Test");
            dbCommand.Parameters.AddWithValue("@Param2", 2);

            result = (int)dbCommand.ExecuteScalar();
        }
    }
	
	result.Dump();
}
