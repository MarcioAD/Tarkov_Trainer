﻿using EFT.InventoryLogic;
using EFT.Trainer.Configuration;
using EFT.Trainer.Extensions;
using EFT.Trainer.UI;
using UnityEngine;
using System;
using JetBrains.Annotations;

#nullable enable

namespace EFT.Trainer.Features
{
	[UsedImplicitly]
	internal class Hud : ToggleFeature
	{
		public override string Name => "hud";

		[ConfigurationProperty]
		public Color Color { get; set; } = Color.white;

		[ConfigurationProperty]
		public bool Compass { get; set; } = true;

		private static readonly string[] _directions = { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };

		protected override void OnGUIWhenEnabled()
		{
			var player = GameState.Current?.LocalPlayer;
			if (!player.IsValid())
				return;

			if (player.HandsController == null || player.HandsController.Item is not Weapon weapon)
				return;

			var mag = weapon.GetCurrentMagazine();
			if (mag == null)
				return;

			var prefix = "";
			if (Compass)
			{
				var forward = player.Transform.forward;
				forward.y = 0;
				var heading = Quaternion.LookRotation(forward).eulerAngles.y;
				prefix = _directions[(int)Math.Round((double)heading % 360 / 45)] + " - ";
			}

			var hud = $"{prefix}{mag.Count}+{weapon.ChamberAmmoCount}/{mag.MaxCount} [{weapon.SelectedFireMode}]";
			Render.DrawString(new Vector2(512, Screen.height - 16f), hud, Color);
		}
	}
}
