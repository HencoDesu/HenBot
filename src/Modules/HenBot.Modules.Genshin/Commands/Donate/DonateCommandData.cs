﻿using HenBot.Core.Commands;
using HenBot.Core.Commands.Parsing;
using HenBot.Core.Extensions;
using JetBrains.Annotations;

namespace HenBot.Modules.Genshin.Commands.Donate;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class DonateCommandData : ICommandData
{
	public int Amount { get; set; }

	public string Currency { get; set; } = null!;

}