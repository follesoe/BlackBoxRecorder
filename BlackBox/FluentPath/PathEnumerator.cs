// Copyright © Microsoft Corporation.  All Rights Reserved.
// This code released under the terms of the 
// Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)


using System;
using System.Collections;
using System.Collections.Generic;

namespace BlackBox.FluentPath {
    public class PathEnumerator : IEnumerator<Path> {
        private IEnumerator<string> _pathEnumerator;

        public PathEnumerator(IEnumerable<string> paths) {
            _pathEnumerator = paths.GetEnumerator();
        }

        Path IEnumerator<Path>.Current {
            get { return new Path(_pathEnumerator.Current); }
        }

        void IDisposable.Dispose() {
            _pathEnumerator.Dispose();
        }

        object IEnumerator.Current {
            get { return new Path(_pathEnumerator.Current); }
        }

        bool IEnumerator.MoveNext() {
            return _pathEnumerator.MoveNext();
        }

        void IEnumerator.Reset() {
            _pathEnumerator.Reset();
        }
    }
}


