using Godot;

namespace NewGameProject.Scripts.Entities.Weapons;

public partial class Crossbow : RangedWeapon
{
	public override void _Ready()
	{
		base._Ready();
	}

	public override void Fire(Vector2 direction)
	{
		GD.Print("Crossbow firing");
		
		base.Fire(direction);
	}
	
}
