using System.IO;
using Newtonsoft.Json;

public class TemplateLoader
{
	public TemplateFile LoadTemplateFile(string filePath)
	{
		try
		{
			string value;
			using (var reader = new StreamReader(filePath, true))
			{
				value = reader.ReadToEnd();
			}
			TemplateFile templateFile = JsonConvert.DeserializeObject<TemplateFile>(value);
			if (templateFile == null)
			{
				throw new InvalidDataException("Deserialized JSON data is null.");
			}
			return templateFile;
		}
		catch (JsonException ex)
		{
			throw new InvalidDataException("Error parsing JSON data. " + ex.Message, ex);
		}
		catch (IOException ex2)
		{
			throw new IOException("Error reading JSON file. " + ex2.Message, ex2);
		}
	}
}
