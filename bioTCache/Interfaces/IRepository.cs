using bioTCache.Interfaces;

namespace bioTCache
{
    public interface IRepository
    {
        IResponse Get(int i_Id);
        IResponse GetAll();
        IResponse Add(string i_Key, string i_Value);
        IResponse Update(string i_Key, string i_Value);
        IResponse Remove(int i_Id);
    }
}
