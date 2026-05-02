namespace TinyMvCs.MvLib;

public interface IMvValueBase
{
    Type OfMvType();
    void Attach(IMvMvvmBase mvMvvmBase);
}
public abstract class MvValue<T> : IMvValueBase
{
    private T _value;
    public MvStage<T> OnUpdate { get; set; } = new(nameof(OnUpdate));
    public MvStage<T> OnChanged { get; set; } = new(nameof(OnChanged));
    
    public T Set(T newBroadcastedValue)
    {
        MvStagePayload<T> mvStagePayload = new MvStagePayload<T>(_value, newBroadcastedValue);
        if (!EqualityComparer<T>.Default.Equals(_value, newBroadcastedValue))
        {
            _value = newBroadcastedValue;
            OnChanged.InvokeCallbacks(mvStagePayload);
        }
        OnUpdate.InvokeCallbacks(mvStagePayload);
        
        return newBroadcastedValue;
    }
    
    public T Value
    {
        get => _value;
        set => Set(value);
    }

    public abstract Type OfMvType();
    public void Attach(IMvMvvmBase mvMvvmBase)
    {
        mvMvvmBase.Attach(this);
    }
}

public class MvValue<T, TMv> : MvValue<T> where TMv : MvMvvm<TMv>
{
    public MvValue()
    {
        MvMvvm<TMv>.AddSingleThreadMvValueInst(this);
    }

    public override Type OfMvType()
    {
        return typeof(T);
    }

}