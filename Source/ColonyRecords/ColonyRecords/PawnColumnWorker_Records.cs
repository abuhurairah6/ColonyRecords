using RimWorld;
using Verse;
using UnityEngine;

namespace ColonyRecords
{
    public class PawnColumnWorker_Records : PawnColumnWorker
    {
        private RecordDef Record
        {
            get
            {
                return DefDatabase<RecordDef>.GetNamed(this.def.defName);
            }
        }
        
        public override void DoHeader(Rect rect, PawnTable table)
        {
            Text.Font = this.DefaultHeaderFont;
            GUI.color = this.DefaultHeaderColor;
            Text.Anchor = TextAnchor.MiddleCenter;
            Rect rect2 = rect;
            rect2.y += 3f;
            Widgets.Label(rect2, this.def.LabelCap.Resolve());
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.white;
            Text.Font = GameFont.Small;

            if (table.SortingBy == this.def)
            {
                Texture2D texture2D = table.SortingDescending ? ContentFinder<Texture2D>.Get("UI/Icons/SortingDescending", true) : ContentFinder<Texture2D>.Get("UI/Icons/Sorting", true);
                GUI.DrawTexture(new Rect(rect.xMax - (float)texture2D.width - 1f, rect.yMax - (float)texture2D.height - 1f, (float)texture2D.width, (float)texture2D.height), texture2D);
            }
            if (this.def.HeaderInteractable)
            {
                Rect interactableHeaderRect = new Rect(rect.x, rect.yMax - (this.GetMinHeaderHeight(table)), rect.width, this.GetMinHeaderHeight(table));
                if (Mouse.IsOver(interactableHeaderRect))
                {
                    Widgets.DrawHighlight(interactableHeaderRect);
                    string headerTip = this.GetHeaderTip(table);
                    if (!headerTip.NullOrEmpty())
                    {
                        TooltipHandler.TipRegion(interactableHeaderRect, headerTip);
                    }
                }
                if (Widgets.ButtonInvisible(interactableHeaderRect, true))
                {
                    this.HeaderClicked(rect, table);
                }
            }
        }

        public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
        {
            string text;
            if (this.Record.type == RecordType.Time)
            {
                text = pawn.records.GetAsInt(this.Record).ToStringTicksToPeriod(true, false, true, true);
            }
            else
            {
                text = pawn.records.GetValue(this.Record).ToString("0.##");
            }
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect, text);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public override int Compare(Pawn a, Pawn b)
        {
            return this.GetIntValue(a).CompareTo(this.GetIntValue(b));
        }

        protected int GetIntValue(Pawn pawn)
        {
            if (this.Record.type == RecordType.Time)
            {
                return pawn.records.GetAsInt(this.Record);
            }

            return (int) pawn.records.GetValue(this.Record);
        }

        public override int GetMinHeaderHeight(PawnTable table)
        {
            return 55;
        }

        public override int GetMinWidth(PawnTable table)
        {
            return 100;
        }
    }
}
