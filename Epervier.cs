public class Epervier : Character
{
	// Public Getters

	void Start()
	{
		LookAtTargetTransform = FindObjectOfType<Avatar>().transform;
	}
}
