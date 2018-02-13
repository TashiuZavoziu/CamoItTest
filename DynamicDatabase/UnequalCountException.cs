using System;

namespace DynamicDatabase {
    public class UnequalCountException : Exception {
        public UnequalCountException(string seq1, string seq2) : base($"Sequences '{seq1}' and '{seq2}' contains unequal elements counts") {

        }
    }
}
