using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ColonyRecords
{
    [StaticConstructorOnStartup]
    static class PawnColumnRecordDefgenerator
    {
        static PawnColumnRecordDefgenerator()
        {
            foreach (PawnColumnDef def in PawnColumnRecordDefgenerator.ImpliedRecordColumnDefs())
            {
                DefGenerator.AddImpliedDef<PawnColumnDef>(def);
            }

            PawnTableDef recordTable = DefDatabase<PawnTableDef>.GetNamed("CR_Records");
            int pageint = 0;
            int i = 0;
            int num = 0;
            while (i < recordTable.columns.Count)
            {
                PawnPagedTableDef page = new PawnPagedTableDef();
                page.defName = "CR_Records_" + pageint.ToString();
                page.workerClass = typeof(PawnTable_PlayerPawns);
                page.columns = new List<PawnColumnDef>();

                page.columns.Add(DefDatabase<PawnColumnDef>.GetNamed("Label"));
                for (int j = 0; j < page.maxColumnPerPage; j++)
                {
                    if (num == recordTable.columns.Count)
                    {
                        break;
                    }
                    page.columns.Add(DefDatabase<PawnColumnDef>.GetNamed("GapTiny"));
                    page.columns.Add(recordTable.columns[num]);
                    num = num + 1;
                }
                page.columns.Add(DefDatabase<PawnColumnDef>.GetNamed("RemainingSpace"));
                DefDatabase<PawnPagedTableDef>.Add(page);
                i = i + page.maxColumnPerPage;
                pageint = pageint + 1;
            }
        }

        private static IEnumerable<PawnColumnDef> ImpliedRecordColumnDefs()
        {
            PawnTableDef recordTable = DefDatabase<PawnTableDef>.GetNamed("CR_Records");
            bool moveWorkTypeLabelDown = true;
            foreach (RecordDef recordDef in (from d in DefDatabase<RecordDef>.AllDefsListForReading
                                                 select d).Reverse<RecordDef>())
            {
                PawnColumnDef pawnColumnDef = new PawnColumnDef();
                pawnColumnDef.defName = recordDef.defName;
                pawnColumnDef.workerClass = typeof(PawnColumnWorker_Records);
                pawnColumnDef.label = recordDef.label;
                pawnColumnDef.headerTip = recordDef.LabelCap + "\n" + recordDef.description;
                pawnColumnDef.sortable = true;
                pawnColumnDef.width = 30;
                pawnColumnDef.moveWorkTypeLabelDown = !moveWorkTypeLabelDown;
                recordTable.columns.Insert(recordTable.columns.FindIndex((PawnColumnDef x) => x.Worker is PawnColumnWorker_Label) + 1, pawnColumnDef);
                yield return pawnColumnDef;
            }
            yield break;
        }
    }
}