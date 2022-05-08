using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.AppliedStack.Replays;
using osu.Game.Rulesets.Mods;

namespace osu.Game.Rulesets.AppliedStack.Mods
{
    public class AppliedStackModAutoplay : ModAutoplay
    {
        public override ModReplayData CreateReplayData(IBeatmap beatmap, IReadOnlyList<Mod> mods)
            => new ModReplayData(new AppliedStackAutoGenerator(beatmap).Generate(), new ModCreatedUser { Username = "Very Dumb Autoplay Bot" });
    }
}
