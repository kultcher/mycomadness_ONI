using System;
using TUNING;

// Token: 0x02000183 RID: 387
public class TarFunnelHarvestWorkable : Workable
{
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Harvesting);
		this.workAnims = new HashedString[]
		{
			"syrup_harvest_trunk_pre",      // leaving anims, hopefully these are just anims for the dupes and not the plant?
			"syrup_harvest_trunk_loop"
		};
		this.workingPstComplete = new HashedString[]
		{
			"syrup_harvest_trunk_pst"
		};
		this.workingPstFailed = new HashedString[]
		{
			"syrup_harvest_trunk_loop"
		};
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_syrup_tree_kanim")
		};
		this.synchronizeAnims = true;
		this.shouldShowSkillPerkStatusItem = false;
		base.SetWorkTime(10f);
		this.resetProgressOnStop = true;
	}

	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	protected override void OnStartWork(WorkerBase worker)
	{
		base.OnStartWork(worker);
	}
}
