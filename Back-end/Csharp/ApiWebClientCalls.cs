// WebClient Calls

var templateXml = TemplatesResource.TemplateXml;
var currentDate = DateTime.Now;

var request = pubsubTemplateXml
	.Replace("[SomeDataId]", n.ToString())
	.Replace("[DateTimeGeneration]", currentDate.ToString("O"))
	.Replace("[Title]", title);

string response = string.Empty;

using (WebClient wc = new WebClient())
{
	wc.Encoding = Encoding.UTF8;
	wc.Headers[HttpRequestHeader.ContentType] = "text/xml";
	response = wc.UploadString(_appSettings.EndpointUrl, request);
}
