public class MovingInput
{
	public bool Pressed = false;
	public float HorizontalValue;
	public float VerticalValue;

	public MovingInput(bool pressed = false, float horizontalValue = 0.0f, float verticalValue = 0.0f)
	{
		Pressed = pressed;
		HorizontalValue = horizontalValue;
		VerticalValue = verticalValue;
	}
}