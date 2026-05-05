namespace TinyMvCs.MvLib;

public interface IMvStageBase {}
public class MvStagePayload<T>
{
    public MvStagePayload(T current, T newValue)
    {
        CurrentValue = current;
        NewValue = newValue;
    }
    
    public T CurrentValue { get; set; }
    public T NewValue { get; set; }
    public static implicit operator T(MvStagePayload<T> payload) => payload.NewValue;
}

public class MvBinderStageEntry<T> : IMvBinderEntry
{
    private MvStage<T> _stage;
    private MvStage<T>.StageHandler _callback;
    public MvBinderStageEntry(MvStage<T> stage, MvStage<T>.StageHandler callback)
    {
        _stage = stage;
        _callback = callback;
    }
    
    public void Bind()
    {
        _stage.AddCallback(_callback);
    }

    public void UnBind()
    {
        _stage.RemoveCallback(_callback);
    }
}


public class MvStage<T> : IMvStageBase
{
    public delegate void StageHandler(MvStagePayload<T> payload);

    private StageHandler _callbacks;
    
    public MvStage(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
    
    public void AddCallback(StageHandler callback)
    {
        if (callback == null)
            return;
        
        _callbacks += callback;
    }
    
    public void RemoveCallback(StageHandler callback)
    {
        if (callback == null)
            return;
        
        _callbacks -= callback;
    }
    
    public void InvokeCallbacks(MvStagePayload<T> payload)
    {
        _callbacks?.Invoke(payload);
    }
    
    public void ClearCallbacks()
    {
        _callbacks = null;
    }

    public MvBinderStageEntry<T> MakeBinderEntry(StageHandler callback)
    {
        return new MvBinderStageEntry<T>(this, callback);
    }

    public void Bind(IMvBinderStateBase binderState, StageHandler callback)
    {
        binderState.AddEntry(MakeBinderEntry(callback));
    }
}