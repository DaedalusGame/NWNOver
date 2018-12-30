using NWNOver.TwoDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver
{
    public class TwoDASchemaDatabase
    {
        ContentEnvironment Environment;
        Dictionary<string, TwoDASchema> SchemaLookup = new Dictionary<string, TwoDASchema>();

        public TwoDASchemaDatabase(ContentEnvironment environment)
        {
            Environment = environment;
        }

        public IEnumerable<TwoDASchema> GetAllSchemas()
        {
            return SchemaLookup.Values.Distinct().Select(x => { x.SetEnvironment(Environment); return x; });
        }

        public void AddSchema(string filename, TwoDASchema schema)
        {
            SchemaLookup.Add(filename, schema);
            if(schema.Name == null)
                schema.Name = filename;
        }

        public TwoDASchema GetSchema(string filename)
        {
            filename = filename.ToLower();
            TwoDASchema schema = new TwoDASchema();
            if (SchemaLookup.ContainsKey(filename))
                schema = SchemaLookup[filename];
            schema.SetEnvironment(Environment);
            return schema;
        }

        public void Setup()
        {
            TwoDASchema currentSchema;

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            currentSchema.AddColumn(new StrRefColumn(3));
            currentSchema.AddColumn(new StrRefColumn(4));
            currentSchema.AddColumn(new StrRefColumn(5));
            currentSchema.AddColumn(new FileRefColumn(6, "{0}.tga"));
            currentSchema.AddColumn(new FileRefColumn(8, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(9, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(10, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(11, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(12, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(14, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(15, "{0}.2da"));
            currentSchema.AddColumn(new BoolColumn(16));
            currentSchema.AddColumn(new BoolColumn(17));
            currentSchema.AddColumn(new EnumColumn(24, new string[] { "STR", "DEX", "CON", "INT", "WIS", "CHA", null }));
            var flagInfoAlignment = new FlagInfo("X2");
            flagInfoAlignment.AddFlag("Neutral", 0x01);
            flagInfoAlignment.AddFlag("Lawful", 0x02);
            flagInfoAlignment.AddFlag("Chaotic", 0x04);
            flagInfoAlignment.AddFlag("Good", 0x08);
            flagInfoAlignment.AddFlag("Evil", 0x10);
            currentSchema.AddColumn(new FlagsColumn(25, flagInfoAlignment));
            var flagInfoAlignmentType = new FlagInfo("X1");
            flagInfoAlignmentType.AddFlag("Law-Chaos", 0x01);
            flagInfoAlignmentType.AddFlag("Good-Evil", 0x02);
            flagInfoAlignmentType.AddFlag("Both", 0x03);
            currentSchema.AddColumn(new FlagsColumn(26, flagInfoAlignmentType));
            currentSchema.AddColumn(new BoolColumn(27));
            currentSchema.AddColumn(new FileRefColumn(49, "{0}.2da"));
            currentSchema.AddColumn(new BoolColumn(51));
            currentSchema.AddColumn(new TwoDARefColumn(55, "packages.2da"));
            currentSchema.AddExtraData(new ExtraAlignmentRestrictionData(25,26,27));
            AddSchema("classes", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            currentSchema.AddColumn(new StrRefColumn(3));
            currentSchema.AddColumn(new TwoDARefColumn(4, "classes.2da"));
            currentSchema.AddColumn(new EnumColumn(5, new string[] { "STR", "DEX", "CON", "INT", "WIS", "CHA", null }));
            currentSchema.AddColumn(new TwoDARefColumn(7, "spellschools.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(8, "domains.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(9, "domains.2da"));
            currentSchema.AddColumn(new FileRefColumn(11, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(12, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(13, "{0}.2da"));
            currentSchema.AddColumn(new FileRefColumn(14, "{0}.2da"));
            currentSchema.AddColumn(new BoolColumn(16));
            AddSchema("packages", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(3));
            currentSchema.AddColumn(new TwoDARefColumn(4, "spellschools.2da"));
            currentSchema.AddColumn(new StrRefColumn(5));
            AddSchema("spellschools", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            currentSchema.AddColumn(new StrRefColumn(3));
            currentSchema.AddColumn(new TwoDARefColumn(5, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(6, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(7, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(8, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(9, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(10, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(11, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(12, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(13, "spells.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(14, "feat.2da"));
            currentSchema.AddColumn(new BoolColumn(15));
            AddSchema("domains", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            AddSchema("spells", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            AddSchema("feat", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            AddSchema("poison", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            AddSchema("disease", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new EnumColumn(2, new string[] { "ARCSPELL", "BAB", "CLASSOR", "CLASSNOT", "FEAT", "FEATOR", "RACE", "SAVE", "SKILL", "SPELL", "VAR" }));
            AddSchema("cls_pres_*", currentSchema);
            AddSchema("cls_pres_archer", currentSchema);
            AddSchema("cls_pres_asasin", currentSchema);
            AddSchema("cls_pres_blkgrd", currentSchema);
            AddSchema("cls_pres_divcha", currentSchema);
            AddSchema("cls_pres_dradis", currentSchema);
            AddSchema("cls_pres_dwdef", currentSchema);
            AddSchema("cls_pres_harper", currentSchema);
            AddSchema("cls_pres_kensei", currentSchema);
            AddSchema("cls_pres_palema", currentSchema);
            AddSchema("cls_pres_pdk", currentSchema);
            AddSchema("cls_pres_shadow", currentSchema);
            AddSchema("cls_pres_shiftr", currentSchema);
            AddSchema("cls_pres_wm", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 2;
            currentSchema.AddColumn(new StrRefColumn(1));
            currentSchema.AddColumn(new IntColumn(3));
            currentSchema.AddColumn(new IntColumn(4));
            var flagInfoSlots = new FlagInfo("X5");
            flagInfoSlots.AddFlag("Helmet", 0x00001);
            flagInfoSlots.AddFlag("Armor", 0x00002);
            flagInfoSlots.AddFlag("Boots", 0x00004);
            flagInfoSlots.AddFlag("Gloves", 0x00008);
            flagInfoSlots.AddFlag("Mainhand", 0x00010);
            flagInfoSlots.AddFlag("Offhand", 0x00020);
            flagInfoSlots.AddFlag("Cloak", 0x00040);
            flagInfoSlots.AddFlag("Ring 1", 0x00080);
            flagInfoSlots.AddFlag("Ring 2", 0x00100);
            flagInfoSlots.AddFlag("Rings", 0x00180);
            flagInfoSlots.AddFlag("Amulets", 0x00200);
            flagInfoSlots.AddFlag("Belt", 0x00400);
            flagInfoSlots.AddFlag("Arrows", 0x00800);
            flagInfoSlots.AddFlag("Bullets", 0x01000);
            flagInfoSlots.AddFlag("Bolts", 0x02000);
            flagInfoSlots.AddFlag("Creature Weapon 1", 0x04000);
            flagInfoSlots.AddFlag("Creature Weapon 2", 0x08000);
            flagInfoSlots.AddFlag("Creature Weapon 3", 0x10000);
            flagInfoSlots.AddFlag("Creature Weapons", 0x1C000);
            flagInfoSlots.AddFlag("Creature Armor", 0x20000);
            currentSchema.AddColumn(new FlagsColumn(5, flagInfoSlots));
            currentSchema.AddColumn(new BoolColumn(6));
            currentSchema.AddColumn(new EnumColumn(7, new string[] { "0", "1", "2", "3", null }, new Dictionary<string, string>()
            {
                { "0" , "0 - simple item (miscs, ...)" },
                { "1" , "1 - colored item (cloaks, ...)" },
                { "2" , "2 - configurable item (weapons, ...)" },
                { "3" , "3 - armor" },
            }));
            currentSchema.AddColumn(new BoolColumn(9));
            var valuesEnvMap = new string[] { "0", "1", null };
            var descEnvMap = new Dictionary<string, string>()
            {
                { "0" , "0 - transparency" },
                { "1" , "1 - reflectiveness" },
            };
            currentSchema.AddColumn(new EnumColumn(10, valuesEnvMap, descEnvMap));
            currentSchema.AddColumn(new EnumColumn(11, valuesEnvMap, descEnvMap));
            currentSchema.AddColumn(new EnumColumn(12, valuesEnvMap, descEnvMap));
            currentSchema.AddColumn(new FileRefColumn(13, "{0}.mdl"));
            currentSchema.AddColumn(new FileRefColumn(14, "{0}.tga"));
            currentSchema.AddColumn(new BoolColumn(15));
            currentSchema.AddColumn(new EnumColumn(16, new string[] { "0", "1", "4", "5", "6", "7", "8", "9", "10", "11", null }, new Dictionary<string, string>()
            {
                { "0" , "0 - one-handed weapon" },
                { "1" , "1 - not wieldable" },
                { "4" , "4 - two-handed weapon" },
                { "5" , "5 - bow" },
                { "6" , "6 - crossbow" },
                { "7" , "7 - shield" },
                { "8" , "8 - double-sided weapon" },
                { "9" , "9 - creature weapon" },
                { "10" , "10 - dart or sling" },
                { "11" , "11 - shuriken or throwing axe" },
            }));
            currentSchema.AddColumn(new EnumColumn(17, new string[] { "1", "2", "3", "4", "5", null }, new Dictionary<string, string>()
            {
                { "1" , "1 - piercing" },
                { "2" , "2 - bludgeoning" },
                { "3" , "3 - slashing" },
                { "4" , "4 - piercing-slashing" },
                { "5" , "5 - bludgeoning-piercing" },
            }));
            currentSchema.AddColumn(new EnumColumn(18, new string[] { "1", "2", "3", "4", null }, new Dictionary<string, string>()
            {
                { "1" , "1 - tiny" },
                { "2" , "2 - small" },
                { "3" , "3 - medium" },
                { "4" , "4 - large" },
            }));
            currentSchema.AddColumn(new TwoDARefColumn(19, "baseitems.2da"));
            currentSchema.AddColumn(new FloatColumn(20));
            currentSchema.AddColumn(new IntColumn(21));
            currentSchema.AddColumn(new IntColumn(22));
            currentSchema.AddColumn(new IntColumn(23));
            currentSchema.AddColumn(new IntColumn(24));
            currentSchema.AddColumn(new IntColumn(25));
            currentSchema.AddColumn(new IntColumn(26));
            currentSchema.AddColumn(new IntColumn(28));
            currentSchema.AddColumn(new IntColumn(29));
            currentSchema.AddColumn(new FloatColumn(30));
            currentSchema.AddColumn(new StrRefColumn(31));
            currentSchema.AddColumn(new TwoDARefColumn(32, "inventorysnds.2da"));
            currentSchema.AddColumn(new IntColumn(33));
            currentSchema.AddColumn(new IntColumn(34));
            currentSchema.AddColumn(new EnumColumn(36, new string[] { "0", "1", "2", "3", "4", null }, new Dictionary<string, string>()
            {
                { "0" , "0 - armor and clothing" },
                { "1" , "1 - weapons" },
                { "2" , "2 - potions and scrolls" },
                { "3" , "3 - wands and magic items" },
                { "4" , "4 - miscellaneous" },
            }));
            currentSchema.AddColumn(new TwoDARefColumn(37, "feat.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(38, "feat.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(39, "feat.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(40, "feat.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(41, "feat.2da"));
            currentSchema.AddColumn(new EnumColumn(42, new string[] { "0", "1", "2", "3", "4", null }, new Dictionary<string, string>()
            {
                { "0" , "0 - dodge" },
                { "1" , "1 - natural" },
                { "2" , "2 - armor" },
                { "3" , "3 - shield" },
                { "4" , "4 - deflect" },
            }));
            currentSchema.AddColumn(new IntColumn(43));
            currentSchema.AddColumn(new IntColumn(44));
            currentSchema.AddColumn(new StrRefColumn(45));
            currentSchema.AddColumn(new IntColumn(46));
            currentSchema.AddColumn(new IntColumn(48));
            currentSchema.AddColumn(new TwoDARefColumn(49, "weaponsounds.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(50, "ammunitiontypes.2da"));
            currentSchema.AddColumn(new IntColumn(52));
            currentSchema.AddColumn(new IntColumn(53));
            currentSchema.AddColumn(new IntColumn(54));
            currentSchema.AddColumn(new IntColumn(55));
            currentSchema.AddColumn(new IntColumn(56));
            currentSchema.AddColumn(new IntColumn(57));
            AddSchema("baseitems", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(3));
            currentSchema.AddColumn(new StrRefColumn(4));
            AddSchema("hen_*", currentSchema);
            AddSchema("hen_companion", currentSchema);
            AddSchema("hen_familiar", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.AddColumn(new IntColumn(1));
            AddSchema("cls_atk_*", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.AddColumn(new IntColumn(1));
            AddSchema("cls_bfeat_*", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new TwoDARefColumn(2,"feat.2da"));
            currentSchema.AddColumn(new EnumColumn(3, new string[] { "0", "1", "2", "3", null }, new Dictionary<string, string>()
            {
                { "0" , "0 - Selectable on level-up" },
                { "1" , "1 - General feat or bonus feat" },
                { "2" , "2 - Bonus feat" },
                { "3" , "3 - Automatically granted" },
            }));
            currentSchema.AddColumn(new IntColumn(4));
            currentSchema.AddColumn(new EnumColumn(5, new string[] { "0", "1", "2", null }, new Dictionary<string, string>()
            {
                { "0" , "0 - Does not appear" },
                { "1" , "1 - Class radial menu" },
                { "2" , "2 - Epic spells menu" },
            }));
            AddSchema("cls_feat_*", currentSchema);
            AddSchema("cls_feat_aber", currentSchema);
            AddSchema("cls_feat_archer", currentSchema);
            AddSchema("cls_feat_asasin", currentSchema);
            AddSchema("cls_feat_barb", currentSchema);
            AddSchema("cls_feat_bard", currentSchema);
            AddSchema("cls_feat_blkgrd", currentSchema);
            AddSchema("cls_feat_cheat", currentSchema);
            AddSchema("cls_feat_cler", currentSchema);
            AddSchema("cls_feat_comm", currentSchema);
            AddSchema("cls_feat_crea", currentSchema);
            AddSchema("cls_feat_divcha", currentSchema);
            AddSchema("cls_feat_dradis", currentSchema);
            AddSchema("cls_feat_drag", currentSchema);
            AddSchema("cls_feat_druid", currentSchema);
            AddSchema("cls_feat_dwdef", currentSchema);
            AddSchema("cls_feat_fey", currentSchema);
            AddSchema("cls_feat_fight", currentSchema);
            AddSchema("cls_feat_gian", currentSchema);
            AddSchema("cls_feat_harper", currentSchema);
            AddSchema("cls_feat_kensei", currentSchema);
            AddSchema("cls_feat_monk", currentSchema);
            AddSchema("cls_feat_outs", currentSchema);
            AddSchema("cls_feat_pal", currentSchema);
            AddSchema("cls_feat_palema", currentSchema);
            AddSchema("cls_feat_pdk", currentSchema);
            AddSchema("cls_feat_rang", currentSchema);
            AddSchema("cls_feat_rog", currentSchema);
            AddSchema("cls_feat_shadow", currentSchema);
            AddSchema("cls_feat_shiftr", currentSchema);
            AddSchema("cls_feat_sorc", currentSchema);
            AddSchema("cls_feat_wiz", currentSchema);
            AddSchema("cls_feat_wm", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.AddColumn(new IntColumn(1));
            currentSchema.AddColumn(new IntColumn(2));
            currentSchema.AddColumn(new IntColumn(3));
            currentSchema.AddColumn(new IntColumn(4));
            AddSchema("cls_savthr_*", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new TwoDARefColumn(2,"skills.2da"));
            currentSchema.AddColumn(new BoolColumn(3));
            AddSchema("cls_skill_*", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.AddColumn(new IntColumn(1));
            currentSchema.AddColumn(new IntColumn(2));
            currentSchema.AddColumn(new IntColumn(3));
            currentSchema.AddColumn(new IntColumn(4));
            currentSchema.AddColumn(new IntColumn(5));
            currentSchema.AddColumn(new IntColumn(6));
            currentSchema.AddColumn(new IntColumn(7));
            currentSchema.AddColumn(new IntColumn(8));
            currentSchema.AddColumn(new IntColumn(9));
            currentSchema.AddColumn(new IntColumn(10));
            currentSchema.AddColumn(new IntColumn(11));
            currentSchema.AddColumn(new IntColumn(12));
            AddSchema("cls_spgn_*", currentSchema);
            AddSchema("cls_spgn_bard", currentSchema);
            AddSchema("cls_spgn_cler", currentSchema);
            AddSchema("cls_spgn_dru", currentSchema);
            AddSchema("cls_spgn_pal", currentSchema);
            AddSchema("cls_spgn_rang", currentSchema);
            AddSchema("cls_spgn_sorc", currentSchema);
            AddSchema("cls_spgn_wiz", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.AddColumn(new IntColumn(1));
            currentSchema.AddColumn(new IntColumn(2));
            currentSchema.AddColumn(new IntColumn(3));
            currentSchema.AddColumn(new IntColumn(4));
            currentSchema.AddColumn(new IntColumn(5));
            currentSchema.AddColumn(new IntColumn(6));
            currentSchema.AddColumn(new IntColumn(7));
            currentSchema.AddColumn(new IntColumn(8));
            currentSchema.AddColumn(new IntColumn(9));
            currentSchema.AddColumn(new IntColumn(10));
            currentSchema.AddColumn(new IntColumn(11));
            AddSchema("cls_spkn_*", currentSchema);
            AddSchema("cls_spkn_bard", currentSchema);
            AddSchema("cls_spkn_sorc", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            currentSchema.AddColumn(new EnumColumn(6, new string[] { "N", "R", "G", "Y", "W", null }, new Dictionary<string, string>()
            {
                { "N", "N - None" },
                { "R", "R - Red" },
                { "G", "G - Green" },
                { "Y", "Y - Yellow" },
                { "W", "W - White" },
            }));
            currentSchema.AddColumn(new EnumColumn(7, new string[] { "P", "S", "F", "L", "SW", "FW", "LW", "ST", "FT", "LT", "SWT", "FWT", "LWT", null }, new Dictionary<string, string>()
            {
                { "P", "P - Part-based" },
                { "S", "S - Simple" },
                { "F", "F - Full-sized" },
                { "L", "L - Large" },
                { "SW", "S - Simple (Wings)" },
                { "FW", "F - Full-sized (Wings)" },
                { "LW", "L - Large (Wings)" },
                { "ST", "S - Simple (Tails)" },
                { "FT", "F - Full-sized (Tails)" },
                { "LT", "L - Large (Tails)" },
                { "SWT", "S - Simple (Wings and Tails)" },
                { "FWT", "F - Full-sized (Wings and Tails)" },
                { "LWT", "L - Large (Wings and Tails)" },
                { "s", "s - Placeable (?)" },
            }));
            currentSchema.AddColumn(new FloatColumn(8));
            currentSchema.AddColumn(new FloatColumn(9));
            currentSchema.AddColumn(new FloatColumn(10));
            currentSchema.AddColumn(new FloatColumn(11));
            currentSchema.AddColumn(new EnumColumn(20, new string[] { "H", "L", null }));
            currentSchema.AddColumn(new BoolColumn(21));
            currentSchema.AddColumn(new BoolColumn(23));
            currentSchema.AddColumn(new BoolColumn(24));
            currentSchema.AddColumn(new TwoDARefColumn(26,"creaturesize.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(28, "footstepsounds.2da"));
            currentSchema.AddColumn(new TwoDARefColumn(29, "appearancesndset.2da"));
            currentSchema.AddColumn(new BoolColumn(30));
            currentSchema.AddColumn(new TwoDARefColumn(34, "bodybag.2da"));
            currentSchema.AddColumn(new BoolColumn(35));
            AddSchema("appearance", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(2));
            currentSchema.AddColumn(new TwoDARefColumn(3, "placeables.2da"));
            AddSchema("bodybag", currentSchema);

            currentSchema = new TwoDASchema();
            currentSchema.LabelColumn = 1;
            currentSchema.AddColumn(new StrRefColumn(3));
            AddSchema("creaturesize", currentSchema);
        }
    }
}
