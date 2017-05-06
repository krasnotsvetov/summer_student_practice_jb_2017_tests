using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PEExtractor.Common
{
    public static class Either
    {
        private sealed class LeftImpl<L, R> : IEither<L, R>
        {
            private readonly L value;

            public LeftImpl(L value)
            {
                this.value = value;
            }

            public U Case<U>(Func<L, U> ofLeft, Func<R, U> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));

                return ofLeft(value);
            }

            public void Case(Action<L> ofLeft, Action<R> ofRight)
            {
                if (ofLeft == null)
                    throw new ArgumentNullException(nameof(ofLeft));


                ofLeft(value);
            }
        }

        private sealed class RightImpl<L, R> : IEither<L, R>
        {
            private readonly R value;

            public RightImpl(R value)
            {
                this.value = value;
            }

            public U Case<U>(Func<L, U> ofLeft, Func<R, U> ofRight)
            {
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));

                return ofRight(value);
            }

            public void Case(Action<L> ofLeft, Action<R> ofRight)
            {
                if (ofRight == null)
                    throw new ArgumentNullException(nameof(ofRight));


                ofRight(value);
            }
        }

      

        public static IEither<L, R> Left<L, R>(L value)
        {
            return new LeftImpl<L, R>(value);
        }

        public static IEither<L, R> Right<L, R>(R value)
        {
            return new RightImpl<L, R>(value);
        }
    }
}
