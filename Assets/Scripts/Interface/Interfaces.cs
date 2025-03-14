public interface I_Interactable
{
    void OnInteract();
    string SetPrompt();
}

public interface IStage
{
    void OnInteract();
    void SetPrompt();
}