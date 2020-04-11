using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class PlayerStatus
{
	private int maxHP;
	private double doubleJumpCooltime;
	private double airtimeDuartion;

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
			if (onChangePlayerStatus != null) onChangePlayerStatus();
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


	public double AirtimeDuration
	{
		get {
			airtimeDuartion = GameStatics.default_AirtimeDuration + (GameStatics.default_IncAirTimeDuration * GameConfigs.SkillLevel_AirTimeDuration);
			return airtimeDuartion; 
		}
	}


	public double DoubleJumpCooltime
	{
		get {
			doubleJumpCooltime = GameStatics.default_DoubleJumpCooltime + (GameStatics.default_IncDoublejumpCooltime * GameConfigs.SkillLevel_DoubleJumpCooltime);
			return doubleJumpCooltime; 
		}
	}


	public int MaxHP
	{
		get {
			maxHP = GameStatics.default_BaseMaxHP + (GameStatics.default_IncHP * GameConfigs.SkillLevel_MaxHP);	
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

	public void SavePlayerMemoryShards()
	{
		GameConfigs.SetCurrentMemoryShards(currentMemoryShards);
	}
}
