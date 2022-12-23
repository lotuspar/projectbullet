﻿using ProjectBullet.Core.Node;
using Sandbox;

namespace ProjectBullet.Characters;

public partial class Gunner
{
	public class PrimaryFireController : NodeExecutor
	{
		public override string DisplayName => "Primary Fire";
		public override float ActionDelay => 0.13f;
		public override InputButton InputButton => InputButton.PrimaryAttack;

		private float CalculateSpread()
		{
			var speed = Player.Velocity.Length;
			return speed switch
			{
				<= 50.0f => 0.0f,
				<= 200.0f => 0.3f,
				_ => speed <= 350.0f ? 1f : 1.4f
			};
		}

		protected override void PerformAction( IClient cl )
		{
			base.PerformAction( cl );

			Game.SetRandomSeed( Time.Tick );

			var ray = Player.AimRay;

			var forward = ray.Forward;
			forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) *
			           (CalculateSpread() * 0.02f);
			forward = forward.Normal;

			var trace = Trace.Ray( ray.Position, ray.Position + forward * 5000 )
				.UseHitboxes()
				.WithAnyTags( "solid", "player", "npc", "glass" )
				.Ignore( Player );

			var result = trace.Run();

			Player.PlaySound( "rust_pistol.shoot" );

			ExecuteEntryNode( new ExecuteInfo()
				.UsingTraceResult( result )
				.WithAttacker( Player )
				.WithForce( 1, ray.Forward ) );
		}
	}
}
