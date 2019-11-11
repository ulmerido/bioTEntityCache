namespace bioTCache
{
    public interface ISerializer
    {
        string Serialize<T>(T i_Obj);
        T Deserialize<T>(string i_Obj);
    }
}
