using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerStatus
{
	private int maxHP;
	private double airTimeDuration;

	private int currentMemoryShards;

	private int currentHP;
	private int remainReviveCount;

	public Action onChangePlayerStatus;

	public int CurrentMemoryShards
	{
		get { return currentMemoryShards; }
		set { 
			currentMemoryShards = value;
			if (onChangePlayerStatus != null) onChangePlayerStatus();
		}
	}

	public int RemainReviveCount
	{
		get { return remainReviveCount; }
		set { 
			remainReviveCount = value;
		}
	}


	public int CurrentHP
	{
		get { return currentHP; }
		set { 

			currentHP = value;
			if (currentHP <= 0) currentHP = 0;
			if (onChangePlayerStatus != null) onChangePlayerStatus();
		}
	}


	public double AirTimeDuration
	{
		get {
			airTimeDuration = GameStatics.default_AirTimeDuration + (GameStatics.default_IncAirTimeDuration * GameConfigs.SkillLevel(GameStatics.SKILL_TYPE.AIRTIME_DURATION));
			return airTimeDuration; 
		}
	}


	public int MaxHP
	{
		get {
			maxHP = GameStatics.default_BaseMaxHP + (GameStatics.default_IncHP * GameConfigs.SkillLevel(GameStatics.SKILL_TYPE.MAXHP));	
			return maxHP; 
		}
	}

	public void InitPlayerForGameStart()
	{
		currentMemoryShards = GameConfigs.CurrentMemoryShards;
		currentHP = MaxHP;
		remainReviveCount = GameStatics.default_MaxReviceCount;

		if (onChangePlayerStatus != null) onChangePlayerStatus();
	}

}
