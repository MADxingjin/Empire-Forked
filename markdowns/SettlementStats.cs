using namespace Empire;

public class SStatCategoryDef{

}

/// <summary>
/// Settlement Stat Def.
/// Based on Rimworld.StatDef + some twist.
/// Shorten to SStatDef
/// </summary>
/// <value></value>
public class SStatDef{
public SStatCategoryDef category;
public int displayPriorityInCategory;
public bool showZeroBaseValue;
public StorageCate ProductLinkStorageCate;
public StorageCate ProductLinkStorageCateBlackList;
public Dict<List<StorageCate>,int> ProductLinkStorageCateBlackListTechBased;

}

/// <summary>
/// Parser needs to be able to distinguish base/offset/mult .
/// </summary>
/// <value></value>
public static SStatDefOf{
    ProdWood;
    ProdQuarry;
    ProdFarm;
    ProdLivestock;
    ProdManufactured;
    ProdWeapon;
    ProdArmor;
    ProdPower;
    ProdResearch;
    CivUnrest;
    CivUnrestGain;
    CivUnrestFall;
    CivMilPower;
    CivMilPowerRise;
    CivGovernorTrust;
    CivSlaveSuppress;



}
public SettlementBuildingDef{
    List<SStat> Stats;
    
}