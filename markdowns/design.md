[OBSLETE]
# Scrap this file.
# design.md

Instead of directly generating manufactured stuffs from thin air, the settlement now must process resource they gathered.

Empire.Settlement
## raw resources
Raw Food
    Plant
    Meat
Fabric
    Leather
    Cloth
Stonic
Metallic
Fuel ( Yes wood is a kind of fuel.)
## assemblied resources
Long Term Food
Clothing
    Armor
Weapon
    Melee
    Ranged
    Ammo (Mortar Shell)
    Ammo (CE Compat)

Artifacts

Building Materials
    Comp
Medic Supplies
Electricity
Research Points
Defense Points.

Like stellaris, we now separate settlement with districts and addititonal buildings.
## Districts
Farming. HM> Plant,Meat,Leather,Fuel(Wood),MedicSupplies(Herbal)
Mining. HM> Stonic,Metallic,Fuel(Liquid Fuel,"Coal","charcoal")
Electricity. Fuel > Electricity

Manufacture
    Stonic+Metallic+Fuel > Building Material
    Raw Food > Fuel
    Cloth + Leather + Metallic > Clothing / Armor
    Metallic > Weapon
    Raw Food > Medic Supplies
    Stonic/Cloth > Art =  Silver
Defense
    HM > Defense
    Armor > Defense
    Weap > Defense
    BM > Defense

## Tiered Building
Hydro Array > Hydro Room > Hydro Farm
Mountain / Crust Quarry > Crust Drill > Core Drill
Battery Room > Power Grid > Spaceship Capactor > Space Ship Reactor > Permanent Nuclear Reactor



Weekly Countdown:
Resources first to storage, then consumed by Manufacturing Process, Excessive Amount will be converted to silver.
Silver - Tax1 => Life Support Deficy => Wage => Tax Method 2
! Enough Silver = Angry.

# Code Structre:
```C#

public class Settlement{
    WorldObject WorldRefSettlement;
    string UniqueLoadID;
    //Updated Daily
    SettlementResourceStorage ResourceStorage;
    SettlementDistrictCollection District;
    SettlementBuildings Buildings;
    SettlementBuildQueue;
}


```