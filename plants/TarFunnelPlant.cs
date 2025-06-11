//
using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using MycobrickMod.Patches;

// Token: 0x02000182 RID: 386
public class TarFunnelPlant : GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>
{
	// Token: 0x06000789 RID: 1929 RVA: 0x000345F0 File Offset: 0x000327F0
    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
        default_state = this.growing;
        base.serializable = StateMachine.SerializeType.ParamsOnly;
        this.root.EventHandler(GameHashes.OnStorageChange, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshFullnessVariable));
        
        this.growing.InitializeStates(this.masterTarget, this.dead).DefaultState(this.growing.idle);
        this.growing.idle
            .EventTransition(GameHashes.Grow, this.growing.complete, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkMature))
            .EventTransition(GameHashes.Wilt, this.growing.wilted, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkWilted))
            .PlayAnim((TarFunnelPlant.Instance smi) => "grow", KAnim.PlayMode.Paused)
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshGrowingAnimation))
            .Update(new Action<TarFunnelPlant.Instance, float>(TarFunnelPlant.RefreshGrowingAnimationUpdate), UpdateRate.SIM_4000ms, false);
        
        this.growing.complete.PlayAnim("grow_pst", KAnim.PlayMode.Once).OnAnimQueueComplete(this.production);
        
        this.growing.wilted
            .EventTransition(GameHashes.WiltRecover, this.growing.idle, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Not(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkWilted)))
            .PlayAnim(new Func<TarFunnelPlant.Instance, string>(TarFunnelPlant.GetGrowingStatesWiltedAnim), KAnim.PlayMode.Loop);
        
        this.production.InitializeStates(this.masterTarget, this.dead)
            .EventTransition(GameHashes.Grow, this.growing, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Not(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkMature)))
            .ParamTransition<bool>(this.ReadyForHarvest, this.harvest, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.IsTrue)
            .ParamTransition<float>(this.Fullness, this.harvest, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.IsGTEOne)
            .DefaultState(this.production.producing);
        
        this.production.producing
            .EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkWilted))
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshFullnessAnimation))
            .EventHandler(GameHashes.OnStorageChange, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshFullnessAnimation))
			.ToggleStatusItem(ModStatusItems.ProducingPetroleum, (TarFunnelPlant.Instance plant_smi) => plant_smi)
            .Update(new Action<TarFunnelPlant.Instance, float>(TarFunnelPlant.ProductionUpdate), UpdateRate.SIM_200ms, false);
        
        this.production.halted
            .EventTransition(GameHashes.Wilt, this.production.wilted, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkWilted))
            .ToggleStatusItem(ModStatusItems.PetroleumProductionPaused, null)
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshFullnessAnimation));
        
        this.production.wilted
            .EventTransition(GameHashes.WiltRecover, this.production.producing, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Not(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkWilted)))
            .ToggleStatusItem(ModStatusItems.PetroleumProductionWilted, null)
            .PlayAnim("idle_empty", KAnim.PlayMode.Once);
        
        this.harvest.InitializeStates(this.masterTarget, this.dead)
            .EventTransition(GameHashes.Grow, this.growing, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Not(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.IsTrunkMature)))
            .ParamTransition<float>(this.Fullness, this.harvestCompleted, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.IsLTOne)
            .ToggleStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, null)
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.SetReadyToHarvest))
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.EnablePiping))
            .DefaultState(this.harvest.prevented);
        
        this.harvest.prevented
            .PlayAnim("harvest_ready", KAnim.PlayMode.Loop)
            .Toggle("ToggleReadyForHarvest", new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.AddHarvestReadyTag), new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RemoveHarvestReadyTag))
            .EventHandler(GameHashes.EntombedChanged, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.PlayHarvestReadyOnUntentombed))
            .EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.manualHarvest, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.CanBeManuallyHarvested))
            .EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (TarFunnelPlant.Instance smi) => TarFunnelPlant.HasPipeConnected(smi) && smi.IsPipingEnabled)
            .ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (TarFunnelPlant.Instance smi, bool pipeEnable) => pipeEnable && TarFunnelPlant.HasPipeConnected(smi));
        
        this.harvest.manualHarvest.DefaultState(this.harvest.manualHarvest.awaitingForFarmer)
            .Toggle("ToggleReadyForHarvest", new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.AddHarvestReadyTag), new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RemoveHarvestReadyTag))
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.ShowSkillRequiredStatusItemIfSkillMissing))
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.StartHarvestWorkChore))
            .EventHandler(GameHashes.EntombedChanged, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.PlayHarvestReadyOnUntentombed))
            .EventTransition(GameHashes.HarvestDesignationChanged, this.harvest.prevented, GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Not(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.Transition.ConditionCallback(TarFunnelPlant.CanBeManuallyHarvested)))
            .EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.pipes, (TarFunnelPlant.Instance smi) => TarFunnelPlant.HasPipeConnected(smi) && smi.IsPipingEnabled)
            .ParamTransition<bool>(this.PipingEnabled, this.harvest.pipes, (TarFunnelPlant.Instance smi, bool pipeEnable) => pipeEnable && TarFunnelPlant.HasPipeConnected(smi))
            .WorkableCompleteTransition(new Func<TarFunnelPlant.Instance, Workable>(this.GetWorkable), this.harvest.farmerWorkCompleted)
            .Exit(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.CancelHarvestWorkChore))
            .Exit(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.HideSkillRequiredStatusItemIfSkillMissing));
        
        this.harvest.manualHarvest.awaitingForFarmer
            .PlayAnim("harvest_ready", KAnim.PlayMode.Loop)
            .WorkableStartTransition(new Func<TarFunnelPlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.farmerWorking);
        
        this.harvest.manualHarvest.farmerWorking
            .WorkableStopTransition(new Func<TarFunnelPlant.Instance, Workable>(this.GetWorkable), this.harvest.manualHarvest.awaitingForFarmer);
        
        this.harvest.farmerWorkCompleted
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.DropInventory));
        
        this.harvest.pipes
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshFullnessAnimation))
            .Toggle("ToggleReadyForHarvest", new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.AddHarvestReadyTag), new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RemoveHarvestReadyTag))
            .PlayAnim("harvest_ready", KAnim.PlayMode.Loop)
            .EventHandler(GameHashes.EntombedChanged, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshOnPipesHarvestAnimations))
            .EventHandler(GameHashes.OnStorageChange, new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.RefreshFullnessAnimation))
            .EventTransition(GameHashes.ConduitConnectionChanged, this.harvest.prevented, (TarFunnelPlant.Instance smi) => !smi.IsPipingEnabled || !TarFunnelPlant.HasPipeConnected(smi))
            .ParamTransition<bool>(this.PipingEnabled, this.harvest.prevented, (TarFunnelPlant.Instance smi, bool pipeEnable) => !pipeEnable || !TarFunnelPlant.HasPipeConnected(smi));
        
        this.harvestCompleted
            .Enter(new StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State.Callback(TarFunnelPlant.UnsetReadyToHarvest))
            .GoTo(this.production);
        
        this.dead.ToggleMainStatusItem(Db.Get().CreatureStatusItems.Dead, null).Enter(delegate(TarFunnelPlant.Instance smi)
        {
            if (!smi.IsWildPlanted && !smi.GetComponent<KPrefabID>().HasTag(GameTags.Uprooted))
            {
                Notifier notifier = smi.gameObject.AddOrGet<Notifier>();
                Notification notification = TarFunnelPlant.CreateDeathNotification(smi);
                notifier.Add(notification, "");
            }
            GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), smi.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
            smi.Trigger(1623392196, null);
            smi.GetComponent<KBatchedAnimController>().StopAndClear();
            UnityEngine.Object.Destroy(smi.GetComponent<KBatchedAnimController>());
        }).ScheduleAction("Delayed Destroy", 0.5f, new Action<TarFunnelPlant.Instance>(TarFunnelPlant.SelfDestroy));
    }

	public Workable GetWorkable(TarFunnelPlant.Instance smi)
	{
		return smi.GetWorkable();
	}

	public static void EnablePiping(TarFunnelPlant.Instance smi)
	{
		smi.SetPipingState(true);
	}

	public static void RefreshFullnessVariable(TarFunnelPlant.Instance smi)
	{
		smi.RefreshFullnessVariable();
	}

	public static void ShowSkillRequiredStatusItemIfSkillMissing(TarFunnelPlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(true);
	}

	public static void HideSkillRequiredStatusItemIfSkillMissing(TarFunnelPlant.Instance smi)
	{
		smi.GetWorkable().SetShouldShowSkillPerkStatusItem(false);
	}

	public static void StartHarvestWorkChore(TarFunnelPlant.Instance smi)
	{
		smi.CreateHarvestChore();
	}

	public static void CancelHarvestWorkChore(TarFunnelPlant.Instance smi)
	{
		smi.CancelHarvestChore();
	}

	public static bool HasPipeConnected(TarFunnelPlant.Instance smi)
	{
		return smi.HasPipeConnected;
	}

	public static bool CanBeManuallyHarvested(TarFunnelPlant.Instance smi)
	{
		return smi.CanBeManuallyHarvested;
	}

	public static void SetReadyToHarvest(TarFunnelPlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(true, smi, false);
	}

	public static void UnsetReadyToHarvest(TarFunnelPlant.Instance smi)
	{
		smi.sm.ReadyForHarvest.Set(false, smi, false);
	}

	public static void RefreshOnPipesHarvestAnimations(TarFunnelPlant.Instance smi)
	{
		if (smi.IsReadyForHarvest)
		{
			return;
		}
		TarFunnelPlant.RefreshFullnessAnimation(smi);
	}

	public static void RefreshFullnessAnimation(TarFunnelPlant.Instance smi)
	{
		smi.RefreshFullnessTreeTrunkAnimation();
	}

	public static void ProductionUpdate(TarFunnelPlant.Instance smi, float dt)
	{
		smi.ProduceUpdate(dt);
	}

	public static void DropInventory(TarFunnelPlant.Instance smi)
	{
		smi.DropInventory();
	}

	public static void AddHarvestReadyTag(TarFunnelPlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(true);
	}

	public static void RemoveHarvestReadyTag(TarFunnelPlant.Instance smi)
	{
		smi.SetReadyForHarvestTag(false);
	}

	public static string GetGrowingStatesWiltedAnim(TarFunnelPlant.Instance smi)
	{
		return smi.GetTrunkWiltAnimation();
	}

	public static void RefreshGrowingAnimation(TarFunnelPlant.Instance smi)
	{
		smi.RefreshGrowingAnimation();
	}

	public static void RefreshGrowingAnimationUpdate(TarFunnelPlant.Instance smi, float dt)
	{
		smi.RefreshGrowingAnimation();
	}

     public static void PlayHarvestReadyOnUntentombed(TarFunnelPlant.Instance smi)
    {
        if (!smi.IsEntombed)
        {
            smi.PlayHarvestReadyAnimation();  // This calls the instance method
        }
    }

	public static bool IsTrunkMature(TarFunnelPlant.Instance smi)
	{
		return smi.IsMature;
	}

	public static bool IsTrunkWilted(TarFunnelPlant.Instance smi)
	{
		return smi.IsWilting;
	}

	public static bool CanNOTProduce(TarFunnelPlant.Instance smi)
	{
		return !TarFunnelPlant.CanProduce(smi);
	}


	public static void SelfDestroy(TarFunnelPlant.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	public static bool CanProduce(TarFunnelPlant.Instance smi)
	{
		return !smi.IsUprooted && !smi.IsWilting && smi.IsMature && !smi.IsReadyForHarvest;
	}

	public static Notification CreateDeathNotification(TarFunnelPlant.Instance smi)
	{
		return new Notification(CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION, NotificationType.Bad, (List<Notification> notificationList, object data) => CREATURES.STATUSITEMS.PLANTDEATH.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false), "/t• " + smi.gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
	}


	public const float WILD_PLANTED_PRODUCTION_SPEED_MODIFIER = 4f;

	public static Tag TarFunnelReadyForHarvest = TagManager.Create("TarFunnelReadyForHarvest");

	public const string GROWN_WILT_ANIM_NAME = "idle_empty";
	public const string WILT_ANIM_NAME = "wilt";
	public const string GROW_ANIM_NAME = "grow";
    public const string GROW_PST_ANIM_NAME = "grow_pst";
	public const string FILL_ANIM_NAME = "grow_fill";
	public const string MANUAL_HARVEST_READY_ANIM_NAME = "harvest_ready";
	private const int FILLING_ANIMATION_FRAME_COUNT = 42;
	private const int WILT_LEVELS = 3;
	private const float PIPING_ENABLE_TRESHOLD = 0.25f;

	public const SimHashes ProductElement = SimHashes.Petroleum;

	public TarFunnelPlant.GrowingState growing;
	public TarFunnelPlant.ProductionStates production;
	public TarFunnelPlant.HarvestStates harvest;

	public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State harvestCompleted;
	public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State dead;

	public StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.BoolParameter ReadyForHarvest;
	public StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.BoolParameter PipingEnabled;
	public StateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.FloatParameter Fullness;


	public class Def : StateMachine.BaseDef
	{
		public float OptimalProductionDuration;
	}

	public class GrowingState : GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.PlantAliveSubState
	{
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State idle;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State complete;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State wilted;
	}

	public class ProductionStates : GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.PlantAliveSubState
	{
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State wilted;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State halted;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State producing;
	}

	public class HarvestStates : GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.PlantAliveSubState
	{
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State wilted;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State prevented;
		public TarFunnelPlant.ManualHarvestStates manualHarvest;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State farmerWorkCompleted;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State pipes;
	}

	public class ManualHarvestStates : GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State
	{
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State awaitingForFarmer;
		public GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.State farmerWorking;
	}

	public new class Instance : GameStateMachine<TarFunnelPlant, TarFunnelPlant.Instance, IStateMachineTarget, TarFunnelPlant.Def>.GameInstance
	{
		public float OptimalProductionDuration
		{
			get
			{
				if (!this.IsWildPlanted)
				{
					return base.def.OptimalProductionDuration;
				}
				return base.def.OptimalProductionDuration * 4f;
			}
		}

		public float CurrentProductionProgress
		{
			get
			{
				return base.sm.Fullness.Get(this);
			}
		}

		public bool IsWilting
		{
			get
			{
				return base.gameObject.HasTag(GameTags.Wilting);
			}
		}

		public bool IsMature
		{
			get
			{
				return this.growingComponent.IsGrown();
			}
		}

		public bool IsReadyForHarvest
		{
			get
			{
				return base.sm.ReadyForHarvest.Get(base.smi);
			}
		}


		public bool CanBeManuallyHarvested
		{
			get
			{
				return this.UserAllowsHarvest && !this.HasPipeConnected;
			}
		}

		public bool UserAllowsHarvest
		{
			get
			{
				return this.harvestDesignatable == null || (this.harvestDesignatable.HarvestWhenReady && this.harvestDesignatable.MarkedForHarvest);
			}
		}

		public bool HasPipeConnected
		{
			get
			{
				return this.conduitDispenser.IsConnected;
			}
		}

		public bool IsUprooted
		{
			get
			{
				return this.uprootMonitor != null && this.uprootMonitor.IsUprooted;
			}
		}

		public bool IsWildPlanted
		{
			get
			{
				return !this.receptacleMonitor.Replanted;
			}
		}

		public bool IsPipingEnabled
		{
			get
			{
				return base.sm.PipingEnabled.Get(this);
			}
		}

		public Workable GetWorkable()
		{
			return this.workable;
		}

		public Instance(IStateMachineTarget master, TarFunnelPlant.Def def) : base(master, def)
		{
		}

		public override void StartSM()
		{
			base.StartSM();
			this.SetPipingState(this.IsPipingEnabled);
			this.RefreshFullnessVariable();
			TarFunnelHarvestWorkable TarFunnelHarvestWorkable = this.workable;
			TarFunnelHarvestWorkable.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(TarFunnelHarvestWorkable.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnManualHarvestWorkableStateChanges));
		}

		private void OnManualHarvestWorkableStateChanges(Workable workable, Workable.WorkableEvent workableEvent)
		{
			return;
		}


		public void SetPipingState(bool enable)
		{
			base.sm.PipingEnabled.Set(enable, this, false);
			this.SetConduitDispenserAbilityToDispense(enable);
		}

		private void SetConduitDispenserAbilityToDispense(bool canDispense)
		{
			this.conduitDispenser.SetOnState(canDispense);
		}

		public void SetReadyForHarvestTag(bool isReady)
		{
			if (isReady)
			{
				base.gameObject.AddTag(TarFunnelPlant.TarFunnelReadyForHarvest);
			}
			else
			{

				base.gameObject.RemoveTag(TarFunnelPlant.TarFunnelReadyForHarvest);
			}
		}

		public void CreateHarvestChore()
		{
			if (this.harvestChore == null)
			{
				this.harvestChore = new WorkChore<TarFunnelHarvestWorkable>(Db.Get().ChoreTypes.Harvest, this.workable, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		public void CancelHarvestChore()
		{
			if (this.harvestChore != null)
			{
				this.harvestChore.Cancel("TarFunnelProduction.CancelHarvestChore()");
				this.harvestChore = null;
			}
		}

		public void ProduceUpdate(float dt)
		{
			if (this.storage.IsFull())
			{	
				return;
			}

			float crudeOilConsumedPerSecond = 0.003333334f; // From your config
			float crudeOilConsumedThisDt = crudeOilConsumedPerSecond * dt;

			float efficiency = 0.75f; // 75%
			float petroleumToProduceThisDt = crudeOilConsumedThisDt * efficiency;
			float massToAdd = Mathf.Min(petroleumToProduceThisDt, this.storage.RemainingCapacity());

			if (massToAdd > 0)
			{
				float lowTemp = ElementLoader.GetElement(SimHashes.Petroleum.CreateTag()).lowTemp;
				float temperature_delta_for_output = 8f;
				float temperature = Mathf.Max(this.pe.Temperature, lowTemp + temperature_delta_for_output);
				this.storage.AddLiquid(SimHashes.Petroleum, massToAdd, temperature, byte.MaxValue, 0, false, true);
				
				RefreshFullnessVariable();
			}
		}

		public void DropInventory()
		{
			List<GameObject> list = new List<GameObject>();
			Storage storage = this.storage;
			bool vent_gas = false;
			bool dump_liquid = false;
			List<GameObject> collect_dropped_items = list;
			storage.DropAll(vent_gas, dump_liquid, default(Vector3), true, collect_dropped_items);
			foreach (GameObject gameObject in list)
			{
				Vector3 position = gameObject.transform.position;
				position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
				gameObject.transform.SetPosition(position);
			}
		}




		public void RefreshFullnessVariable()
		{
			float fullness = this.storage.MassStored() / this.storage.capacityKg;
			base.sm.Fullness.Set(fullness, this, false);
			if (fullness < 0.25f)
			{
				this.SetPipingState(false);
			}
		}

		public float GetProductionSpeed()
		{
			return 0.1f;     // *** adjust production speed
		}

		public string GetTrunkWiltAnimation()
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(this.growing.PercentOfCurrentHarvest() / 0.33333334f), 0, 2);        // *** tweak numbers?
			return "wilt" + (num + 1).ToString();
		}


        public void PlayHarvestReadyAnimation()
        {
            if (this.animController != null)
            {
                this.animController.Play("harvest_ready", KAnim.PlayMode.Loop, 1f, 0f);
            }
        }


		public void RefreshFullnessTreeTrunkAnimation()
		{
			int num = Mathf.FloorToInt(this.CurrentProductionProgress * 42f);
			if (this.animController.currentAnim != "grow_fill")
			{
				this.animController.Play("grow_fill", KAnim.PlayMode.Loop);
				//this.animController.SetPositionPercent(this.CurrentProductionProgress);
				//this.animController.enabled = false;
				this.animController.enabled = true;
				return;
			}
			if (this.animController.currentFrame != num)
			{
				//this.animController.SetPositionPercent(this.CurrentProductionProgress);
				Debug.Log("RefreshFullnessTreeTrunkAnimation");
			}
		}

		// Token: 0x06007E5E RID: 32350 RVA: 0x003193A0 File Offset: 0x003175A0
		public void RefreshGrowingAnimation()
		{
			this.animController.SetPositionPercent(this.growing.PercentOfCurrentHarvest());
		}

        public bool IsEntombed
        {
            get
            {
                return false;
            }
        }


		[MyCmpReq]
		private ReceptacleMonitor receptacleMonitor;
		[MyCmpReq]
		private KBatchedAnimController animController;
		[MyCmpReq]
		private Growing growingComponent;
		[MyCmpReq]
		private ConduitDispenser conduitDispenser;
		[MyCmpReq]
		private Storage storage;
    	[MyCmpReq]
		private TarFunnelHarvestWorkable workable;
		[MyCmpGet]
		private PrimaryElement pe;
		[MyCmpGet]
		private HarvestDesignatable harvestDesignatable;
		[MyCmpGet]
		private UprootedMonitor uprootMonitor;
		[MyCmpGet]
		private Growing growing;

		private Chore harvestChore;

        private static State.Callback DebugEnter(string stateName)
        {
            return smi => Debug.Log($"[TarFunnelPlant] Entering state: {stateName}");
        }

        private static State.Callback DebugExit(string stateName)
        {
            return smi => Debug.Log($"[TarFunnelPlant] Exiting state: {stateName}");
        }
	}
}
