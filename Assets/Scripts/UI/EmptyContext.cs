using Scripts.Input;

namespace Scripts.UI
{
	public class EmptyContext : UIContext, IInputBack
	{
		public void OnBack()
		{
			Hide();
		}
	}
}