using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.SignalRDataModels {
    public class MoveCardModel {
        public int SourcePileVersionNo = -1,
            SourcePileNo = -1,
            SourceIndex = -1,
            TargetPileVersionNo = -1,
            TargetPileNo = -1,
            TargetIndex = -1;

        public MoveCardModel(int sourcePileVersionNo, int sourcePileNo, int sourceIndex, int targetPileVersionNo, int targetPileNo, int targetIndex) {
            SourcePileVersionNo = sourcePileVersionNo;
            SourcePileNo = sourcePileNo;
            SourceIndex = sourceIndex;
            TargetPileVersionNo = targetPileVersionNo;
            TargetPileNo = targetPileNo;
            TargetIndex = targetIndex;
        }

        public override string ToString() {
            return $@"SourcePileNo:  {SourcePileNo}
SourceIndex:   {SourceIndex}
SourcePileVer: {SourcePileVersionNo}
TargetPileNo:  {TargetPileNo}
TargetIndex:   {TargetIndex}
TargetPileVer: {TargetPileVersionNo}";
        }
    }
}
