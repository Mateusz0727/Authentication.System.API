namespace Authentication.System.API.Models.Settings;

public class MongoDBSetting
{
    public string? ConnectionURI { get; set; }=null;
    public string? DatabaseName { get; set; }=null;
    
    public string? CollectionName { get; set; }=null;


}