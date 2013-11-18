﻿using System.Collections.Generic;
using System.Linq;

namespace POEApi.Model
{
    public abstract class GearTypeRunner
    {
        public abstract bool IsCompatibleType(Gear item);
        public abstract string GetBaseType(Gear item);
        public GearType Type { get; set; }

        public GearTypeRunner(GearType gearType)
        {
            this.Type = gearType;
        }
    }

    public class GearTypeRunnerBase : GearTypeRunner
    {
        protected List<string> compatibleTypes;

        public GearTypeRunnerBase(GearType gearType, params string[] compatibleTypes)
            : base(gearType)
        {
            this.compatibleTypes = compatibleTypes.ToList();
        }

        public override bool IsCompatibleType(Gear item)
        {
            foreach (var type in compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return true;

            return false;
        }

        public override string GetBaseType(Gear item)
        {
            foreach (var type in compatibleTypes)
                if (item.TypeLine.Contains(type))
                    return type;

            return null;
        }
    }

    public class RingRunner : GearTypeRunnerBase
    {
        public RingRunner()
            : base(GearType.Ring)
        { }

        public override bool IsCompatibleType(Gear item)
        {
            if (item.TypeLine.Contains("Ring") && !item.TypeLine.Contains("Ringmail"))
                return true;

            return false;
        }
    }

    public class AmuletRunner : GearTypeRunnerBase
    {
        public AmuletRunner()
            : base(GearType.Amulet, "Amulet")
        { }
    }

    public class HelmetRunner : GearTypeRunnerBase
    {
        public HelmetRunner()
            : base(GearType.Helmet, "Helmet", "Circlet", "Cap", "Mask", "Chain Coif", "Casque", "Hood", "Ringmail Coif", "Chainmail Coif", "Ring Coif", "Crown", "Burgonet", "Bascinet", "Pelt", "Hat")
        { }
    }

    public class ChestRunner : GearTypeRunnerBase
    {
        public ChestRunner()
            : base(GearType.Chest, "Robe", "Ringmail", "Bone Armor", "Full Wyrmscale", "Brigandine", "Raiment", "Doublet", "Lordly Plate", "Jacket", "Garb", "Vest", "Jerkin", "Chestplate", "Tunic", "Brigandine",
                                   "Strapped Leather","Coat","Copper Plate","War Plate", "Wild Leather", "Full Scale Armor", "Full Plate", "Full Leather", "Sun Leather", "Arena Plate",
                                   "Lamellar", "Bronze Plate", "Battle Plate", "Frontier Leather", "Sun Plate", "Colosseum Plate", "Hauberk", "Battle Lamellar", "Crypt Armor", "Necromancer Silks",
                                   "Crusader Plate", "Gladiator Plate", "Carnal Armor", "Majestic Plate", "Coronal Leather", "Glorious Leather", "Silken Wrap", "Regalia", "Holy Chainmail", "Devout Chainmail", 
                                   "Crusader Chainmail", "Conquest Chainmail", "Saintly Chainmail", "Golden Plate", "Exquisite Leather", "Astral Plate", "Full Dragonscale", "Destiny Leather", "Glorious plate")
        { }
    }

    public class BeltRunner : GearTypeRunnerBase
    {
        public BeltRunner()
            : base(GearType.Belt, "Belt", "Sash")
        { }
    }

    public class FlaskRunner : GearTypeRunnerBase
    {
        public FlaskRunner()
            : base(GearType.Flask, "Flask")
        { }
    }

    public class MapRunner : GearTypeRunnerBase
    {
        public MapRunner()
            : base(GearType.Unknown, "Map")
        { }
    }

    public class GloveRunner : GearTypeRunnerBase
    {
        public GloveRunner()
            : base(GearType.Gloves, "Glove", "Mitts", "Gauntlets")
        { }
    }

    public class BootRunner : GearTypeRunnerBase
    {
        public BootRunner()
            : base(GearType.Boots, "Greaves", "Slippers", "Boots", "Shoes")
        { }
    }

    public class AxeRunner : GearTypeRunnerBase
    {
        public AxeRunner()
            : base(GearType.Axe, "Axe", "Chopper", "Splitter", "Labrys", "Tomahawk", "Hatchet", "Poleaxe", "Woodsplitter", "Cleaver" )
        { }
    }

    public class ClawRunner : GearTypeRunnerBase
    {
        public ClawRunner()
            : base(GearType.Claw, "Fist", "Awl", "Paw", "Blinder", "Ripper", "Stabber", "Claw", "Gouger")
        { }
    }

    public class BowRunner : GearTypeRunnerBase
    {
        public BowRunner()
            : base(GearType.Bow, "Bow")
        { }
    }

    public class DaggerRunner : GearTypeRunnerBase
    {
        public DaggerRunner()
            : base(GearType.Dagger, "Dagger", "Shank", "Knife", "Stiletto", "Skean", "Poignard", "Ambusher", "Boot Blade", "Kris")
        { }
    }

    public class MaceRunner : GearTypeRunnerBase
    {
        public MaceRunner()
            : base(GearType.Mace, "Club", "Tenderizer", "Mace", "Hammer", "Maul", "Mallet", "Breaker", "Gavel", "Pernarch", "Steelhead", "Piledriver", "Bladed Mace")
        { }
    }

    public class QuiverRunner : GearTypeRunnerBase
    {
        public QuiverRunner()
            : base(GearType.Quiver, "Quiver")
        { }
    }

    public class SceptreRunner : GearTypeRunnerBase
    {
        public SceptreRunner()
            : base(GearType.Sceptre, "Sceptre", "Fetish", "Sekhem")
        { }
    }

    public class StaffRunner : GearTypeRunnerBase
    {
        public StaffRunner()
            : base(GearType.Staff, "Staff", "Gnarled Branch", "Quarterstaff", "Lathi")
        { }
    }

    public class SwordRunner : GearTypeRunnerBase
    {
        public SwordRunner()
            : base(GearType.Sword, "Sword", "sword", "Sabre", "Dusk Blade", "Cutlass", "Baselard", "Gladius", "Variscite Blade", "Vaal Blade", "Midnight Blade", "Corroded Blade", 
                   "Highland Blade", "Ezomyte Blade", "Rusted Spike", "Rapier", "Foil", "Pecoraro", "Estoc", "Twilight Blade")
        { }
    }

    public class ShieldRunner : GearTypeRunnerBase
    {
        public ShieldRunner()
            : base(GearType.Shield, "Shield", "Spiked Bundle", "Buckler" )
        { }
    }

    public class WandRunner : GearTypeRunnerBase
    {
        public WandRunner()
            : base(GearType.Wand, "Wand", "Horn")
        { }
    }
}