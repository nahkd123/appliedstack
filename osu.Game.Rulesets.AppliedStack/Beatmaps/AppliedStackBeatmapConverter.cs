using System.Collections.Generic;
using System.Threading;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.AppliedStack.Objects;
using System.Linq;
using osu.Game.Rulesets.Objects.Types;
using System;

namespace osu.Game.Rulesets.AppliedStack.Beatmaps
{
    public class AppliedStackBeatmapConverter : BeatmapConverter<AppliedStackHitObject>
    {
        public bool AllSoft { get; set; } = false;
        public int HardBeatDivisor { get; set; } = 1;
        
        private bool firstHitObjectProcessed = false;

        public AppliedStackBeatmapConverter(IBeatmap beatmap, Ruleset ruleset)
            : base(beatmap, ruleset)
        {
        }

        public override bool CanConvert() => Beatmap.HitObjects.All(h => h is IHasPosition);

        private AppliedStackHitObject Convert(IList<Audio.HitSampleInfo> samples, double startTime, float objX, float objY, IBeatmap beatmap)
        {
            int seed = BitConverter.ToInt32(BitConverter.GetBytes(objX)) & BitConverter.ToInt32(BitConverter.GetBytes(objY));
            double beatAtMs = beatmap.ControlPointInfo.GetClosestSnappedTime(startTime, HardBeatDivisor);
            bool isAtBeat = Math.Abs(startTime - beatAtMs) <= 5;
            bool hasFinishSample = samples.Any(v => v.Name.Equals("hitfinish"));

            AppliedStackHitObject obj;
            
            if (!firstHitObjectProcessed || (!AllSoft && (isAtBeat || hasFinishSample))) obj = new HardAction();
            else obj = new SoftAction();

            obj.Samples = samples;
            obj.StartTime = startTime;
            obj.ObjectSeed = seed;

            firstHitObjectProcessed = true;
            return obj;
        }

        protected override IEnumerable<AppliedStackHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap, CancellationToken cancellationToken)
        {
            float objX = (original as IHasPosition).X, objY = (original as IHasPosition).Y;

            switch (original)
            {
                case IHasPathWithRepeats hasPath:
                    yield return Convert(hasPath.NodeSamples[0], original.StartTime, objX, objY, beatmap);
                    yield return Convert(hasPath.NodeSamples[hasPath.NodeSamples.Count - 1], hasPath.EndTime, objX, objY, beatmap);
                    break;

                case IHasDuration hasDuration:
                    yield return Convert(original.Samples, original.StartTime, objX, objY, beatmap);
                    yield return Convert(original.Samples, hasDuration.EndTime, objX, objY, beatmap);
                    break;
                
                default:
                    yield return Convert(original.Samples, original.StartTime, objX, objY, beatmap);
                    break;
            }
        }

        protected override Beatmap<AppliedStackHitObject> CreateBeatmap() => new AppliedStackBeatmap();
    }
}
