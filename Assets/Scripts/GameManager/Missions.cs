using System;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Base abstract class used to define a mission the player needs to complete to gain some premium currency.
/// Subclassed for every mission.
/// </summary>
public abstract class MissionBase
{
    // Mission type
    public enum MissionType
    {
        SINGLE_RUN,
        PICKUP,
        MAX
    }

    public float progress;
    public float max;
    public int reward;

    public bool isComplete { get { return (progress / max) >= 1.0f; } }

//    public void Serialize(BinaryWriter w)
//    {
//        w.Write(progress);
//        w.Write(max);
//        w.Write(reward);
//    } 
//
//    public void Deserialize(BinaryReader r)
//    {
//        progress = r.ReadSingle();
//        max = r.ReadSingle();
//        reward = r.ReadInt32();
//    }

	public virtual bool HaveProgressBar() { return true; }

    public abstract void Created();
    public abstract MissionType GetMissionType();
    public abstract string GetMissionDesc();
    public abstract void RunStart(TrackManager manager);
    public abstract void Update(TrackManager manager);

    static public MissionBase GetNewMissionFromType(MissionType type)
    {
        switch (type)
        {
            case MissionType.SINGLE_RUN:
                return new SingleRunMission();
            case MissionType.PICKUP:
                return new PickupMission();
        }

        return null;
    }
}

public class SingleRunMission : MissionBase
{
    public override void Created()
    {
        float[] maxValues = { 500, 1000, 1500, 2000 };
        int choosenVal = Random.Range(0, maxValues.Length);

        reward = choosenVal + 1;
        max = maxValues[choosenVal];
        progress = 0;
    }

	public override bool HaveProgressBar()
	{
		return false;
	}

	public override string GetMissionDesc()
    {
        return "Run " + ((int)max) + "m in a single run";
    }

    public override MissionType GetMissionType()
    {
        return MissionType.SINGLE_RUN;
    }

    public override void RunStart(TrackManager manager)
    {
        progress = 0;
    }

    public override void Update(TrackManager manager)
    {
        progress = manager.worldDistance;
    }
}

public class PickupMission : MissionBase
{
    int previousCoinAmount;

    public override void Created()
    {
        float[] maxValues = { 1000, 2000, 3000, 4000 };
        int choosen = Random.Range(0, maxValues.Length);

        max = maxValues[choosen];
        reward = choosen + 1;
        progress = 0;
    }

    public override string GetMissionDesc()
    {
        return "Pickup " + max + " fishbones";
    }

    public override MissionType GetMissionType()
    {
        return MissionType.PICKUP;
    }

    public override void RunStart(TrackManager manager)
    {
        previousCoinAmount = 0;
    }

    public override void Update(TrackManager manager)
    {
        int coins = manager.characterController.coins - previousCoinAmount;
        progress += coins;

        previousCoinAmount = manager.characterController.coins;
    }
}
