public interface IInteractable
{
	void InteractionStarted(IInteractor interactor);
	void InteractionFinished(IInteractor interactor);
}