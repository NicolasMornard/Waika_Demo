using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Epervier : Character
{
	// Public Getters

	void Start()
	{
		LookAtTargetTransform = FindObjectOfType<Avatar>().transform;
	}
}
