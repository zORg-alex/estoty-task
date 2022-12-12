public interface IInteractable
{
	void Interact(IInteractor interactor);
	void InteractionFinished(IInteractor interactor);
}