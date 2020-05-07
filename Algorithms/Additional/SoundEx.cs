public class SoundEx
{
    public string Soundex(string data)
    {
        var result = new StringBuilder();
        
        if (data != null && data.Length > 0)
        {
            var currentCode = string.Empty;
            var previousCode = string.Empty;
            
            result.Append(Char.ToUpper(data[0]));
            
            for (int i = 1; i < data.Length; i++)
            {
                currentCode = EncodeChar(data[i]);
                
                if (currentCode != previousCode)
                    result.Append(currentCode);

                if (result.Length == 4) 
                    break;
                    
                if (!currentCode.Equals(string.Empty))
                    previousCode = currentCode;
            }
        }
        
        if (result.Length < 4)
            result.Append(new String('0', 4 - result.Length));
        
        return result.ToString();
    }

    private string EncodeChar(char c)
    {
        switch (Char.ToLower(c))
        {
            case 'b':
            case 'f':
            case 'p':
            case 'v':
                return "1";
            case 'c':
            case 'g':
            case 'j':
            case 'k':
            case 'q':
            case 's':
            case 'x':
            case 'z':
                return "2";
            case 'd':
            case 't':
                return "3";
            case 'l':
                return "4";
            case 'm':
            case 'n':
                return "5";
            case 'r':
                return "6";
            default:
                return string.Empty;
        }
    }
}