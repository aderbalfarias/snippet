public static IEnumerable<T> ReadExcel<T>()
{
    //Install-Package ExcelDataReader
    var path = "D:\\Projetos C#\\DemoExcel\\Files";
    var file = "Carga.xlsx";
    var filePath = Path.Combine(path, file);

    FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);

    //1: Read file OpenXml Excel (formato 97-2003; *.xls)
    //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);

    //2: Read file OpenXml Excel (formato 2007; *.xlsx)
    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

    DataSet data = excelReader.AsDataSet();
    excelReader.Close();

    IList<Entity> list = new List<Entity>();
    int sheet = data.Tables.Contains("SheetName") ? 0 : 1;

    for (int i = 1; i < data.Tables[sheet].Rows.Count; i++)
    {
        if (!string.IsNullOrEmpty(data.Tables[sheet].Rows[i].ItemArray[0].ToString()))
        {
            list.Add(new Entity
            {
                Id = Convert.ToInt64(data.Tables[sheet].Rows[i].ItemArray[0]),
                Nome = data.Tables[sheet].Rows[i].ItemArray[1].ToString(),
                Observacao = data.Tables[sheet].Rows[i].ItemArray[2].ToString(),
                Data = Convert.ToDateTime(data.Tables[sheet].Rows[i].ItemArray[3])
            });
        }
    }

    return (IEnumerable<T>) list;
}

//-----------------------------------------------------------------------------------------------------------------------------
//Example context
private readonly Context _context = new Context();

public void SaveData<T>(IEnumerable<T> list)
{
    //Install-Package EntityFramework.BulkInsert-ef6
    _context.BulkInsert(list);
    _context.SaveChanges();
}