using SimpleJSON;

public interface IJsonSavable
{
    void ImportState(JSONClass node);
    JSONClass ExportState();
}