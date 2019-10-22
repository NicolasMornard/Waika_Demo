using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
	[SerializeField] private float hp				= 10.0f;
	[SerializeField] private float mana				= 100f;
	[SerializeField] private float maxSpeed			= 10.0f;
	[SerializeField] private float dashSpeed		= 50.0f;
	[SerializeField] private float animationSpeed	= 0.75f;
	[SerializeField] private float flinchDuration	= 0.5f;
	[SerializeField] private float thresholdRun		= 0.04f;
	[SerializeField] private float thresholdIdle	= 0.015f;
	[SerializeField] private float thresholdDash	= 1.6f;
	[SerializeField] private float diagonalAngle	= 70.0f;
	[SerializeField] private List<CharacterCapabilities> capabilities;

	// Enum
	public enum CharacterCapabilities
	{
		Idle,
		Walk,
		Run,
		Dash,
		Fly,
		Slide,
		Equipment,
		LookAtTarget,
		Interaction,
		Flinch,
		Search,
		FollowPath,
		Attack,
		LongRangeAttack,
		Story,
	};

	private static readonly Dictionary<CharacterCapabilities, Character.CharacterState> characterStateForCapabilities =
		new Dictionary<CharacterCapabilities, Character.CharacterState>()
		{
			{CharacterCapabilities.Idle, Character.CharacterState.Idle | Character.CharacterState.AnimationNone},
			{CharacterCapabilities.Walk, Character.CharacterState.Walk},
			{CharacterCapabilities.Run, Character.CharacterState.Run},
			{CharacterCapabilities.Dash, Character.CharacterState.Dash},
			{CharacterCapabilities.Fly, Character.CharacterState.Fly},
			{CharacterCapabilities.Flinch, Character.CharacterState.FlinchNone | Character.CharacterState.Flinch},
			{CharacterCapabilities.Story, Character.CharacterState.StoryNone | Character.CharacterState.StoryStuck},
		};

	private static readonly Dictionary<CharacterCapabilities, NPC.EnemyState> npcStateForCapabilities =
		new Dictionary<CharacterCapabilities, NPC.EnemyState>()
		{
			{CharacterCapabilities.Idle, NPC.EnemyState.Idle},
			{CharacterCapabilities.Attack,
				NPC.EnemyState.AttackNone |
				NPC.EnemyState.StandardAttack |
				NPC.EnemyState.Fight |
				NPC.EnemyState.Flee},
			{CharacterCapabilities.LongRangeAttack,
				NPC.EnemyState.AttackNone |
				NPC.EnemyState.StandardAttack |
				NPC.EnemyState.Fight |
				NPC.EnemyState.Flee},
			{CharacterCapabilities.FollowPath,
				NPC.EnemyState.FollowPath |
				NPC.EnemyState.WaypointNone |
				NPC.EnemyState.ReachedWaypoint |
				NPC.EnemyState.MovingToWaypoint},
			{CharacterCapabilities.Search,
				NPC.EnemyState.Search |
				NPC.EnemyState.DetectionIdle |
				NPC.EnemyState.LookingForAvatar |
				NPC.EnemyState.DetectionPullingWay},
		};

	private static readonly Dictionary<CharacterCapabilities, Avatar.AvatarState> avatarStateForCapabilities =
		new Dictionary<CharacterCapabilities, Avatar.AvatarState>()
		{
			{CharacterCapabilities.Attack,
				Avatar.AvatarState.AttackNone |
				Avatar.AvatarState.StandardAttack |
				Avatar.AvatarState.SpecialAttack1},
			{CharacterCapabilities.Slide,
				Avatar.AvatarState.SlideNone |
				Avatar.AvatarState.Slide},
			{CharacterCapabilities.LongRangeAttack,
				Avatar.AvatarState.AttackNone |
				Avatar.AvatarState.FrondeAttack},
			{CharacterCapabilities.Equipment,
				Avatar.AvatarState.EquipmentNone |
				Avatar.AvatarState.EquipmentFronde |
				Avatar.AvatarState.EquipmentHammer},
			{CharacterCapabilities.LookAtTarget,
				Avatar.AvatarState.LookAtTagetFree |
				Avatar.AvatarState.LookAtTagetLocked},
			{CharacterCapabilities.Interaction,
				Avatar.AvatarState.InteractionDialogue |
				Avatar.AvatarState.InteractionLearnWord},
		};

	public static bool StateAllowed(CharacterCapabilities capability, Character.CharacterState state)
	{
		if (characterStateForCapabilities.TryGetValue(capability, out Character.CharacterState stateValue))
		{
			return stateValue.HasFlag(state);
		}
		return false;
	}

	public static bool StateAllowed(CharacterCapabilities capability, NPC.EnemyState state)
	{
		if (npcStateForCapabilities.TryGetValue(capability, out NPC.EnemyState stateValue))
		{
			return stateValue.HasFlag(state);
		}
		return false;
	}

	public static bool StateAllowed(CharacterCapabilities capability, Avatar.AvatarState state)
	{
		if (avatarStateForCapabilities.TryGetValue(capability, out Avatar.AvatarState stateValue))
		{
			return stateValue.HasFlag(state);
		}
		return false;
	}

	public float HP
	{
		get
		{
			return hp;
		}
		set
		{
			hp = value;
		}
	}

	public float Mana
	{
		get
		{
			return mana;
		}
		set
		{
			mana = value;
		}
	}

	public float MaxSpeed
	{
		get
		{
			return maxSpeed;
		}
		set
		{
			maxSpeed = value;
		}
	}

	public float DashSpeed
	{
		get
		{
			return dashSpeed;
		}
		set
		{
			dashSpeed = value;
		}
	}

	public float AnimationSpeed
	{
		get
		{
			return animationSpeed;
		}
		set
		{
			animationSpeed = value;
		}
	}
	public float FlinchDuration
	{
		get
		{
			return flinchDuration;
		}
		set
		{
			flinchDuration = value;
		}
	}
	public float ThresholdRun
	{
		get
		{
			return thresholdRun;
		}
		set
		{
			thresholdRun = value;
		}
	}
	public float ThresholdIdle
	{
		get
		{
			return thresholdIdle;
		}
		set
		{
			thresholdIdle = value;
		}
	}
	public float ThresholdDash
	{
		get
		{
			return thresholdDash;
		}
		set
		{
			thresholdDash = value;
		}
	}
	public float DiagonalAngle
	{
		get
		{
			return diagonalAngle;
		}
		set
		{
			diagonalAngle = value;
		}
	}
	public List<CharacterCapabilities> Capabilities
	{
		get
		{
			return capabilities;
		}
		set
		{
			capabilities = value;
		}
	}

	void Awake()
	{
		Debug.Assert(Capabilities != null && Capabilities.Count > 0, transform.root.name + " has no Capability set");
	}
}
