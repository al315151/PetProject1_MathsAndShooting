using Cysharp.Threading.Tasks;
public interface IBuilder
{
    public UniTask Build();
    public void Reset();
}
