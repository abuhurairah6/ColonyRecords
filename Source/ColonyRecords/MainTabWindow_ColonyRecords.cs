using RimWorld;
using UnityEngine;
using Verse;

namespace ColonyRecords
{
    public class MainTabWindow_ColonyRecords : MainTabWindow_PawnTable
    {
        private ScrollPositioner scrollPositioner = new ScrollPositioner();
        private Vector2 rightScrollPosition = Vector2.zero;
        private Vector2 position = Vector2.zero;
        private int curPage = 0;

        protected override PawnTableDef PawnTableDef
        {
            get
            {
                return DefDatabase<PawnPagedTableDef>.GetNamed("CR_Records_" + this.curPage);
            }
        }

        private int AllPageCount
        {
            get
            {
                return DefDatabase<PawnPagedTableDef>.DefCount;
            }
        }

        protected override float ExtraTopSpace
        {
            get
            {
                return 33f;
            }
        }

        private void nextPage()
        {
            if (this.curPage + 1 >= this.AllPageCount)
            {
                return;
            }
            this.curPage = this.curPage + 1;
            base.Notify_ResolutionChanged();
        }

        private void previousPage()
        {
            if (this.curPage <= 0)
            {
                return;
            }
            this.curPage = this.curPage - 1;
            base.Notify_ResolutionChanged();
        }

        public override void DoWindowContents(Rect outRect)
        {
            base.DoWindowContents(outRect);
            if (Event.current.type == EventType.Layout)
            {
                return;
            }

            GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.MiddleCenter;
            Rect rect = new Rect(outRect.x + (outRect.width - Mathf.Min(outRect.width / 3, 180f) * 3), outRect.y, Mathf.Min(outRect.width / 3, 180f), 32f);
            Widgets.Label(rect, "Page " + (this.curPage + 1).ToString() + "/" + this.AllPageCount.ToString());
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;

            if (this.curPage > 0)
            {
                Rect rect2 = new Rect(outRect.x + (outRect.width - Mathf.Min(outRect.width / 3, 180f) * 2), outRect.y, Mathf.Min(outRect.width / 3, 180f), 32f);
                if (Widgets.ButtonText(rect2, "PreviousPage".Translate(), true, true, true))
                {
                    this.previousPage();
                }
            }

            if (this.curPage + 1 < this.AllPageCount)
            {
                Rect rect3 = new Rect(outRect.x + (outRect.width - Mathf.Min(outRect.width / 3, 180f)), outRect.y, Mathf.Min(outRect.width / 3, 180f), 32f);
                if (Widgets.ButtonText(rect3, "NextPage".Translate(), true, true, true))
                {
                    this.nextPage();
                }
            }
        }
    }
}
