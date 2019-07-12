# TinyHeap
Solution to Q3 in the screening doc.

I ended up having some difficulty implementing this in C#, mainly due to GC not allowing to free memory on demand. 
I chose to use unmanaged memory but that resulted in not being able to properly chack memory usage and therefore
the testing is incomplete. As such i can not guarantee that the solution is complete/fully functional. In hindsight
i would've written this is C instead.

