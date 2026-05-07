namespace TinyMvCs.MvLib
{
    public class Requester
    {
        public Requester() {}
        public Requester(object obj) {}
    
        public T Get<T>(object requester)
        {
            return Get<T>(requester as Requester);
        }
    
        public virtual T Get<T>(Requester requester) where T: class
        {
            return null;
        }
    }

    public class MvProvider
    {
    }

    public class MvProvider<T> where T: class
    {
        public delegate T ProviderRequestDel(Requester requester);
    
        private static ProviderRequestDel _providerRequestDel;

        public virtual T Locate(Requester requester, T defaultValue)
        {
            return _providerRequestDel?.Invoke(requester) ?? defaultValue;
        }

        public virtual T Locate(object requester, T defaultValue)
        {
            return _providerRequestDel?.Invoke(new Requester(requester)) ?? defaultValue;
        }

        public virtual void Register(ProviderRequestDel providerRequestDel)
        {
            _providerRequestDel = providerRequestDel;
        }
    
        public static MvProvider<T> Make(Requester requester)
        {
            return new MvProvider<T>();
        }
    
        public static MvProvider<T> Make(MvProvider requester)
        {
            return new MvProvider<T>();
        }
    
        public static MvProvider<T> Make(object requester)
        {
            return new MvProvider<T>();
        }
    
        public static T Provider(Requester requester, T defaultValue)
        {
            return Make(requester).Locate(requester, defaultValue);
        }
    
        public static T Provider(object requester, T defaultValue)
        {
            return Make(requester).Locate(requester, defaultValue);
        }
    
        public static void Enroll(MvProvider provider, ProviderRequestDel providerRequestDel)
        {
            Make(provider).Register(providerRequestDel);
        }
    
        public static void Enroll(object requester, ProviderRequestDel providerRequestDel)
        {
            Make(requester).Register(providerRequestDel);
        }
    }

    public class MvProviderNew<T> : MvProvider<T> where T : class, new()
    {
        public static T Provider(Requester requester)
        {
            return Make(requester).Locate(requester, new T());
        }

        public static T Provider(object requester)
        {
            return Make(requester).Locate(requester, new T());
        }
    }
}