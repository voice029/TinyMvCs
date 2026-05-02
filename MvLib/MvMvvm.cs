namespace TinyMvCs.MvLib;

public interface IMvMvvmBase
{
    IMvValueBase[] AttachedValues { get; }
    void Attach<T>(MvValue<T> mvValueBase);
}

public class MvMvvm<T> : IMvMvvmBase where T : MvMvvm<T>
{
    public MvMvvm()
    {
        var values = GetSingleThreadedAttachedValues();
        for (int i = 0; i < GetSingleThreadedAttachedValuesCount(); i++)
        {
            values[i].Attach(this);
        }
        ClearSingleThreadMvValueInst();
    }

    public IMvValueBase[] AttachedValues { get; }
    
    public void Attach<T1>(MvValue<T1> mvValueBase)
    {
            
    }
    
    private static IMvValueBase[] _sValues = new IMvValueBase[50];
    private static int _sValuesCount = 0;
    
    public static void AddSingleThreadMvValueInst<T1>(MvValue<T1> mvValue)
    {
        _sValues[_sValuesCount] = mvValue;
        ++_sValuesCount;
    }

    private static IMvValueBase[] GetSingleThreadedAttachedValues()
    {
        return _sValues;
    }
    
    private int GetSingleThreadedAttachedValuesCount()
    {
        return _sValuesCount;
    }
    
    private static void ClearSingleThreadMvValueInst()
    {
        _sValuesCount = 0;
    }
}