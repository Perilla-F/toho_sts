using Cysharp.Threading.Tasks;

public interface ICardObj
{
    public UniTask Use();
    public void SetPreviewDelay();
    public void ClearPreview();
}
