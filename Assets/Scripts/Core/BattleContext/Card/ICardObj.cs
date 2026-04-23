using Cysharp.Threading.Tasks;

public interface ICardObj
{
    public SourceCard Source { get; }
    public UniTask Use();
    public void SetPreviewDelay();
    public void ClearPreview();
}
