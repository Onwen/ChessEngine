using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    public class BitSet
    {
        public ulong bits;

        public BitSet(ulong bits)
        {
            this.bits = bits;
        }

        public void SetBitOff(int bit)
        {
            bits = bits & (~(1ul << bit));
        }
        public void SetBitOn(int bit)
        {
            bits |= (1lu << bit);
        }
        public static BitSet operator &(BitSet a, BitSet b)
        => new BitSet(a.bits & b.bits);
        public static ulong operator &(BitSet a, ulong b)
        => a.bits & b;
        public static BitSet operator |(BitSet a, BitSet b)
        => new BitSet(a.bits | b.bits);
        public static BitSet operator ^(BitSet a, BitSet b)
        => new BitSet(a.bits | b.bits);

        internal bool GetBit(int bit)
        {
            return (bits & (1ul << bit)) > 0;
        }
    }
}
