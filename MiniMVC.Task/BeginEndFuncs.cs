using System;

namespace MiniMVC.TaskAdapter {
    public class BeginEndFuncs<T> {
        public readonly Func<T, AsyncCallback, IAsyncResult> Begin;
        public readonly Action<IAsyncResult> End;

        public BeginEndFuncs(Func<T, AsyncCallback, IAsyncResult> begin, Action<IAsyncResult> end) {
            Begin = begin;
            End = end;
        }
    }
}