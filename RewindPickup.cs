public class RewindPickup : Powerup
{
	public Manipulate[] actions;

	public override void Activate()
	{
		MainText.Instance.PutText("REWIND");
		Manipulate[] array = actions;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Activate();
		}
	}
}
